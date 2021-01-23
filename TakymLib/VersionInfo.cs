/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Reflection;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  <see cref="System.Reflection.Assembly"/>のバージョン情報を提供します。
	/// </summary>
	public class VersionInfo
	{
		/// <summary>
		///  このライブラリ(<see cref="TakymLib"/>)のバージョン情報を取得します。
		/// </summary>
		public static VersionInfo Library { get; } = new(typeof(VersionInfo).Assembly);

		/// <summary>
		///  現在の<see cref="System.AppDomain"/>に読み込まれている全てのアセンブリのバージョン情報をコンソール画面に出力します。
		/// </summary>
		public static void PrintAllAssemblies()
		{
			var asms = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < asms.Length; ++i) {
				new VersionInfo(asms[i]).Print();
			}
		}

		/// <summary>
		///  コンストラクタに渡されたアセンブリ情報を取得します。
		/// </summary>
		public Assembly Assembly { get; }

		/// <summary>
		///  アセンブリの内部名を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  アセンブリの表示名を取得します。
		/// </summary>
		public string DisplayName { get; }

		/// <summary>
		///  アセンブリの作成者情報を取得します。
		/// </summary>
		public string Authors { get; }

		/// <summary>
		///  アセンブリの著作者情報を取得します。
		/// </summary>
		public string Copyright { get; }

		/// <summary>
		///  アセンブリの説明を取得します。
		/// </summary>
		public string Description { get; }

		/// <summary>
		///  アセンブリのバージョン情報を取得します。
		/// </summary>
		public Version? Version { get; }

		/// <summary>
		///  アセンブリの開発コード名を取得します。
		/// </summary>
		public string CodeName { get; }

		/// <summary>
		///  型'<see cref="TakymLib.VersionInfo"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="asm">バージョン情報の取得元のアセンブリです。</param>
		/// <exception cref="System.ArgumentNullException"/>
		public VersionInfo(Assembly asm)
		{
			asm.EnsureNotNull(nameof(asm));
			this.Assembly    = asm;
			this.Name        = asm.GetName().Name ?? "Unknown";
			this.DisplayName = asm.GetCustomAttribute<AssemblyProductAttribute>    ()?.Product     ?? Resources.VersionInfo_DisplayName;
			this.Authors     = asm.GetCustomAttribute<AssemblyCompanyAttribute>    ()?.Company     ?? Resources.VersionInfo_Authors;
			this.Copyright   = asm.GetCustomAttribute<AssemblyCopyrightAttribute>  ()?.Copyright   ?? Resources.VersionInfo_Copyright;
			this.Description = asm.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? Resources.VersionInfo_Description;
			this.Version     = asm.GetName().Version;
			this.CodeName    = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
		}

		/// <summary>
		///  アセンブリのバージョン情報を含む題名を取得します。
		/// </summary>
		/// <returns>題名を表す文字列です。</returns>
		public string GetCaption()
		{
			return $"{this.DisplayName} [{this.GetFullVersionString()}]";
		}

		/// <summary>
		///  バージョン情報の長い形式を取得します。
		/// </summary>
		/// <returns>バージョン情報を表す文字列です。</returns>
		public string GetFullVersionString()
		{
			return $"v{this.GetVersionString()}, cn:{this.CodeName}";
		}

		/// <summary>
		///  バージョン情報の文字列形式を取得します。
		/// </summary>
		/// <returns>バージョン情報を表す文字列です。</returns>
		public string GetVersionString()
		{
			return this.Version?.ToString(4) ?? "?.?.?.?";
		}
	}
}
