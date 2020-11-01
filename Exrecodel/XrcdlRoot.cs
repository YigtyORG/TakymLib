/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;

namespace Exrecodel
{
	/// <summary>
	///  <see cref="Exrecodel"/>文書の他の全ての要素を含む根要素を表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class XrcdlRoot : XrcdlNode
	{
		/// <summary>
		///  この根要素の全ての直接の子要素を読み取り専用で取得します。
		/// </summary>
		public abstract IReadOnlyList<XrcdlNode> Children { get; }

		internal XrcdlRoot(XrcdlDocument document) : base(document, document) { }
	}
}
