/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;

namespace TakymLib.Text
{
	/// <summary>
	///  東アジアの文字幅の種類を表します。
	/// </summary>
	public enum EastAsianWidthType
	{
		/// <summary>
		///  無効な文字幅を表します。
		/// </summary>
		/// <remarks>
		///  この値を直接利用しないでください。
		/// </remarks>
		[Obsolete("この値を直接利用しないでください。", DiagnosticId = "TakymLib_EAWInvalid")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Invalid,

		/// <summary>
		///  曖昧な文字幅を表します。
		/// </summary>
		Ambiguous,

		/// <summary>
		///  全角文字を表します。
		/// </summary>
		FullWidth,

		/// <summary>
		///  半角文字を表します。
		/// </summary>
		HalfWidth,

		/// <summary>
		///  中立な文字幅を表します。
		/// </summary>
		Neutral,

		/// <summary>
		///  狭い文字幅を表します。
		/// </summary>
		Narrow,

		/// <summary>
		///  広い文字幅を表します。
		/// </summary>
		Wide
	}
}
