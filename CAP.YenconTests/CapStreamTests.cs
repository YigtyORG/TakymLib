/****
 * CAP - "Configuration and Property"
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAP.Tests
{
	[TestClass()]
	public class CapStreamTests
	{
		[TestMethod()]
		public void DisposeTest()
		{
			var obj = new DummyCapStream();
			using (obj) { }
			Assert.IsNotNull(obj);
			Assert.IsTrue(obj.IsDisposing);
			Assert.IsTrue(obj.IsDisposed);
		}

		[TestMethod()]
		public async void DisposeAsyncTest()
		{
			var obj = new DummyCapStream();
			await using (obj.ConfigureAwait(false)) { }
			Assert.IsNotNull(obj);
			Assert.IsTrue(obj.IsDisposing);
			Assert.IsTrue(obj.IsDisposed);
		}

		private sealed class DummyCapStream : CapStream
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
