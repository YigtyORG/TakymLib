/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Security.Properties;

namespace TakymLib.Security
{
	/// <summary>
	///  メモリの検証を定期的に行います。
	///  このクラスは継承できません。
	/// </summary>
	public sealed class MemoryValidation : DisposableBase
	{
		/// <summary>
		///  メモリ検証スレッドを新たに開始します。
		/// </summary>
		/// <returns>メモリ検証スレッドを監視するオブジェクトです。</returns>
		public static MemoryValidation Start()
		{
			var mv = new MemoryValidation(1000);
			mv.StartCore();
			return mv;
		}

		private readonly Thread                        _thread;
		private readonly CancellationTokenSource       _cts;
		private readonly CancellationTokenSource       _cts_main;
		private readonly CancellationTokenRegistration _ctr_main;
		private readonly int                           _interval;
		private readonly RandomNumberGenerator         _rng;
		private          long                          _value;
		private          long                          _mask;
		private          long                          _masked_and;
		private          long                          _masked_ior;
		private          long                          _masked_xor;

		private MemoryValidation(int interval)
		{
			_thread              = new Thread(this.BackgroundThread);
			_thread.IsBackground = true;
			_cts                 = new CancellationTokenSource();
			_cts_main            = new CancellationTokenSource();
			_ctr_main            = _cts_main.Token.Register(this.ThrowSecurityException, true);
			_interval            = interval;
			_rng                 = RandomNumberGenerator.Create();
		}

		private void StartCore()
		{
			_thread.Start();
		}

		private void BackgroundThread()
		{
			var token = _cts.Token;
			try {
				Span<byte> buf = stackalloc byte[8];
				while (!token.IsCancellationRequested) {
					lock (this) {
						_rng.GetNonZeroBytes(buf);
						_value = BitConverter.ToInt64(buf);
						_rng.GetNonZeroBytes(buf);
						_mask = BitConverter.ToInt64(buf);
						_masked_and = _value & _mask;
						_masked_ior = _value | _mask;
						_masked_xor = _value ^ _mask;
					}
					Thread.Sleep(_interval);
					lock (this) {
						if (((_value & _mask) != _masked_and) ||
							((_value | _mask) != _masked_ior) ||
							((_value ^ _mask) != _masked_xor)) {
							_cts_main.Cancel();
						}
					}
				}
			} catch (OperationCanceledException oce) when (oce.CancellationToken == token) {
				return;
			}
		}

		private void ThrowSecurityException()
		{
			throw new SecurityException(Resources.MemoryValidation_SecurityException, this.GetType());
		}

		/// <summary>
		///  直ちにメモリ検証スレッドを停止させます。
		/// </summary>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.AggregateException"/>
		public void StopImmediately()
		{
			this.EnsureNotDisposed();
			_cts.Cancel(true);
		}

		/// <summary>
		///  メモリ検証スレッドに終了要求を発行し、終了するまで待機します。
		/// </summary>
		/// <returns>この処理の非同期操作です。</returns>
		/// <exception cref="System.ObjectDisposedException"/>
		/// <exception cref="System.AggregateException"/>
		public ConfiguredTaskAwaitable StopAsync()
		{
			_cts.Cancel();
			return Task.Run(async () => {
				while (_thread.ThreadState.HasFlag(ThreadState.Running)) {
					await Task.Yield();
				}
			}).ConfigureAwait(false);
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
				if (disposing) {
					_cts     .Dispose();
					_ctr_main.Dispose();
					_cts_main.Dispose();
					_rng     .Dispose();
					_thread.ExecutionContext?.Dispose();
				}
				base.Dispose(disposing);
			}
		}

		/// <summary>
		///  現在のオブジェクトインスタンスと利用しているリソースを非同期で破棄します。
		///  この関数内で例外を発生させてはいけません。
		/// </summary>
		/// <remarks>この処理の非同期操作です。</remarks>
		protected override async ValueTask DisposeAsyncCore()
		{
			if (_cts is IAsyncDisposable asyncDisposable0) {
				await asyncDisposable0.ConfigureAwait(false).DisposeAsync();
			} else {
				_cts.Dispose();
			}
			await _ctr_main.ConfigureAwait(false).DisposeAsync();
			if (_cts_main is IAsyncDisposable asyncDisposable1) {
				await asyncDisposable1.ConfigureAwait(false).DisposeAsync();
			} else {
				_cts_main.Dispose();
			}
			if (_rng is IAsyncDisposable asyncDisposable2) {
				await asyncDisposable2.ConfigureAwait(false).DisposeAsync();
			} else {
				_rng.Dispose();
			}
			_thread.ExecutionContext?.Dispose();
			await base.DisposeAsyncCore();
		}
	}
}
