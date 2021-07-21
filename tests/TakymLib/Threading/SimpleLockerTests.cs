/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Threading;

namespace TakymLibTests.TakymLib.Threading
{
	[TestClass()]
	public class SimpleLockerTests
	{
		[TestMethod()]
		public void Test()
		{
			var  locker = new SimpleLocker();
			bool taken  = false;

			Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
			locker.EnterLock(ref taken);
			Assert.AreEqual(SimpleLocker.State.Locked, locker.GetState());
			Assert.IsTrue(taken);
			Assert.IsTrue(locker.LeaveLock());
			Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
			Assert.IsTrue(locker.LeaveLock());
		}

		[TestMethod()]
		public void AsyncTest()
		{
			AsyncTestCore().ConfigureAwait(false).GetAwaiter().GetResult();

			static async Task AsyncTestCore()
			{
				var locker = new SimpleLocker();

				Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
				await locker.EnterLockAsync();
				Assert.AreEqual(SimpleLocker.State.Locked, locker.GetState());
				Assert.IsTrue(await locker.LeaveLockAsync());
				Assert.AreEqual(SimpleLocker.State.Shared, locker.GetState());
				Assert.IsTrue(await locker.LeaveLockAsync());
			}
		}
	}
}
