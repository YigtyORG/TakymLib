/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using TakymLib.Threading.Tasks.Wrappers;

namespace TakymLib.Threading.Tasks
{
	/// <summary>
	///  型'<see cref="System.Threading.Tasks.Task"/>'と型'<see cref="System.Threading.Tasks.ValueTask"/>'の機能を拡張します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class TaskExtensions
	{
		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトとして利用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトへ変換可能な構造体です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TaskWrapper WrapAwaitable(this Task task)
		{
			task.EnsureNotNull(nameof(task));
			return new(task);
		}

		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトとして利用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトへ変換可能な構造体です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static TaskWrapper<TResult> WrapAwaitable<TResult>(this Task<TResult> task)
		{
			task.EnsureNotNull(nameof(task));
			return new(task);
		}

		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトとして利用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter"/>オブジェクトへ変換可能な構造体です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static ValueTaskWrapper WrapAwaitable(this ValueTask task)
		{
			task.EnsureNotNull(nameof(task));
			return new(task);
		}

		/// <summary>
		///  指定されたタスクを<see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトとして利用できる様にラップします。
		/// </summary>
		/// <param name="task">ラップするタスクです。</param>
		/// <returns><see cref="TakymLib.Threading.Tasks.IAwaiter{TResult}"/>オブジェクトへ変換可能な構造体です。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static ValueTaskWrapper<TResult> WrapAwaitable<TResult>(this ValueTask<TResult> task)
		{
			task.EnsureNotNull(nameof(task));
			return new(task);
		}
	}
}
