/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.IO;
using TakymLibTests.Properties;

namespace TakymLibTests.IO
{
	[TestClass()]
	public class PathStringTests
	{
		internal const string Path = "C:\\aaa\\bbb\\ccc\\123.xyz";

		[TestMethod()]
		public void ToStringTest()
		{
			string formatted1 = new PathString(Path).ToString(PathStringFormatterTests.Format);
			Assert.AreEqual(Resources.PathStringToStringResult, formatted1);

			string formatted2 = new PathString(Path).ToString("");
			Assert.AreEqual(Path, formatted2);
		}
	}
}
