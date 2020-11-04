/****
 * Yencon - "Yencon Environment Configuration"
 *    「ヱンコン環境設定ファイル」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using CAP.Properties;

namespace CAP.Yencon
{
	/// <summary>
	///  ヱンコン内の単一のノードを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class YNode
	{
		private YDocument? _doc;
		private string?    _link_string;

		/// <summary>
		///  このノードの親セクションまたは親配列を取得します。
		/// </summary>
		public YNode? Parent { get; }

		/// <summary>
		///  このノードの名前を取得します。
		/// </summary>
		public string Name { get; }

		/// <summary>
		///  このノードが根セクションかどうか判定します。
		/// </summary>
		/// <returns>
		///  根ノードである場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public bool IsRoot => this.Parent == null;

		/// <summary>
		///  型'<see cref="CAP.Yencon.YNode"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="parent">
		///  新しいノードの親セクションまたは親配列です。
		///  このノードが根セクションの場合は<see langword="null"/>になります。
		/// </param>
		/// <param name="name">新しいノードの名前です。</param>
		/// <exception cref="System.ArgumentNullException"/>
		/// <exception cref="System.ArgumentException"/>
		protected YNode(YNode? parent, string name)
		{
			if (parent == null && !(this is YSection)) {
				throw new ArgumentNullException(nameof(parent), string.Format(Resources.YNode_ArgumentNullException, nameof(parent)));
			}
			if (!(parent is YSection) || !(parent is YArray)) {
				throw new ArgumentException(string.Format(Resources.YNode_ArgumentException, nameof(parent)), nameof(parent));
			}
			if (name == null) {
				throw new ArgumentNullException(nameof(name));
			}
			this.Parent = parent;
			this.Name   = name;
		}

		/// <summary>
		///  このノードを保持する文書オブジェクトを取得します。
		/// </summary>
		/// <returns><see cref="CAP.Yencon.YDocument"/>オブジェクトです。</returns>
		public YDocument? GetDocument()
		{
			if (_doc == null) {
				if (this is YDocument doc) {
					_doc = doc;
				} else {
					_doc = this.Parent?.GetDocument();
				}
			}
			return _doc;
		}

		/// <summary>
		///  このノードのリンク文字列を取得します。
		/// </summary>
		/// <returns>このノードへのリンクを表す文字列です。</returns>
		public string GetLink()
		{
			if (this.Parent == null) {
				return this.Name;
			} else {
				if (_link_string == null) {
					_link_string = $"{this.Parent.GetLink()}.{this.Name}";
				}
				return _link_string;
			}
		}
	}
}
