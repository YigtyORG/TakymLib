/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書内の要素として必要な機能を提供します。
	/// </summary>
	public interface IXrcdlElement
	{
		/// <summary>
		///  現在の要素の名前を取得または設定します。
		/// </summary>
		string Name { get; set; }

		/// <summary>
		///  現在の要素と関連付けられている文書を取得します。
		/// </summary>
		XrcdlDocument Document { get; }
	}
}
