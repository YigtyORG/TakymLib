/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace CAP.Yencon
{
	/// <summary>
	///  ヱンコン内の単一のノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YNode
	{
		/// <summary>
		///  このノードの名前を取得します。
		/// </summary>
		public abstract string Name { get; }
	}
}
