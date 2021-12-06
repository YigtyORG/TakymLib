/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TakymLib.UI
{
	/// <summary>
	///  <see cref="System.Windows.UIElement"/>を<see cref="System.Windows.Forms.Form"/>として扱います。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class ElementWindow : Form
	{
		private readonly ElementHost _eh;

		/// <summary>
		///  <see cref="System.Windows.UIElement"/>オブジェクトを取得します。
		/// </summary>
		public UIElement UIElement => _eh.Child;

		/// <inheritdoc/>
		public override string Text
		{
			get => _eh?.Text ?? base.Text;
			set
			{
				base.Text = value;
				if (_eh is not null) _eh.Text = value;
			}
		}

		/// <summary>
		///  型'<see cref="ElementWindow"/>'の新しいインスタンスを生成します。
		/// </summary>
		/// <param name="uiElement"><see cref="System.Windows.UIElement"/>オブジェクトを指定します。</param>
		/// <exception cref="ArgumentNullException"/>
		public ElementWindow(UIElement uiElement)
		{
			if (uiElement is null) throw new ArgumentNullException(nameof(uiElement));
			this.SuspendLayout();
			_eh        = new();
			_eh.Dock   = DockStyle.Fill;
			_eh.Child  = uiElement;
			_eh.Parent = this;
			this.ResumeLayout(false);
		}
	}
}
