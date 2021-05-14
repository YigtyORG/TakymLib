/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Aspect;
using TakymLib.Logging;

namespace TakymLibTests.TakymLib.Aspect
{
	[TestClass()]
	public class LoggableTaskTests
	{
		[TestMethod()]
		public void VoidTest()
		{
			TestCore(VoidTestCore, s => {
				Assert.IsTrue(s.Contains(nameof(VoidTestCore)));
				Assert.IsTrue(s.Contains("begin"));
				Assert.IsTrue(s.Contains("end"));
				Assert.IsTrue(s.Contains("LoggableTaskTests.cs"));
			}).ConfigureAwait(false).GetAwaiter().GetResult();

			static async LoggableTask VoidTestCore()
			{
				await Task.CompletedTask;
			}
		}

		[TestMethod()]
		public void IntTest()
		{
			TestCore(IntTestCore, s => {
				Assert.IsTrue(s.Contains(nameof(IntTestCore)));
				Assert.IsTrue(s.Contains("begin"));
				Assert.IsTrue(s.Contains("end"));
				Assert.IsTrue(s.Contains("LoggableTaskTests.cs"));
			}, 0, 1, 2, 3, 4, 5).ConfigureAwait(false).GetAwaiter().GetResult();

			static async LoggableTask<int> IntTestCore(int n)
			{
				await Task.CompletedTask;
				return n;
			}
		}

		[TestMethod()]
		public void StringTest()
		{
			TestCore(StringTestCore, s => {
				Assert.IsTrue(s.Contains(nameof(StringTestCore)));
				Assert.IsTrue(s.Contains("begin"));
				Assert.IsTrue(s.Contains("end"));
				Assert.IsTrue(s.Contains("LoggableTaskTests.cs"));
			}, "hello", "WORLD", "hoge", "TEST", "1234").ConfigureAwait(false).GetAwaiter().GetResult();

			static async LoggableTask<string> StringTestCore(string s)
			{
				await Task.CompletedTask;
				return s;
			}
		}

		private static async Task TestCore(Func<LoggableTask> tester, Action<string> validate)
		{
			using (var sw     = new StringWriter())
			using (var logger = new TextWriterCallerLogger(sw)) {
				LoggableTask.Logger = logger;
				await tester();
				validate(sw.ToString());
			}
		}

		private static async Task TestCore<T>(Func<T, LoggableTask<T>> tester, Action<string> validate, params T[] args)
		{
			using (var sw     = new StringWriter())
			using (var logger = new TextWriterCallerLogger(sw)) {
				LoggableTask<T>.Logger = logger;
				for (int i = 0; i < args.Length; ++i) {
					var arg = args[i];
					Assert.AreEqual(arg, await tester(arg));
				}
				validate(sw.ToString());
			}
		}
	}
}
