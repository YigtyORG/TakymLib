/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Properties;

namespace TakymLib
{
	/// <summary>
	///  破棄可能なオブジェクトの基底クラスです。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class DisposableBase : IDisposable, IAsyncDisposable
	{
		private volatile uint _run_locks;
		private volatile int  _state;
		private const    int  _state_disposing = 0b01;
		private const    int  _state_disposed  = 0b10;

		/// <summary>
		///  このオブジェクトの破棄処理を実行している場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposing => (_state & _state_disposing) == _state_disposing;

		/// <summary>
		///  このオブジェクトが破棄されている場合は<see langword="true"/>、有効な場合は<see langword="false"/>を返します。
		/// </summary>
		public bool IsDisposed => (_state & _state_disposed) == _state_disposed;

		/// <summary>
		///  上書きされた場合、破棄可能なオブジェクトを取得します。
		/// </summary>
		protected virtual IList<object?>? Disposables => null;

		protected virtual bool CanClearDisposed => true;

		/// <summary>
		///  型'<see cref="TakymLib.DisposableBase"/>'の新しいインスタンスを生成します。
		/// </summary>
		protected DisposableBase()
		{
			_run_locks = 0;
			_state     = 0;
		}

		/// <summary>
		///  型'<see cref="TakymLib.DisposableBase"/>'の現在のインスタンスを破棄します。
		/// </summary>
		~DisposableBase()
		{
			if (this.EnterDisposeLock()) {
				try {
					this.Dispose(false);
				} finally {
					this.LeaveDisposeLock();
				}
			}
		}

		/// <summary>
		///  現在のオブジェクトを破棄します。
		/// </summary>
		public void Dispose()
		{
			if (this.EnterDisposeLock()) {
				try {
					this.Dispose(true);
					GC.SuppressFinalize(this);
				} finally {
					this.LeaveDisposeLock();
				}
			}
		}

#pragma warning disable CA1816 // Dispose メソッドは、SuppressFinalize を呼び出す必要があります
		/// <summary>
		///  現在のオブジェクトを非同期的に破棄します。
		/// </summary>
		/// <returns>この処理の非同期操作です。</returns>
		public async ValueTask DisposeAsync()
		{
			if (await this.EnterDisposeLockAsync()) {
				try {
					await this.DisposeAsyncCore();
					this.Dispose(false);
					GC.SuppressFinalize(this);
				} finally {
					await this.LeaveDisposeLockAsync();
				}
			}
		}
#pragma warning restore CA1816 // Dispose メソッドは、SuppressFinalize を呼び出す必要があります

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを破棄します。
		/// </summary>
		/// <remarks>
		///  この関数内では例外を発生させてはいけません。
		/// </remarks>
		/// <param name="disposing">
		///  マネージドリソースとアンマネージリソース両方を破棄する場合は<see langword="true"/>、
		///  アンマネージリソースのみを破棄する場合は<see langword="false"/>を設定します。
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.IsDisposed) {
				return;
			}
			if (this.Disposables is not null and var disposables) {
				if (disposing) {
					int count = disposables.Count;
					for (int i = 0; i < count; ++i) {
						switch (disposables[i]) {
						case IAsyncDisposable o:
							o.ConfigureAwait(false).DisposeAsync().GetAwaiter().GetResult();
							break;
						case IDisposable o:
							o.Dispose();
							break;
						}
					}
				}
				disposables.Clear();
			}
			this.SetDisposed();
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているマネージドリソースを非同期的に破棄します。
		/// </summary>
		/// <remarks>
		///  この関数内では例外を発生させてはいけません。
		/// </remarks>
		/// <returns>この処理の非同期操作です。</returns>
		protected virtual async ValueTask DisposeAsyncCore()
		{
			if (this.IsDisposed) {
				return;
			}
			if (this.Disposables is not null and var disposables) {
				int count = disposables.Count;
				for (int i = 0; i < count; ++i) {
					switch (disposables[i]) {
					case IAsyncDisposable o:
						await o.ConfigureAwait(false).DisposeAsync();
						break;
					case IDisposable o:
						o.Dispose();
						break;
					}
				}
			}
		}

		/// <summary>
		///  現在のインスタンスが破棄されている場合に例外を発生させます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		protected virtual void EnsureNotDisposed()
		{
			this.ThrowIfDisposedCore();
		}

		/// <summary>
		///  <see cref="TakymLib.DisposableBase.EnsureNotDisposed"/>を呼び出します。
		/// </summary>
		/// <remarks>
		///  デバッグログまたはスタックトレースへログ出力を行う場合に利用します。
		/// </remarks>
		/// <exception cref="System.ObjectDisposedException"/>
		protected void ThrowIfDisposed()
		{
			this.LogThrowIfDisposed();
			this.ThrowIfDisposedCore();
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		private void ThrowIfDisposedCore()
		{
			if (this.IsDisposing) {
				throw new ObjectDisposedException(this.GetType().Name, Resources.DisposableBase_ObjectDisposedException_IsDisposing);
			}
			if (this.IsDisposed) {
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		[DebuggerHidden()]
		[StackTraceHidden()]
		[Conditional("DEBUG")]
		private void LogThrowIfDisposed()
		{
			Debug.WriteLineIf( this.IsDisposing, $"{this.GetType().Name}.{nameof(this.IsDisposing)} == {true}");
			Debug.WriteLineIf( this.IsDisposed,  $"{this.GetType().Name}.{nameof(this.IsDisposed)}  == {true}");
			Debug.Assert     (!this.IsDisposing, $"{this.GetType().Name} is disposing.");
			Debug.Assert     (!this.IsDisposed,  $"{this.GetType().Name} is disposed.");
		}

		/// <summary>
		///  処理の実行を開始した事を通知し、排他制御を登録します。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException" />
		protected void EnterRunLock()
		{
			Interlocked.Increment(ref _run_locks);

			try {
				this.EnsureNotDisposed();
			} catch (ObjectDisposedException) {
				this.LeaveRunLock();
				throw;
			}
		}

		/// <summary>
		///  処理の実行を終了した事を通知し、排他制御を解除します。
		/// </summary>
		/// <exception cref="System.InvalidOperationException"/>
		protected void LeaveRunLock()
		{
			uint locks;
			while (true) {
				locks = _run_locks;
				if (locks == 0) {
					throw new InvalidOperationException(Resources.DisposableBase_LeaveRunLock_InvalidOperationException);
				}
				if (Interlocked.CompareExchange(ref _run_locks, locks - 1, locks) == locks) {
					return;
				}
				Thread.Yield();
			}
		}

		/// <summary>
		///  処理の実行を終了した事を通知し、排他制御を非同期的に解除します。
		/// </summary>
		/// <returns>
		///  この処理の非同期操作です。
		/// </returns>
		/// <exception cref="System.InvalidOperationException"/>
		protected async ValueTask LeaveRunLockAsync()
		{
			uint locks;
			while (true) {
				locks = _run_locks;
				if (locks == 0) {
					throw new InvalidOperationException(Resources.DisposableBase_LeaveRunLock_InvalidOperationException);
				}
				if (Interlocked.CompareExchange(ref _run_locks, locks - 1, locks) == locks) {
					return;
				}
				await Task.Yield();
			}
		}

		/// <summary>
		///  指定された処理を安全に実行します。
		/// </summary>
		/// <param name="action">実行する処理を指定します。</param>
		/// <returns><paramref name="action"/>の戻り値を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void RunSafely(Action action)
		{
			this.EnterRunLock();
			try {
				action();
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  指定された処理を安全に実行します。
		/// </summary>
		/// <typeparam name="TState">引数の型を指定します。</typeparam>
		/// <param name="action">実行する処理を指定します。</param>
		/// <param name="state">処理に渡す引数を指定します。</param>
		/// <returns><paramref name="action"/>の戻り値を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void RunSafely<TState>(Action<TState> action, TState state)
		{
			this.EnterRunLock();
			try {
				action(state);
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  指定された処理を安全に実行します。
		/// </summary>
		/// <typeparam name="TState">引数の型を指定します。</typeparam>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="action">実行する処理を指定します。</param>
		/// <param name="state">処理に渡す引数を指定します。</param>
		/// <returns><paramref name="action"/>の戻り値を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected TResult RunSafely<TState, TResult>(Func<TState, TResult> action, TState state)
		{
			this.EnterRunLock();
			try {
				return action(state);
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  指定された処理を安全に実行します。
		/// </summary>
		/// <typeparam name="TState">引数の型を指定します。</typeparam>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="action">実行する処理を指定します。</param>
		/// <param name="state">処理に渡す引数を指定します。</param>
		/// <returns><paramref name="action"/>の戻り値を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected TResult RunSafely<TState, TResult>(Func<DisposableBase, TState, TResult> action, TState state)
		{
			this.EnterRunLock();
			try {
				return action(this, state);
			} finally {
				this.LeaveRunLock();
			}
		}

		/// <summary>
		///  指定された処理を安全かつ非同期的に実行します。
		/// </summary>
		/// <typeparam name="TState">引数の型を指定します。</typeparam>
		/// <param name="action">実行する処理を指定します。</param>
		/// <param name="state">処理に渡す引数を指定します。</param>
		/// <returns>この処理の非同期操作を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected async ValueTask RunSafelyAsync<TState>(Func<TState, ValueTask> action, TState state)
		{
			this.EnterRunLock();
			try {
				await action(state);
			} finally {
				await this.LeaveRunLockAsync();
			}
		}

		/// <summary>
		///  指定された処理を安全かつ非同期的に実行します。
		/// </summary>
		/// <typeparam name="TState">引数の型を指定します。</typeparam>
		/// <typeparam name="TResult">戻り値の型を指定します。</typeparam>
		/// <param name="action">実行する処理を指定します。</param>
		/// <param name="state">処理に渡す引数を指定します。</param>
		/// <returns><paramref name="action"/>の戻り値を含む非同期操作を返します。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.InvalidOperationException"/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected async ValueTask<TResult> RunSafelyAsync<TState, TResult>(Func<DisposableBase, TState, ValueTask<TResult>> action, TState state)
		{
			this.EnterRunLock();
			try {
				return await action(this, state);
			} finally {
				await this.LeaveRunLockAsync();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool EnterDisposeLock()
		{
			int stateValue;
			while (true) {
				stateValue = _state;
				if ((stateValue & _state_disposing) == _state_disposing) {
					return false;
				}
				if (Interlocked.CompareExchange(ref _state, stateValue | _state_disposing, stateValue) == stateValue) {
					while (_run_locks != 0) {
						Thread.Yield();
					}
					return true;
				}
				Thread.Yield();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private async ValueTask<bool> EnterDisposeLockAsync()
		{
			int stateValue;
			while (true) {
				stateValue = _state;
				if ((stateValue & _state_disposing) == _state_disposing) {
					return false;
				}
				if (Interlocked.CompareExchange(ref _state, stateValue | _state_disposing, stateValue) == stateValue) {
					while (_run_locks != 0) {
						await Task.Yield();
					}
					return true;
				}
				await Task.Yield();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LeaveDisposeLock()
		{
			int stateValue;
			while (true) {
				stateValue = _state;
				if (Interlocked.CompareExchange(ref _state, stateValue & ~_state_disposing, stateValue) == stateValue) {
					return;
				}
				Thread.Yield();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private async ValueTask LeaveDisposeLockAsync()
		{
			int stateValue;
			while (true) {
				stateValue = _state;
				if (Interlocked.CompareExchange(ref _state, stateValue & ~_state_disposing, stateValue) == stateValue) {
					return;
				}
				await Task.Yield();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetDisposed()
		{
			int stateValue;
			while (true) {
				stateValue = _state;
				if (Interlocked.CompareExchange(ref _state, stateValue | _state_disposed, stateValue) == stateValue) {
					return;
				}
				Thread.Yield();
			}
		}

		protected bool TryClearDisposed()
		{
			if (this.CanClearDisposed) {
				int stateValue;
				while (true) {
					stateValue = _state;
					if ((stateValue & _state_disposing) == _state_disposing) {
						return false;
					}
					if (Interlocked.CompareExchange(ref _state, stateValue & ~_state_disposed, stateValue) == stateValue) {
						return true;
					}
					Thread.Yield();
				}
			}
			return false;
		}

		protected async ValueTask<bool> TryClearDisposedAsync()
		{
			if (this.CanClearDisposed) {
				int stateValue;
				while (true) {
					stateValue = _state;
					if ((stateValue & _state_disposing) == _state_disposing) {
						return false;
					}
					if (Interlocked.CompareExchange(ref _state, stateValue & ~_state_disposed, stateValue) == stateValue) {
						return true;
					}
					await Task.Yield();
				}
			}
			return false;
		}
	}
}
