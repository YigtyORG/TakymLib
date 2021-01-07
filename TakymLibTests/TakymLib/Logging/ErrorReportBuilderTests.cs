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
using TakymLib.Logging;

namespace TakymLibTests.TakymLib.Logging
{
	[TestClass()]
	public class ErrorReportBuilderTests
	{
		[TestMethod()]
		public void CreateTest()
		{
			Assert.IsTrue(ErrorReportBuilder.Create(new Exception()) is ErrorReportBuilder);
		}

		[TestMethod()]
		public void SaveTest()
		{
			ErrorReportBuilder.Create(new Exception()).Save(new PathString(), null);
		}
	}
}
