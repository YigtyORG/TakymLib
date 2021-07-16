/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

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
		[TestMethod()]
		public void Run()
		{
#if BENCHMARK
			BenchmarkSwitcher.FromTypes(new[] { this.GetType() }).RunAllJoined();
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
	}
}
