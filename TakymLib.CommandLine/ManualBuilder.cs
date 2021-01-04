﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Reflection;
using System.Text;
using TakymLib.CommandLine.Properties;

namespace TakymLib.CommandLine
{
	/// <summary>
	///  コマンド行引数説明書を生成します。
	/// </summary>
	public class ManualBuilder
	{
		private readonly StringBuilder        _sb;
		private readonly CommandLineConverter _conv;
		private          string?              _appName;

		/// <summary>
		///  書き込み先の文字列バッファを取得します。
		/// </summary>
		protected StringBuilder Writer => _sb;

		/// <summary>
		///  型'<see cref="TakymLib.CommandLine.ManualBuilder"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="converter">コマンド行引数の解析と変換を行うオブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public ManualBuilder(CommandLineConverter converter)
		{
			converter.EnsureNotNull(nameof(converter));
			_sb   = new StringBuilder();
			_conv = converter;
		}

		/// <summary>
		///  バージョン情報を書き込みます。
		///  プログラム名も設定されます。
		/// </summary>
		/// <param name="asm">アセンブリ情報を表すオブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void WriteVersion(Assembly asm)
		{
			this.WriteVersion(new VersionInfo(asm));
		}

		/// <summary>
		///  バージョン情報を書き込みます。
		///  プログラム名も設定されます。
		/// </summary>
		/// <param name="ver">バージョン情報を表すオブジェクトです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void WriteVersion(VersionInfo ver)
		{
			ver.EnsureNotNull(nameof(ver));
			_appName = ver.Assembly.GetName().Name;
			_sb.Append(ver.GetCaption());
			_sb.Append(" - ");
			_sb.AppendLine(Resources.ManualBuilder_WriteVersion);
			_sb.AppendLine(ver.Copyright);
			_sb.AppendLine();
		}

		/// <summary>
		///  使用法を表す文字列を書き込みます。
		/// </summary>
		/// <param name="usage">使用法を表す文字列です。</param>
		public void WriteUsage(string usage)
		{
			if (_appName is null) {
				_sb.AppendLine(string.Format(Resources.ManualBuilder_WriteUsage, usage));
			} else {
				_sb.AppendLine(string.Format(Resources.ManualBuilder_WriteUsage, $"{_appName} {usage}"));
			}
		}

		/// <summary>
		///  使用法を書き込む時に利用されるプログラム名を設定します。
		/// </summary>
		/// <param name="appName">プログラム名を表す文字列です。</param>
		public void SetProgramName(string? appName)
		{
			_appName = appName;
		}

		/// <summary>
		///  コマンド行引数説明書の生成を開始します。
		/// </summary>
		public void Build()
		{
			_sb.AppendLine();
			foreach (object swt in _conv.GetAll()) {
				var     type     = swt.GetType();
				string? sname    = type.GetCustomAttribute<SwitchAttribute>()?.Name;
				var     provider = swt as IHelpProvider;
				if (sname is not null) {
					if (provider is not null) {
						_sb.AppendLine(string.Format(Resources.ManualBuilder_Build_Switch_Desc, "/" + sname));
						_sb.Append("  ");
						provider.WriteHelp(_sb, null);
						_sb.AppendLine();
					}
					_sb.AppendLine(string.Format(Resources.ManualBuilder_Build_Switch_Options, "/" + sname));
					var props = type.GetProperties();
					for (int i = 0; i < props.Length; ++i) {
						WriteOption(props[i], provider);
					}
					var fields = type.GetFields();
					for (int i = 0; i < fields.Length; ++i) {
						WriteOption(fields[i], provider);
					}
					_sb.AppendLine();
				}
			}
#if true
			void WriteOption(MemberInfo mi, IHelpProvider? provider)
			{
				var o = mi.GetCustomAttribute<OptionAttribute>();
				if (o is not null) {
					int pos;
					if (o.ShortName is null) {
						_sb.Append("    ");
						pos = 4;
					} else {
						_sb.Append("  -");
						_sb.Append(o.ShortName);
						pos = 3 + o.ShortName.Length;
					}
					if (pos < 12) {
						_sb.Append(' ', 12 - pos);
					} else {
						_sb.Append("  ");
					}
					_sb.Append("--");
					_sb.Append(o.LongName);
					if (provider is not null) {
						pos = o.LongName.Length;
						if (pos < 24) {
							_sb.Append(' ', 24 - pos);
						} else {
							_sb.Append("  ");
						}
						provider.WriteHelp(_sb, o.LongName);
					}
					_sb.AppendLine();
				}
			}
#else
			void WriteOption(MemberInfo mi, IHelpProvider? provider)
			{
				var o = mi.GetCustomAttribute<OptionAttribute>();
				if (o is not null) {
					if (o.ShortName is null) {
						_sb.Append("    ");
					} else {
						_sb.Append("  -");
						_sb.Append(o.ShortName);
					}
					_sb.Append("\t--");
					_sb.Append(o.LongName);
					if (provider is not null) {
						_sb.Append("\t\t");
						provider.WriteHelp(_sb, o.LongName);
					}
					_sb.AppendLine();
				}
			}
#endif
		}

		/// <summary>
		///  バージョン情報と使用法を書き込み、
		///  コマンド行引数説明書の生成を開始します。
		///  プログラム名も設定されます。
		/// </summary>
		/// <param name="asm">アセンブリ情報を表すオブジェクトです。</param>
		/// <param name="usages">使用法を表す文字列です。複数設定できます。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void BuildFull(Assembly asm, params string[] usages)
		{
			this.BuildFull(new VersionInfo(asm), usages);
		}

		/// <summary>
		///  バージョン情報と使用法を書き込み、
		///  コマンド行引数説明書の生成を開始します。
		///  プログラム名も設定されます。
		/// </summary>
		/// <param name="ver">バージョン情報を表すオブジェクトです。</param>
		/// <param name="usages">使用法を表す文字列です。複数設定できます。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public void BuildFull(VersionInfo ver, params string[] usages)
		{
			usages.EnsureNotNull(nameof(usages));
			this.WriteVersion(ver);
			for (int i = 0; i < usages.Length; ++i) {
				this.WriteUsage(usages[i]);
			}
			this.Build();
		}

		/// <summary>
		///  説明書をコンソール画面に出力します。
		/// </summary>
		public void Print()
		{
			Console.WriteLine(_sb);
		}

		/// <summary>
		///  現在のインスタンスに格納されている説明書の文字列形式を取得します。
		/// </summary>
		/// <returns>コマンド行引数説明書を格納した文字列です。</returns>
		public override string ToString()
		{
			return _sb.ToString();
		}
	}
}
