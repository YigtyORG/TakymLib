/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TakymLib.Threading.Distributed.Properties;

namespace TakymLib.Threading.Distributed.Internals
{
	internal sealed class DefaultExecutionContextManager : ExecutionContextManagerInternal
	{
		internal static readonly DefaultExecutionContextManager _inst = new();

		private DefaultExecutionContextManager() { }

		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				throw new InvalidOperationException(Resources.DefaultExecutionContextManager_Dispose_InvalidOperationException);
			}
		}

		protected override ValueTask DisposeAsyncCore()
		{
			throw new InvalidOperationException(Resources.DefaultExecutionContextManager_Dispose_InvalidOperationException);
		}

		[DebuggerHidden()]
		// [StackTraceHidden()]
		private static void ThrowInvalidOperationException()
		{

		}
	}
}
