/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TakymLib.Logging;
using TakymLib.Threading.Tasks;

namespace TakymLib.Aspect
{
	/// <summary>
	///  非同期関数の処理を分断し、ログ出力可能にします。
	/// </summary>
	/// <typeparam name="TResult">戻り値の種類です。</typeparam>
	[StructLayout(LayoutKind.Auto)]
	public readonly struct LoggableTask<TResult> : IAwaitable<TResult>
	{
		private static ICallerLogger? _logger;

		/// <summary>
		///  ログの出力先を取得または設定します。
		/// </summary>
		/// <remarks>
		///  <see langword="null"/>が設定されている場合は<see cref="TakymLib.Aspect.LoggableTask.Logger"/>の値を使用します。
		/// </remarks>
		public static ICallerLogger? Logger
		{
			get => _logger ?? LoggableTask.Logger;
			set
			{
				var logger = _logger;
				while (Interlocked.CompareExchange(ref _logger, value, logger) != logger) {
					Thread.Yield();
					logger = _logger;
				}
			}
		}

		private readonly ValueTask<TResult> _task;
		private readonly bool               _continue_on_captured_context;

		/// <summary>
		///  この<see cref="TakymLib.Aspect.LoggableTask{TResult}"/>の内部で利用される<see cref="System.Threading.Tasks.ValueTask{TResult}"/>を取得します。
		/// </summary>
		public ValueTask<TResult> Task => _task;

		internal LoggableTask(ValueTask<TResult> task, bool continueOnCapturedContext = true)
		{
			_task                         = task;
			_continue_on_captured_context = continueOnCapturedContext;
		}

		/// <summary>
		///  この<see cref="TakymLib.Aspect.LoggableTask{TResult}"/>を構成します。
		/// </summary>
		/// <param name="continueOnCapturedContext">
		///  継続を捕獲された元の実行文脈で実行する場合は<see langword="true"/>、それ以外の場合は<see langword="false"/>を指定します。
		/// </param>
		/// <returns>構成された待機可能なオブジェクトです。</returns>
		public LoggableTask<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new(_task, continueOnCapturedContext);
		}

		/// <summary>
		///  <see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter"/>を取得します。
		/// </summary>
		/// <returns><see cref="System.Runtime.CompilerServices.ConfiguredTaskAwaitable{TResult}.ConfiguredTaskAwaiter"/>を返します。</returns>
		public ConfiguredValueTaskAwaitable<TResult>.ConfiguredValueTaskAwaiter GetAwaiter()
		{
			return _task.ConfigureAwait(_continue_on_captured_context).GetAwaiter();
		}

		IAwaiter<TResult> IAwaitable<TResult>.GetAwaiter()
		{
			return _task.WrapAwaitable().ConfigureAwait(false).GetAwaiter();
		}
	}
}
