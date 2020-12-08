/****
 * TakymLib
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TakymLib.Tests
{
	[TestClass()]
	public class DisposableBaseTests
	{
		[TestMethod()]
		public virtual void DisposeTest()
		{
			var obj = new DisposableBaseMock();
			using (obj) { }
			Assert.IsNotNull(obj);
			Assert.IsTrue(obj.IsDisposing);
			Assert.IsTrue(obj.IsDisposed);
		}

		[TestMethod()]
		public virtual void DisposeAsyncTest()
		{
			var obj = new DisposableBaseMock();
			DisposeAsyncTestCore(obj);
			Assert.IsNotNull(obj);
			Assert.IsTrue(obj.IsDisposing);
			Assert.IsTrue(obj.IsDisposed);
		}

		protected static async void DisposeAsyncTestCore(DisposableBase disp)
		{
			await using (disp.ConfigureAwait(false)) { }
		}

		private sealed class DisposableBaseMock : DisposableBase
		{
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}

			protected override async ValueTask DisposeAsyncCore()
			{
				await base.DisposeAsyncCore();
			}
		}
	}
}
