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
		private static readonly char[]           _separator     = new[] { ':', '=' };
		private static readonly CommandLineLexer _default_lexer = new();
		private        readonly CommandLineLexer _lexer;
		private        readonly object           _source;

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineParser"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="args">コマンド行引数を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected CommandLineParser(string[] args)
		{
			args.EnsureNotNull(nameof(args));
			_lexer  = _default_lexer;
			_source = args;
		}

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineParser"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="lexer">コマンド行を引数へ分解する字句解析機能を持つオブジェクトを指定します。</param>
		/// <param name="text">コマンド行を表す文字列を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected CommandLineParser(CommandLineLexer? lexer, string text)
		{
			text.EnsureNotNull(nameof(text));
			_lexer  = lexer ?? _default_lexer;
			_source = text;
		}

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.CommandLineParser"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="lexer">コマンド行を引数へ分解する字句解析機能を持つオブジェクトを指定します。</param>
		/// <param name="openReader">コマンド行を格納した<see cref="System.IO.TextReader"/>を生成する関数を指定します。</param>
		/// <exception cref="System.ArgumentNullException"/>
		protected CommandLineParser(CommandLineLexer? lexer, Func<TextReader> openReader)
		{
			openReader.EnsureNotNull(nameof(openReader));
			_lexer  = lexer ?? _default_lexer;
			_source = openReader;
		}

		/// <summary>
		///  コマンド行引数の解析を実行します。
		/// </summary>
		public void Parse()
		{
			ParseResult pr;

			var phase1 = this.ParseCore_Phase1(true);
			if (phase1.IsCompleted) {
				pr = phase1.Result;
			} else {
				pr = phase1.ConfigureAwait(false).GetAwaiter().GetResult(); // 1回のみ実行可
			}

			var phase2 = this.ParseCore_Phase2(pr);
			if (!phase2.IsCompleted) {
				phase2.ConfigureAwait(false).GetAwaiter().GetResult(); // 1回のみ実行可
			}
		}

		/// <summary>
		///  コマンド行引数の解析を非同期的に実行します。
		/// </summary>
		/// <returns>この処理の非同期操作です。</returns>
		public async ValueTask ParseAsync()
		{
			var pr = await this.ParseCore_Phase1(true).ConfigureAwait(false);
			         await this.ParseCore_Phase2(pr)  .ConfigureAwait(false);
		}

		private ValueTask<ParseResult> ParseCore_Phase1(bool readCommand)
		{
			switch (_source) {
			case string[] args:
				return ParseArgs(new ArrayAsyncEnumerator(args), readCommand);
			case string text:
				return ParseArgs(_lexer.ScanAsync(text), readCommand);
			case Func<TextReader> openReader:
				using (var tr = openReader()) {
					return ParseArgs(_lexer.ScanAsync(tr), readCommand);
				}
			default:
				return ValueTask.FromResult<ParseResult>(EmptyParseResult._inst);
			}

			static async ValueTask<ParseResult> ParseArgs<T>(T args, bool readCommand)
				where T: IAsyncEnumerable<string>
			{
				var result = new ConcreteParseResult();
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
							case '+':
								int i = argData.IndexOfAny(_separator);
								if (i >= 0) {
									int j = argData.IndexOfAny(_separator, i + 1);
									if (j >= 0) {
										result.AddSwitch(argData.Substring(0, i));
										++i;
										result.AddOption(argData.Substring(i, j - i));
										++j;
										result.AddOption(argData.Substring(j, argData.Length - j));
									} else {
										result.AddSwitch(argData.Substring(0, i));
										++i;
										result.AddOption(argData.Substring(i, argData.Length - i));
									}
								} else {
									result.AddSwitch(argData);
								}
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
									result.Combine(await ParseArgs(LoadFile(argData), false));
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
		}

		private async ValueTask ParseCore_Phase2(ParseResult pr)
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

			static void AddTask(List<ValueTask> tasks, in ValueTask task)
			{
				if (!task.IsCompleted) {
					tasks.Add(task);
				}
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

		private abstract class ParseResult
		{
			internal abstract string?      Command  { get; set; }
			internal abstract List<string> Values   { get; }
			internal abstract List<Option> Options  { get; }
			internal abstract List<Switch> Switches { get; }

			internal abstract void AddValue (string value);
			internal abstract void AddSwitch(string name);
			internal abstract void AddOption(string name);
			internal abstract void Combine  (ParseResult parseResult);
		}

		private sealed class EmptyParseResult : ParseResult
		{
			internal static readonly EmptyParseResult _inst = new();

			internal override string? Command
			{
				get => null;
				set { }
			}

			internal override List<string> Values   => EmptyListCache<string>.Get();
			internal override List<Option> Options  => EmptyListCache<Option>.Get();
			internal override List<Switch> Switches => EmptyListCache<Switch>.Get();

			private EmptyParseResult() { }

			internal override void AddOption(string name)  { }
			internal override void AddSwitch(string name)  { }
			internal override void AddValue (string value) { }
			internal override void Combine  (ParseResult parseResult) { }

			private static class EmptyListCache<T>
			{
				[ThreadStatic()]
				private static List<T>? _cache;

				internal static List<T> Get()
				{
					if (_cache is null || _cache.Count != 0) {
						_cache = new(0);
					}
					return _cache;
				}
			}
		}

		private sealed class ConcreteParseResult : ParseResult
		{
			private List<string> _current_vs;
			private List<Option> _current_os;

			internal override string?      Command  { get; set; }
			internal override List<string> Values   { get; }
			internal override List<Option> Options  { get; }
			internal override List<Switch> Switches { get; }

			internal ConcreteParseResult()
			{
				this.Values   = new List<string>();
				this.Options  = new List<Option>();
				this.Switches = new List<Switch>();
				_current_vs   = this.Values;
				_current_os   = this.Options;
			}

			internal override void AddValue(string value)
			{
				_current_vs.Add(value);
			}

			internal override void AddSwitch(string name)
			{
				var s = new Switch(name);
				_current_vs = s.Values;
				_current_os = s.Options;
				this.Switches.Add(s);
			}

			internal override void AddOption(string name)
			{
				var o = new Option(name);
				_current_vs = o.Values;
				_current_os.Add(o);
			}

			internal override void Combine(ParseResult parseResult)
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
			private readonly int               _mtid;
			private readonly string[]          _array;
			private          int               _index;
			private          CancellationToken _ct;

			public string Current => _array[_index];

			internal ArrayAsyncEnumerator(string[] array)
			{
				_mtid  = Thread.CurrentThread.ManagedThreadId;
				_array = array;
				_index = -1;
				_ct    = default;
			}

			public ValueTask<bool> MoveNextAsync()
			{
				if (_ct.IsCancellationRequested) {
					return ValueTask.FromCanceled<bool>(_ct);
				}
				++_index;
				return ValueTask.FromResult(_index < _array.Length);
			}

			public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
			{
				if (Thread.CurrentThread.ManagedThreadId == _mtid) {
					if (_ct == default) {
						_ct = cancellationToken;
					} else {
						_ct = CancellationTokenSource.CreateLinkedTokenSource(_ct, cancellationToken).Token;
					}
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
