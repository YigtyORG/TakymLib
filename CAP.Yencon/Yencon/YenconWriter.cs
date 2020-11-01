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
	///  ヱンコンファイルを書き込むライターを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YenconWriter : DisposableBase
	{
		/// <summary>
		///  上書きされた場合、ノードを書き込みます。
		/// </summary>
		/// <param name="node">書き込むノードを表すオブジェクトです。</param>
		public abstract void Write(YNode node);
	}
}
