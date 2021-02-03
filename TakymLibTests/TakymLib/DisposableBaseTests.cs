/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

namespace TakymLibTests.TakymLib
{
	[TestClass()]
	public class DisposableBaseTests
	{
		[TestMethod()]
		public void DisposeTest()
		{
			var obj = new DisposableBaseMock();
			using (obj) { }
			Assert.IsNotNull(obj);
			Assert.IsFalse(obj.IsDisposing);
			Assert.IsTrue (obj.IsDisposed);
		}

		[TestMethod()]
		public void DisposeAsyncTest()
		{
			var obj = new DisposableBaseMock();
			DisposeAsyncTestCore(obj);
			Assert.IsNotNull(obj);
			Assert.IsFalse(obj.IsDisposing);
			Assert.IsTrue (obj.IsDisposed);
		}

		private static async void DisposeAsyncTestCore(DisposableBase disp)
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
