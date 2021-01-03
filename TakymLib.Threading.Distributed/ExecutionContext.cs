/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TakymLib.Threading.Distributed
{
	/// <summary>
	///  実行文脈情報を表します。
	/// </summary>
	public class ExecutionContext
	{
		private readonly Queue<object>    _objs;
		private readonly Queue<ValueType> _vals;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.Distributed.ExecutionContext"/>'の新しいインスタンスを生成します。
		/// </summary>
		public ExecutionContext()
		{
			_objs = new Queue<object>();
			_vals = new Queue<ValueType>();
		}

		/// <summary>
		///  参照型オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="obj">送信する参照型オブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		public Task SendObject(object obj)
		{
			return Task.Run(() => {
				lock (_objs) {
					_objs.Enqueue(obj);
				}
			});
		}

		/// <summary>
		///  値型オブジェクトを別スレッドへ送信します。
		/// </summary>
		/// <param name="val">送信する値型オブジェクトです。</param>
		/// <returns>この処理の非同期操作です。</returns>
		public ValueTask SendValue(ValueType val)
		{
			return new ValueTask(Task.Run(() => {
				lock (_vals) {
					_vals.Enqueue(val);
				}
			}));
		}

		/// <summary>
		///  別スレッドから参照型オブジェクトを受信します。
		/// </summary>
		/// <returns>受信した参照型オブジェクトを含む非同期操作です。</returns>
		public Task<object> ReceiveObject()
		{
			return Task.Run(() => {
				lock (_objs) {
					return _objs.Dequeue();
				}
			});
		}

		/// <summary>
		///  別スレッドから値型オブジェクトを受信します。
		/// </summary>
		/// <returns>受信した値型オブジェクトを含む非同期操作です。</returns>
		public ValueTask<ValueType> ReceiveValue()
		{
			return new ValueTask<ValueType>(Task.Run(() => {
				lock (_vals) {
					return _vals.Dequeue();
				}
			}));
		}
	}
}
