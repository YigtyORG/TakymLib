/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.IO;
using TakymLibTests.Properties;

namespace TakymLibTests.TakymLib.IO
{
	[TestClass()]
	public class PathStringTests
	{
		internal const string Path = "C:\\aaa\\bbb\\ccc\\123.xyz";

		[TestMethod()]
		public void ToStringTest()
		{
			if (DoTest()) {
				string formatted1 = PathStringPool.Get(Path).ToString(PathStringFormatterTests.Format);
				Assert.AreEqual(Resources.PathStringToStringResult, formatted1);
				string formatted2 = PathStringPool.Get(Path).ToString("");
				Assert.AreEqual(Path, formatted2);
			} else {
				Console.WriteLine(nameof(ToStringTest) + " is only supported in .NET 5.0 on Windows.");
			}
		}

		internal static bool DoTest()
		{
#if NET5_0
			return OperatingSystem.IsWindows();
#else
			return false;
#endif
		}
	}
}
