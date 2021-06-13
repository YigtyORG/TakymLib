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
	public class PathStringFormatterTests
	{
		internal const string Format = "B//D//F//N//O//P//R//U//X//\\A\\B\\C\\D";

		[TestMethod()]
		public void FormatTest()
		{
			if (OperatingSystem.IsWindows()) {
				var path      = PathStringPool.Get(PathStringTests.Path);
				var formatter = new PathStringFormatter();

				string formatted = formatter.Format(Format, path, null);
				Assert.AreEqual(Resources.PathStringToStringResult, formatted);
			} else {
				Console.WriteLine(nameof(FormatTest) + " is only supported on Windows.");
			}
		}
	}
}
