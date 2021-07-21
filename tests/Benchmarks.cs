/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
//using BenchmarkDotNet.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;
using TakymLib.Threading;

#if BENCHMARK
using BenchmarkDotNet.Running;
#endif

namespace TakymLibTests
{
	[TestClass()]
	[MemoryDiagnoser()]
	[DisassemblyDiagnoser(5, true, true, true, true, true, true)]
	public class Benchmarks
	{
		private static readonly IConfig _config = DefaultConfig.Instance
			//.AddJob(new Job()
			//	.Freeze()
			//	.WithCustomBuildConfiguration("Benchmark")
			//)
			.WithOption(ConfigOptions.KeepBenchmarkFiles, true);

		[TestMethod()]
		public void Run()
		{
#if BENCHMARK
			BenchmarkSwitcher.FromTypes(new[] { this.GetType() }).RunAllJoined(_config);
#else
			Console.WriteLine(nameof(Benchmarks) + " are only supported in the benchmark build.");
#endif
		}

		[Benchmark()]
		public void EnsureNotNull1()
		{
			try {
				ArgumentHelper.EnsureNotNull(null, null);
			} catch (ArgumentNullException) { }
		}

		[Benchmark()]
		public void EnsureNotNull1_Expansion()
		{
			try {
				object? obj = null;
				if (obj is null) {
					throw new ArgumentNullException(null);
				}
			} catch (ArgumentNullException) { }
		}

		[Benchmark()]
		public void EnsureNotNull2()
		{
			ArgumentHelper.EnsureNotNull(new object(), nameof(Object));
			ArgumentHelper.EnsureNotNull(123, "number");
		}

		[Benchmark()]
		public void EnsureNotNull2_Expansion()
		{
			object? obj = new object();
			if (obj is null) {
				throw new ArgumentNullException(nameof(Object));
			}

			object? num = 123;
			if (num is null) {
				throw new ArgumentNullException("number");
			}
		}

		[Benchmark()]
		public void LockStatement()
		{
			int num = 0;
			object obj = new();
			for (int i = 0; i < 16; ++i) {
				lock (obj) {
					++num;
				}
			}
		}

		[Benchmark()]
		public void SimpleLocker()
		{
			int num = 0;
			var locker = new SimpleLocker();
			for (int i = 0; i < 16; ++i) {
				bool lockTaken = false;
				try {
					locker.EnterLock(ref lockTaken);
					++num;
				} finally {
					if (lockTaken) {
						locker.LeaveLock();
					}
				}
			}
		}
	}
}
