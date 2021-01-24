
/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Threading.Tasks;

namespace TakymLib
{
	public delegate ValueTask AsyncEventHandler(object? sender, EventArgs e);

	public delegate ValueTask AsyncEventHandler<TEventArgs>(object? sender, TEventArgs e) where TEventArgs: EventArgs;

	public delegate ValueTask AsyncEventHandler<TSender, TEventArgs>(TSender? sender, TEventArgs e) where TEventArgs: EventArgs;

	public static class AsyncEventHandlerExtensions
	{
		public static async ValueTask Dispatch(this AsyncEventHandler handler, object? sender, EventArgs e)
		{
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				await tasks[i];
			}
		}

		public static async ValueTask Dispatch<TEventArgs>(this AsyncEventHandler<TEventArgs> handler, object? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler<TEventArgs>)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				await tasks[i];
			}
		}

		public static async ValueTask Dispatch<TSender, TEventArgs>(this AsyncEventHandler<TSender, TEventArgs> handler, TSender? sender, TEventArgs e)
			where TEventArgs: EventArgs
		{
			var handlers = handler.GetInvocationList();
			var tasks    = new ValueTask[handlers.Length];
			for (int i = 0; i < handlers.Length; ++i) {
				tasks[i] = ((AsyncEventHandler<TSender, TEventArgs>)(handlers[i]))(sender, e);
			}
			for (int i = 0; i < tasks.Length; ++i) {
				await tasks[i];
			}
		}
	}
}
