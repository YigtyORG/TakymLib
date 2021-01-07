/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.ComponentModel;

namespace TakymLib.Logging
{
	/// <summary>
	///  <see cref="TakymLib.Logging.ErrorReportBuilder"/>にH-RESULT情報を提供します。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class HResultDetailProvider : ICustomErrorDetailProvider
	{
		/// <summary>
		///  型'<see cref="TakymLib.Logging.HResultDetailProvider"/>'の新しいインスタンスを生成します。
		/// </summary>
		public HResultDetailProvider() { }

		/// <summary>
		///  追加情報を可読な翻訳済みの文字列へ変換します。
		/// </summary>
		/// <param name="exception">変換するデータを保持している例外オブジェクトです。</param>
		/// <returns>翻訳済みの文字列です。</returns>
		public string GetLocalizedDetail(Exception exception)
		{
			string result = $"H-RESULT Message: {new Win32Exception(exception.HResult).Message}";
			if (exception is Win32Exception w32e) {
				return result + Environment.NewLine
					+ $"H-RESULT Message (Win32): {new Win32Exception(w32e.NativeErrorCode).Message}";
			} else {
				return result;
			}
		}
	}
}
