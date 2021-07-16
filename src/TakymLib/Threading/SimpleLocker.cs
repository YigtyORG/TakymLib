/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading;
using System.Threading.Tasks;

namespace TakymLib.Threading
{
	/// <summary>
	///  単純な排他制御を行います。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class SimpleLocker
	{
		private const uint SHARED       = 0;
		private const uint LOCKED       = 1;
		private const uint LOCKED_ASYNC = 2;
		private       uint _state;
		private       int  _thread_id;

		/// <summary>
		///  型'<see cref="TakymLib.Threading.SimpleLocker"/>'の新しいインスタンスを生成します。
		/// </summary>
		public SimpleLocker()
		{
			_state = SHARED;
		}

		/// <summary>
		///  ロックします。
		///  既にロックされている場合は解除されるまで待機します。
		/// </summary>
		/// <param name="lockTaken">ロック完了後に<see langword="true"/>に設定する必要のある変数を指定します。</param>
		public void EnterLock(ref bool lockTaken)
		{
			while (Interlocked.CompareExchange(ref _state, LOCKED, SHARED) != SHARED) {
				Thread.Yield();
			}
			_thread_id = Thread.CurrentThread.ManagedThreadId;
			lockTaken  = true;
		}

		/// <summary>
		///  非同期的にロックします。
		///  既にロックされている場合は解除されるまで待機します。
		/// </summary>
		/// <returns>この処理の非同期操作を返します。</returns>
		public async ValueTask EnterLockAsync()
		{
			while (Interlocked.CompareExchange(ref _state, LOCKED_ASYNC, SHARED) != SHARED) {
				await Task.Yield();
			}
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.SimpleLocker.EnterLock(ref bool)"/>により取得したロックを解除します。
		/// </summary>
		/// <remarks>
		///  <see cref="TakymLib.Threading.SimpleLocker.EnterLock(ref bool)"/>を呼び出したスレッドと同じスレッドからのみ呼び出す事ができます。
		/// </remarks>
		/// <returns>成功した場合は<see langword="true"/>、失敗した場合は<see langword="false"/>を返します。</returns>
		public bool LeaveLock()
		{
			if (_thread_id != Thread.CurrentThread.ManagedThreadId) {
				return false;
			}
			while (true) {
				uint oldStatus = _state;
				if (oldStatus == LOCKED_ASYNC) {
					return false;
				}
				if (oldStatus == SHARED ||
					Interlocked.CompareExchange(ref _state, SHARED, oldStatus) == oldStatus) {
					return true;
				}
				Thread.Yield();
			}
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.SimpleLocker.LeaveLockAsync"/>により取得したロックを解除します。
		/// </summary>
		/// <returns>成功したかどうかを示す論理値を含むこの処理の非同期操作を返します。</returns>
		public async ValueTask<bool> LeaveLockAsync()
		{
			while (true) {
				uint oldStatus = _state;
				if (oldStatus == LOCKED) {
					return false;
				}
				if (oldStatus == SHARED ||
					Interlocked.CompareExchange(ref _state, SHARED, oldStatus) == oldStatus) {
					return true;
				}
				await Task.Yield();
			}
		}

		/// <summary>
		///  現在の状態を取得します。
		/// </summary>
		/// <returns><see cref="TakymLib.Threading.SimpleLocker.State"/>列挙値を返します。</returns>
		public State GetState()
		{
			if (_state == SHARED) {
				return State.Shared;
			}
			return State.Locked;
		}

		/// <summary>
		///  <see cref="TakymLib.Threading.SimpleLocker.State"/>の状態を表します。
		/// </summary>
		public enum State
		{
			/// <summary>
			///  共有中である事を表します。
			/// </summary>
			Shared,

			/// <summary>
			///  排他制御中である事を表します。
			/// </summary>
			Locked,
		}
	}
}
