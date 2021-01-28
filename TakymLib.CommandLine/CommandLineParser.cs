/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.CommandLine
{
	/// <summary>
	///  コマンド行引数を解析します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class CommandLineParser
	{
		private readonly string[] _args;

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineParser"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="args">コマンド行引数です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected CommandLineParser(string[] args)
		{
			args.EnsureNotNull(nameof(args));
			_args = args;
		}

		/// <summary>
		///  コマンド行引数の解析を実行します。
		/// </summary>
		public void Parse()
		{
			var pr   = ParseCore(new ArrayAsyncEnumerator(_args), true).ConfigureAwait(false).GetAwaiter().GetResult();
			var task = this.ParseCore(pr);
			if (!task.IsCompleted) {
				task.ConfigureAwait(false).GetAwaiter().GetResult(); // 1回のみ実行可能
			}
		}

		/// <summary>
		///  コマンド行引数の解析を非同期的に実行します。
		/// </summary>
		/// <returns>この処理の非同期操作です。</returns>
		public async Task ParseAsync()
		{
			var pr = await ParseCore(new ArrayAsyncEnumerator(_args), true).ConfigureAwait(false);
			await this.ParseCore(pr).ConfigureAwait(false);
		}

		private static async Task<ParseResult> ParseCore(IAsyncEnumerable<string> args, bool readCommand)
		{
			var result = new ParseResult();
			await using (var e = args.GetAsyncEnumerator()) {
				if (readCommand && await e.MoveNextAsync()) {
					string cmd = e.Current.Trim().ToLower();
					if (cmd == "/?"    || cmd == "-?"    || cmd == "--?"    || cmd == "?" ||
						cmd == "/help" || cmd == "-help" || cmd == "--help" ||
						cmd == "/h"    || cmd == "-h"    || cmd == "--h") {
						result.Command = "help";
					} else {
						result.Command = cmd;
					}
				}
				while (await e.MoveNextAsync()) {
					string arg = e.Current.Trim();
					if (!string.IsNullOrEmpty(arg)) {
						string argData = arg.Substring(1);
						switch (arg[0]) { // argType = arg[0]
						case '/':
							result.AddSwitch(argData);
							break;
						case '-':
							result.AddOption(argData);
							break;
						case '@':
							if (File.Exists(argData)) {
								async IAsyncEnumerable<string> LoadFile(string fname)
								{
									var fs = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read);
									await using (fs.ConfigureAwait(false)) {
										using (var sr = new StreamReader(fs, Encoding.UTF8, true, -1, true)) {
											while (await sr.ReadLineAsync() is string line) {
												yield return line;
											}
										}
									}
								}
								result.Combine(await ParseCore(LoadFile(argData), false));
							}
							break;
						case '#':
							break;
						case '!':
							result.AddValue(argData);
							break;
						default:
							result.AddValue(arg);
							break;
						}
					}
				}
			}
			return result;
		}

		private async ValueTask ParseCore(ParseResult pr)
		{
			await this.OnPreParse(pr.Command);
			var tasks = new List<ValueTask>();
			AddTask(tasks, this.OnParse(null, null, pr.Values.ToArray()));
			int count0 = pr.Options.Count;
			for (int i = 0; i < count0; ++i) {
				var opt = pr.Options[i];
				AddTask(tasks, this.OnParse(null, opt.Name, opt.Values.ToArray()));
			}
			int count1 = pr.Switches.Count;
			for (int i = 0; i < count1; ++i) {
				var    swt    = pr.Switches[i];
				string name_s = swt.Name;
				AddTask(tasks, this.OnParse(name_s, null, swt.Values.ToArray()));
				int count2 = swt.Options.Count;
				for (int j = 0; j < count2; ++j) {
					var opt = swt.Options[j];
					AddTask(tasks, this.OnParse(name_s, opt.Name, opt.Values.ToArray()));
				}
			}
			int count3 = tasks.Count;
			for (int i = 0; i < count3; ++i) {
				await tasks[i].ConfigureAwait(false); // 1回のみ実行可
			}
		}

		private static void AddTask(List<ValueTask> tasks, in ValueTask task)
		{
			if (!task.IsCompleted) {
				tasks.Add(task);
			}
		}

		/// <summary>
		///  上書きされた場合、解析前に呼び出されます。
		/// </summary>
		/// <param name="subCommand">子コマンドです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		protected abstract ValueTask OnPreParse(string? subCommand);

		/// <summary>
		///  上書きされた場合、解析時に呼び出されます。
		/// </summary>
		/// <param name="switchName">スイッチ名です。</param>
		/// <param name="optionName">オプション名です。</param>
		/// <param name="values">文字列配列です。</param>
		/// <returns>この処理の非同期操作です。</returns>
		protected abstract ValueTask OnParse(string? switchName, string? optionName, string[] values);

		private sealed class ParseResult
		{
			private  List<string> _current_vs;
			private  List<Option> _current_os;
			internal string?      Command  { get; set; }
			internal List<string> Values   { get; }
			internal List<Option> Options  { get; }
			internal List<Switch> Switches { get; }

			internal ParseResult()
			{
				this.Values   = new List<string>();
				this.Options  = new List<Option>();
				this.Switches = new List<Switch>();
				_current_vs   = this.Values;
				_current_os   = this.Options;
			}

			internal void AddValue(string value)
			{
				_current_vs.Add(value);
			}

			internal void AddSwitch(string name)
			{
				var s = new Switch(name);
				_current_vs = s.Values;
				_current_os = s.Options;
				this.Switches.Add(s);
			}

			internal void AddOption(string name)
			{
				var o = new Option(name);
				_current_vs = o.Values;
				_current_os.Add(o);
			}

			internal void Combine(ParseResult parseResult)
			{
				this.Values  .AddRange(parseResult.Values);
				this.Options .AddRange(parseResult.Options);
				this.Switches.AddRange(parseResult.Switches);
			}
		}

		private sealed class Switch
		{
			internal string       Name    { get; }
			internal List<string> Values  { get; }
			internal List<Option> Options { get; }

			internal Switch(string name)
			{
				this.Name    = name;
				this.Values  = new List<string>();
				this.Options = new List<Option>();
			}
		}

		private sealed class Option
		{
			internal string       Name   { get; }
			internal List<string> Values { get; }

			internal Option(string name)
			{
				this.Name   = name;
				this.Values = new List<string>();
			}
		}

		private struct ArrayAsyncEnumerator : IAsyncEnumerable<string>, IAsyncEnumerator<string>
		{
			private readonly int      _mtid;
			private readonly string[] _array;
			private          int      _index;

			public string Current => _array[_index];

			internal ArrayAsyncEnumerator(string[] array)
			{
				_mtid  = Thread.CurrentThread.ManagedThreadId;
				_array = array;
				_index = -1;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				++_index;
				return new ValueTask<bool>(_index < _array.Length);
			}

			public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
			{
				if (Thread.CurrentThread.ManagedThreadId == _mtid) {
					return this;
				} else {
					throw new InvalidOperationException();
				}
			}

			public ValueTask DisposeAsync()
			{
				return default;
			}
		}
	}
}
