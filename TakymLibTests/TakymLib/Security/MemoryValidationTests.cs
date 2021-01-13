/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Security;

namespace TakymLibTests.TakymLib.Security
{
	[TestClass()]
	public class MemoryValidationTests
	{
		[TestMethod()]
		public void StartTest()
		{
			using (var mv = MemoryValidation.Start()) { }
		}

		[TestMethod()]
		public void StopImmediatelyTest()
		{
			using (var mv = MemoryValidation.Start()) {
				mv.StopImmediately();
			}
		}

		[TestMethod()]
		public void StopAsyncTest()
		{
			using (var mv = MemoryValidation.Start()) {
				StopAsyncTestCore(mv).ConfigureAwait(true).GetAwaiter().GetResult();
			}
		}

		private static async Task StopAsyncTestCore(MemoryValidation mv)
		{
			await mv.StopAsync();
		}
	}
}
