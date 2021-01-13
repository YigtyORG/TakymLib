/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Security;

namespace TakymLibTests.TakymLib.Security
{
	[TestClass()]
	public class MemoryValidationTests
	{
		private MemoryValidation? _mv;

		[TestInitialize()]
		public void Initialize()
		{
			_mv = MemoryValidation.Start();
		}

		[TestMethod()]
		public void StartTest()
		{
			using (var mv = MemoryValidation.Start()) { }
		}

		[TestMethod()]
		public void StopImmediatelyTest()
		{
			_mv?.StopImmediately();
		}

		[TestMethod()]
		public void StopAsyncTest()
		{
			_mv?.StopAsync().GetAwaiter().GetResult();
		}

		[TestCleanup()]
		public void Cleanup()
		{
			_mv?.Dispose();
		}
	}
}
