/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using CAP.Yencon.Extensions;

namespace CAP.Yencon
{
	/// <summary>
	///  ヱンコン環境設定ファイルを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YDocument
	{
		/// <summary>
		///  根セクションを取得します。
		/// </summary>
		public abstract YSection Root { get; }

		/// <summary>
		///  指定したリーダーからヱンコン情報を読み取ります。
		/// </summary>
		/// <param name="reader">読み込み元のリーダーです。</param>
		public abstract void Load(YenconReader reader);

		/// <summary>
		///  指定したライターへヱンコン情報を書き込みます。
		/// </summary>
		/// <param name="writer">書き込み先のライターです。</param>
		public abstract void Save(YenconWriter writer);

		/// <summary>
		///  指定されたリンク文字列からノードを取得します。
		/// </summary>
		/// <param name="link">リンク文字列です。</param>
		/// <returns>
		///  指定されたリンク文字列からノードを取得できた場合はそのノードを表すオブジェクト、
		///  それ以外の場合は<see langword="null"/>を返します。
		/// </returns>
		/// <exception cref="System.ArgumentNullException"/>
		public YNode? GetNodeByLink(string link)
		{
			if (link == null) {
				throw new ArgumentNullException(nameof(link));
			}
			string[] names = link.Split('.');
			if (names.Length > 0 && names[0] == this.Root.Name) {
				var section = this.Root;
				if (names.Length == 1) {
					return section;
				}
				for (int i = 1; i < names.Length - 1; ++i) {
					var section2 = section.GetSection(names[i]);
					if (section2 == null) {
						return null;
					} else {
						section = section2;
					}
				}
				if (section.TryGetNode(names[names.Length - 1], out var result)) {
					return result;
				}
			}
			return null;
		}
	}
}
