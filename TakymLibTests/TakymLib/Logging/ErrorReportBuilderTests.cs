/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
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
			var dir = new PathString("Temp/Logs");
			Directory.CreateDirectory(dir);
			SaveER(dir, new Exception());
			SaveER(dir, new NullReferenceException());
			SaveER(dir, new DivideByZeroException());
			SaveER(dir, new InvalidOperationException());
			SaveER(dir, new InvalidDataException());
			SaveER(dir, new InvalidPathFormatException("xyz"));
			SaveER(dir, new AccessViolationException());
			SaveER(dir, new StackOverflowException());
			SaveER(dir, new ObjectDisposedException("a"));
			SaveER(dir, new ArgumentNullException("b"));
			SaveER(dir, new AggregateException(new Exception(), new Exception(), new Exception("c", new Exception("d", new Exception()))));
		}

		private static void SaveER(PathString dir, Exception ex)
		{
			ErrorReportBuilder.Create(ex).Save(dir, null);
		}
	}
}
