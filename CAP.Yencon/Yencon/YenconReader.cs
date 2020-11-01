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
	///  ヱンコンファイルを読み込むリーダーを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YenconReader : DisposableBase
	{
		/// <summary>
		///  上書きされた場合、現在の字句を取得します。
		/// </summary>
		/// <returns>
		///  <see cref="CAP.Yencon.YenconReader.Read"/>を一度も実行していない、または、ストリームの終端まで読み進めた場合は
		///  <see langword="null"/>を返します。
		/// </returns>
		public abstract YToken? Current { get; }

		/// <summary>
		///  上書きされた場合、字句を読み取ります。
		/// </summary>
		public abstract void Read();
	}
}
