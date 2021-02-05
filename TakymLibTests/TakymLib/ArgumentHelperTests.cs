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
	public class ArgumentHelperTests
	{
		[TestMethod()]
		public void EnsureNotNullTest()
		{
			object? notnullobj = new object();
			notnullobj.EnsureNotNull(nameof(notnullobj));

			object? nullobj = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNull(nameof(nullobj)));

			string? notnullstr = string.Empty;
			notnullstr.EnsureNotNull(nameof(notnullstr));

			string? nullstr = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullstr.EnsureNotNull(nameof(nullstr)));
		}

#if false
		[TestMethod()]
		public void EnsureNotNullTest2()
		{
			try {
				Function();
			} catch (ArgumentNullException e) {
				Assert.AreEqual("nullObj", e.ParamName);
				return;
			}
			Assert.Fail();

			static void Function(object? nullObj = null)
			{
				nullObj.EnsureNotNull();
			}
		}
#endif

		[TestMethod()]
		public void EnsureWithinClosedRangeTest()
		{
			IComparable? nullobj = null;
			nullobj.EnsureWithinClosedRange(0, 1, nameof(nullobj));
			nullobj.EnsureWithinClosedRange(1, 0, nameof(nullobj));

			int? nullval = default;
			nullval.EnsureWithinClosedRange(0, 1, nameof(nullval));
			nullval.EnsureWithinClosedRange(1, 0, nameof(nullval));

			int a = 123;
			a.EnsureWithinClosedRange( 50, 150, nameof(a));
			a.EnsureWithinClosedRange(  0, 200, nameof(a));
			a.EnsureWithinClosedRange(  0, 123, nameof(a));
			a.EnsureWithinClosedRange(123, 200, nameof(a));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinClosedRange(  0,  50, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinClosedRange(200, 300, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinClosedRange(200,   0, nameof(a)));

			IComparable a2 = a;
			a2.EnsureWithinClosedRange( 50, 150, nameof(a2));
			a2.EnsureWithinClosedRange(  0, 200, nameof(a2));
			a2.EnsureWithinClosedRange(  0, 123, nameof(a2));
			a2.EnsureWithinClosedRange(123, 200, nameof(a2));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinClosedRange(  0,  50, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinClosedRange(200, 300, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinClosedRange(200,   0, nameof(a2)));
		}

		[TestMethod()]
		public void EnsureWithinOpenRangeTest()
		{
			IComparable? nullobj = null;
			nullobj.EnsureWithinOpenRange(0, 1, nameof(nullobj));
			nullobj.EnsureWithinOpenRange(1, 0, nameof(nullobj));

			int? nullval = default;
			nullval.EnsureWithinOpenRange(0, 1, nameof(nullval));
			nullval.EnsureWithinOpenRange(1, 0, nameof(nullval));

			int a = 123;
			a.EnsureWithinOpenRange(50, 150, nameof(a));
			a.EnsureWithinOpenRange( 0, 200, nameof(a));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(  0, 123, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(123, 200, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(  0,  50, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(200, 300, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureWithinOpenRange(200,   0, nameof(a)));

			IComparable a2 = a;
			a2.EnsureWithinOpenRange(50, 150, nameof(a2));
			a2.EnsureWithinOpenRange( 0, 200, nameof(a2));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(  0, 123, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(123, 200, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(  0,  50, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(200, 300, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureWithinOpenRange(200,   0, nameof(a2)));
		}

		[TestMethod()]
		public void EnsureNotNullWithinClosedRangeTest()
		{
			IComparable? nullobj = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinClosedRange(0, 1, nameof(nullobj)));
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinClosedRange(1, 0, nameof(nullobj)));

			int? nullval = default;
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinClosedRange(0, 1, nameof(nullval)));
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinClosedRange(1, 0, nameof(nullval)));

			int a = 123;
			a.EnsureNotNullWithinClosedRange( 50, 150, nameof(a));
			a.EnsureNotNullWithinClosedRange(  0, 200, nameof(a));
			a.EnsureNotNullWithinClosedRange(  0, 123, nameof(a));
			a.EnsureNotNullWithinClosedRange(123, 200, nameof(a));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinClosedRange(  0,  50, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinClosedRange(200, 300, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinClosedRange(200,   0, nameof(a)));

			IComparable a2 = a;
			a2.EnsureNotNullWithinClosedRange( 50, 150, nameof(a2));
			a2.EnsureNotNullWithinClosedRange(  0, 200, nameof(a2));
			a2.EnsureNotNullWithinClosedRange(  0, 123, nameof(a2));
			a2.EnsureNotNullWithinClosedRange(123, 200, nameof(a2));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinClosedRange(  0,  50, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinClosedRange(200, 300, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinClosedRange(200,   0, nameof(a2)));
		}

		[TestMethod()]
		public void EnsureNotNullWithinOpenRangeTest()
		{
			IComparable? nullobj = null;
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinOpenRange(0, 1, nameof(nullobj)));
			Assert.ThrowsException<ArgumentNullException>(() => nullobj.EnsureNotNullWithinOpenRange(1, 0, nameof(nullobj)));

			int? nullval = default;
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinOpenRange(0, 1, nameof(nullval)));
			Assert.ThrowsException<ArgumentNullException>(() => nullval.EnsureNotNullWithinOpenRange(1, 0, nameof(nullval)));

			int a = 123;
			a.EnsureNotNullWithinOpenRange(50, 150, nameof(a));
			a.EnsureNotNullWithinOpenRange( 0, 200, nameof(a));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(  0, 123, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(123, 200, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(  0,  50, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(200, 300, nameof(a)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.EnsureNotNullWithinOpenRange(200,   0, nameof(a)));

			IComparable a2 = a;
			a2.EnsureNotNullWithinOpenRange(50, 150, nameof(a2));
			a2.EnsureNotNullWithinOpenRange( 0, 200, nameof(a2));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(  0, 123, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(123, 200, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(  0,  50, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(200, 300, nameof(a2)));
			Assert.ThrowsException<ArgumentOutOfRangeException>(() => a2.EnsureNotNullWithinOpenRange(200,   0, nameof(a2)));
		}
	}
}
