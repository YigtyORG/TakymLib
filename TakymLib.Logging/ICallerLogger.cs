/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;

namespace TakymLib.Logging
{
	/// <summary>
	///  呼び出し元の情報をログ出力する機能を提供します。
	/// </summary>
	public interface ICallerLogger
	{
		/// <summary>
		///  処理の開始をログに出力します。
		/// </summary>
		/// <param name="memberName">呼び出し元の名前です。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスです。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号です。</param>
		public void Begin(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1
		);

		/// <summary>
		///  処理の開始をログに出力します。
		/// </summary>
		/// <param name="message">出力するメッセージです。</param>
		/// <param name="memberName">呼び出し元の名前です。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスです。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号です。</param>
		/// <param name="messageExpression">メッセージの式表現です。</param>
		public void Begin(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);

		/// <summary>
		///  処理の開始をログに出力します。
		/// </summary>
		/// <typeparam name="T">メッセージの種類です。</typeparam>
		/// <param name="message">出力するメッセージです。</param>
		/// <param name="memberName">呼び出し元の名前です。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスです。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号です。</param>
		/// <param name="messageExpression">メッセージの式表現です。</param>
		public void Begin<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);

		/// <summary>
		///  処理の終了をログに出力します。
		/// </summary>
		/// <param name="memberName">呼び出し元の名前です。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスです。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号です。</param>
		public void End(
			[CallerMemberName()] string memberName = "",
			[CallerFilePath()]   string filePath   = "",
			[CallerLineNumber()] int    lineNumber = -1
		);

		/// <summary>
		///  処理の終了をログに出力します。
		/// </summary>
		/// <param name="message">出力するメッセージです。</param>
		/// <param name="memberName">呼び出し元の名前です。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスです。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号です。</param>
		/// <param name="messageExpression">メッセージの式表現です。</param>
		public void End(
			                                      object? message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);

		/// <summary>
		///  処理の終了をログに出力します。
		/// </summary>
		/// <typeparam name="T">メッセージの種類です。</typeparam>
		/// <param name="message">出力するメッセージです。</param>
		/// <param name="memberName">呼び出し元の名前です。</param>
		/// <param name="filePath">呼び出し元のコードを格納しているファイルのパスです。</param>
		/// <param name="lineNumber">呼び出し元のコードの行番号です。</param>
		/// <param name="messageExpression">メッセージの式表現です。</param>
		public void End<T>(
			                                      T?      message,
			[CallerMemberName()]                  string  memberName        = "",
			[CallerFilePath()]                    string  filePath          = "",
			[CallerLineNumber()]                  int     lineNumber        = -1,
			[CallerArgumentExpression("message")] string  messageExpression = ""
		);
	}
}
