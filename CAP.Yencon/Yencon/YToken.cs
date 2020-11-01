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
	///  ヱンコンの字句を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YToken
	{
		/// <summary>
		///  現在の字句を<see cref="CAP.Yencon.YNode"/>へ変換します。
		/// </summary>
		/// <returns>
		///  <see cref="CAP.Yencon.YNode"/>オブジェクトです。
		/// </returns>
		public abstract YNode AsNode();
	}
}
