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
		///  参照型オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="sender">オブジェクトの送信を行うスレッドの実行文脈です。</param>
		/// <param name="obj">送信する参照型オブジェクトです。</param>
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
		///  参照型オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="obj">送信する参照型オブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public async ValueTask SendObject(object obj)
		{
			await this.SendObject(null, obj);
		}

		/// <summary>
		///  別スレッドから参照型オブジェクトを受信します。
		///  また、送信元スレッドの実行文脈も取得します。
		/// </summary>
		/// <returns>送信元情報と受信した参照型オブジェクトを含む非同期操作です。</returns>
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
		///  別スレッドから参照型オブジェクトを受信します。
		/// </summary>
		/// <returns>受信した参照型オブジェクトを含む非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		public async ValueTask<object> ReceiveObject()
		{
			return (await this.ReceiveObjectWithSender()).obj;
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <param name="disposing">
		///  マネージドオブジェクトとアンマネージオブジェクト両方を破棄する場合は<see langword="true"/>、
		///  アンマネージオブジェクトのみを破棄する場合は<see langword="false"/>を設定します。
		/// </param>
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
