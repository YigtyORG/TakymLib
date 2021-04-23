/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Diagnostics;
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
			if (OperatingSystem.IsWindows()) {

				string formatted1 = PathStringPool.Get(Path).ToString(PathStringFormatterTests.Format);
				Assert.AreEqual(Resources.PathStringToStringResult, formatted1);

				string formatted2 = PathStringPool.Get(Path).ToString("");
				Assert.AreEqual(Path, formatted2);

			} else {
				Debug.WriteLine($"Skipped the {nameof(this.ToStringTest)} in {typeof(PathStringTests).AssemblyQualifiedName}");
			}
		}
	}
}
