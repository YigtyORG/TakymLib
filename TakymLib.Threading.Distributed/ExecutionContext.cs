/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TakymLib.Threading.Distributed
{
	/// <summary>
	///  実行文脈情報を表します。
	/// </summary>
	public class ExecutionContext : DisposableBase
	{
		private readonly Queue<(ExecutionContext?, object)> _objs;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Distributed.ExecutionContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ExecutionContext()
		{
			_objs = new();
		}

		/// <summary>
		///  オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="sender">オブジェクトの送信を行うスレッドの実行文脈です。</param>
		/// <param name="obj">送信するオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public ConfiguredTaskAwaitable SendObject(ExecutionContext? sender, object obj)
		{
			this.EnsureNotDisposed();
			return Task.Run(() => {
				lock (_objs) {
					_objs.Enqueue((sender, obj));
				}
			}).ConfigureAwait(false);
		}

		/// <summary>
		///  オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="obj">送信するオブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public async ValueTask SendObject(object obj)
		{
			await this.SendObject(null, obj);
		}

		/// <summary>
		///  別スレッドからオブジェクトを受信します。
		///  また、送信元スレッドの実行文脈も取得します。
		/// </summary>
		/// <returns>送信元情報と受信したオブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public ConfiguredTaskAwaitable<(ExecutionContext? sender, object obj)> ReceiveObjectWithSender()
		{
			this.EnsureNotDisposed();
			return Task.Run(() => {
				while (_objs.Count == 0) ;
				lock (_objs) {
					return _objs.Dequeue();
				}
			}).ConfigureAwait(false);
		}

		/// <summary>
		///  別スレッドからオブジェクトを受信します。
		/// </summary>
		/// <returns>受信したオブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public async ValueTask<object> ReceiveObject()
		{
			return (await this.ReceiveObjectWithSender()).obj;
		}

		/// <inheritdoc/>
		protected override void Dispose(bool disposing)
		{
			if (!this.IsDisposed) {
				lock (_objs) {
					_objs.Clear();
				}
				this.Dispose(disposing);
			}
		}
	}
}
