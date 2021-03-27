/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  非同期関数の戻り値を表します。
	/// </summary>
	[AsyncMethodBuilder(typeof(IAsyncMethodBuilder))]
	public interface IAsyncMethodResult : IAwaitable, IAsyncResult
	{
		/// <summary>
		///  操作が正常に完了した場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsCompletedSuccessfully
#if NETCOREAPP3_1_OR_GREATER
			=> this.IsCompleted && this.Exception is null;
#else
			{ get; }
#endif

		/// <summary>
		///  操作の実行に失敗した場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsFailed
#if NETCOREAPP3_1_OR_GREATER
			=> this.IsCompleted && this.Exception is not null;
#else
			{ get; }
#endif


		/// <summary>
		///  操作が中止された場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsCancelled
#if NETCOREAPP3_1_OR_GREATER
			=> this.IsCompleted && this.Exception is OperationCanceledException;
#else
			{ get; }
#endif


		/// <summary>
		///  失敗の原因となった例外を取得します。
		/// </summary>
		/// <remarks>
		///  成功した場合は<see langword="null"/>を返します。
		/// </remarks>
		public Exception? Exception { get; }

		/// <summary>
		///  この<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult"/>を構成します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元に実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </param>
		/// <returns>構成された待機可能なオブジェクトです。</returns>
		public IAwaitable ConfigureAwait(bool continueOnCapturedContext);
	}

	/// <summary>
	///  非同期関数の戻り値を表します。
	/// </summary>
	/// <typeparam name="TResult">戻り値の種類です。</typeparam>
	[AsyncMethodBuilder(typeof(IAsyncMethodBuilder<>))]
	public interface IAsyncMethodResult<out TResult> : IAsyncMethodResult, IAwaitable<TResult>
	{
		/// <summary>
		///  この<see cref="TakymLib.Threading.Tasks.IAsyncMethodResult"/>を構成します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元に実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>です。
		/// </param>
		/// <returns>構成された待機可能なオブジェクトです。</returns>
		public new IAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext);

#if NETCOREAPP3_1_OR_GREATER
		IAwaitable IAsyncMethodResult.ConfigureAwait(bool continueOnCapturedContext)
		{
			return this.ConfigureAwait(continueOnCapturedContext);
		}
#endif
	}
}
