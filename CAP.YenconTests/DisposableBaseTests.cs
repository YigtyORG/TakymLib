/****
 * CAP - "Configuration and Property"
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAP.Tests
{
	[TestClass()]
	public class DisposableBaseTests
	{
		[TestMethod()]
		public void DisposeTest()
		{
			var obj = new DisposableBaseMock();
			obj.Dispose();
			Assert.IsNotNull(obj);
			Assert.IsTrue(obj.IsDisposed);
		}

		[TestMethod()]
		public void DisposeAsyncTest()
		{
			var obj = new DisposableBaseMock();
			obj.DisposeAsync().GetAwaiter().GetResult();
			Assert.IsNotNull(obj);
			Assert.IsTrue(obj.IsDisposed);
		}

		private sealed class DisposableBaseMock : DisposableBase
		{
			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}
		}
	}
}
