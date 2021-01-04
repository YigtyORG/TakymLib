/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

namespace TakymLibTests.TakymLib
{
	[TestClass()]
	public class StringExtensionsTests
	{
		[TestMethod()]
		public void ToBooleanTest()
		{
			Assert.IsTrue ("true"    .ToBoolean());
			Assert.IsTrue ("yes"     .ToBoolean());
			Assert.IsTrue ("ot"      .ToBoolean());
			Assert.IsTrue ("on"      .ToBoolean());
			Assert.IsTrue ("enable"  .ToBoolean());
			Assert.IsTrue ("enabled" .ToBoolean());
			Assert.IsTrue ("allow"   .ToBoolean());
			Assert.IsTrue ("high"    .ToBoolean());
			Assert.IsTrue ("pos"     .ToBoolean());
			Assert.IsTrue ("positive".ToBoolean());
			Assert.IsTrue ("one"     .ToBoolean());
			Assert.IsTrue ("1"       .ToBoolean());
			Assert.IsTrue ("t"       .ToBoolean());
			Assert.IsTrue ("y"       .ToBoolean());
			Assert.IsTrue ("+"       .ToBoolean());
			Assert.IsFalse("false"   .ToBoolean());
			Assert.IsFalse("no"      .ToBoolean());
			Assert.IsFalse("not"     .ToBoolean());
			Assert.IsFalse("off"     .ToBoolean());
			Assert.IsFalse("disable" .ToBoolean());
			Assert.IsFalse("disabled".ToBoolean());
			Assert.IsFalse("deny"    .ToBoolean());
			Assert.IsFalse("low"     .ToBoolean());
			Assert.IsFalse("neg"     .ToBoolean());
			Assert.IsFalse("negative".ToBoolean());
			Assert.IsFalse("zero"    .ToBoolean());
			Assert.IsFalse("0"       .ToBoolean());
			Assert.IsFalse("f"       .ToBoolean());
			Assert.IsFalse("n"       .ToBoolean());
			Assert.IsFalse("-"       .ToBoolean());
			Assert.ThrowsException<FormatException>(() => "abc".ToBoolean());
		}

		[TestMethod()]
		public void AbridgeTest()
		{
			Assert.AreEqual("abcd",     "abcd"   .Abridge(4));
			Assert.AreEqual("abcd    ", "abcd"   .Abridge(8));
			Assert.AreEqual("xyz",      "xyz"    .Abridge(3));
			Assert.AreEqual("xyz     ", "xyz"    .Abridge(8));
			Assert.AreEqual("123...",   "1234567".Abridge(6));
		}

		[Obsolete()]
		[TestMethod()]
		public void FitToLineTest()
		{
			Assert.AreEqual("abcxyz123",        "abcxyz123".FitToLine());
			Assert.AreEqual("[CR][LF][TB][SP]", "\r\n\t　" .FitToLine());
		}
	}
}
