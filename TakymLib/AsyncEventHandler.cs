
/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  イベントを処理する非同期関数を表します。
	/// </summary>
	/// <param name="sender">イベントの発生源です。</param>
	/// <param name="e">空のイベントデータを格納しているオブジェクトです。</param>
	/// <returns>この処理の非同期操作です。</returns>
	public delegate ValueTask AsyncEventHandler(object? sender, EventArgs e);

	/// <summary>
	///  イベントを処理する非同期関数を表します。
	/// </summary>
	/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
	/// <param name="sender">イベントの発生源です。</param>
	/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
	/// <returns>この処理の非同期操作です。</returns>
	public delegate ValueTask AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e) where TEventArgs: EventArgs;

	/// <summary>
	///  イベントを処理する非同期関数を表します。
	/// </summary>
	/// <typeparam name="TSender">発生源の種類です。</typeparam>
	/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
	/// <param name="sender">イベントの発生源です。</param>
	/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
	/// <returns>この処理の非同期操作です。</returns>
	public delegate ValueTask AsyncEventHandler<TSender, TEventArgs>(TSender? sender, TEventArgs e) where TEventArgs: EventArgs;

	/// <summary>
	///  型'<see cref="TakymLib.AsyncEventHandler"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class AsyncEventHandlerExtensions
	{
#pragma warning disable CA2012 // ValueTask を正しく使用する必要があります
		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">空のイベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.MemberAccessException"/>
		public static async ValueTask Dispatch(this AsyncEventHandler? handler, object? sender, EventArgs e)
		{
			if (handler is null) return;
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1回のみ実行可
				}
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.MemberAccessException"/>
		public static async ValueTask Dispatch<TEventArgs>(this AsyncEventHandler<TEventArgs>? handler, object? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) return;
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler<TEventArgs>)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1回のみ実行可
				}
			}
		}

		/// <summary>
		///  指定されたイベントを発火させます。
		/// </summary>
		/// <remarks>
		///  実行順序は保証されません。
		/// </remarks>
		/// <typeparam name="TSender">発生源の種類です。</typeparam>
		/// <typeparam name="TEventArgs">イベントデータの種類です。</typeparam>
		/// <param name="handler">実行するイベントハンドラです。<see langword="null"/>の場合は実行されません。</param>
		/// <param name="sender">イベントの発生源です。</param>
		/// <param name="e">イベントデータを格納しているオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.MemberAccessException"/>
		public static async ValueTask Dispatch<TSender, TEventArgs>(this AsyncEventHandler<TSender, TEventArgs>? handler, TSender? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			if (handler is null) return;
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler<TSender, TEventArgs>)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				var task = tasks[i];
				if (!task.IsCompleted) {
					await task; // 1回のみ実行可
				}
			}
		}
#pragma warning restore CA2012 // ValueTask を正しく使用する必要があります
	}
}
