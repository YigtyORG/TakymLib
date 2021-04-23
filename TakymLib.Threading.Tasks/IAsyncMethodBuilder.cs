/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using TakymLib.Threading.Tasks.Internals;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  非同期関数を生成する機能を提供します。
	/// </summary>
	public interface IAsyncMethodBuilder : ICustomAsyncMethodBuilder<IAsyncMethodResult>
	{
#if NETCOREAPP3_1_OR_GREATER
		/// <summary>
		///  既定の実装を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAsyncMethodBuilder"/>オブジェクトです。</returns>
		public static IAsyncMethodBuilder Create()
		{
			return new DefaultAsyncMethodBuilder<VoidResult>();
		}
#endif
	}

	/// <summary>
	///  非同期関数を生成する機能を提供します。
	/// </summary>
	/// <typeparam name="TResult">戻り値の種類です。</typeparam>
	public interface IAsyncMethodBuilder<TResult> : IAsyncMethodBuilder, ICustomAsyncMethodBuilder<IAsyncMethodResult<TResult>, TResult>
	{
#if NETCOREAPP3_1_OR_GREATER
		/// <summary>
		///  既定の実装を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAsyncMethodBuilder"/>オブジェクトです。</returns>
		public new static IAsyncMethodBuilder<TResult> Create()
		{
			return new DefaultAsyncMethodBuilder<TResult>();
		}
#endif

		/// <summary>
		///  <see cref="TakymLib.Threading.Tasks.IAsyncMethodResult{TResult}"/>を取得します。
		/// </summary>
		public new IAsyncMethodResult<TResult> Task { get; }
		
#if NETCOREAPP3_1_OR_GREATER
		IAsyncMethodResult          ICustomAsyncMethodBuilder<IAsyncMethodResult>         .Task => this.Task;
		IAsyncMethodResult<TResult> ICustomAsyncMethodBuilder<IAsyncMethodResult<TResult>>.Task => this.Task;

		void ICustomAsyncMethodBuilder<IAsyncMethodResult>.SetResult()
		{
			this.SetResult(default);
		}
#endif
	}
}
