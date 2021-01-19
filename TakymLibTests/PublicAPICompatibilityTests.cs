/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib;

namespace TakymLibTests
{
	[TestClass()]
	public class PublicAPICompatibilityTests
	{
		[TestMethod()]
		public void ArgumentHelperTest()
		{
			var min  = new EmptyComparable (-1);
			var max  = new EmptyComparable (+1);
			var mint = new EmptyComparableT(-1);
			var maxt = new EmptyComparableT(+1);

			ArgumentHelper.EnsureNotNull            (new(), string.Empty);
			ArgumentHelper.EnsureNotNull<EmptyClass>(new(), string.Empty);

			new object    ().EnsureNotNull            (string.Empty);
			new EmptyClass().EnsureNotNull<EmptyClass>(string.Empty);

			ArgumentHelper.EnsureWithinClosedRange                  (new EmptyComparable (), new(), new(), string.Empty);
			ArgumentHelper.EnsureWithinClosedRange<EmptyComparableT>(new EmptyComparableT(), new(), new(), string.Empty);

			new EmptyComparable ().EnsureWithinClosedRange                  (new(), new(), string.Empty);
			new EmptyComparableT().EnsureWithinClosedRange<EmptyComparableT>(new(), new(), string.Empty);

			ArgumentHelper.EnsureWithinOpenRange                  (new EmptyComparable (), min,  max,  string.Empty);
			ArgumentHelper.EnsureWithinOpenRange<EmptyComparableT>(new EmptyComparableT(), mint, maxt, string.Empty);

			new EmptyComparable ().EnsureWithinOpenRange                  (min,  max,  string.Empty);
			new EmptyComparableT().EnsureWithinOpenRange<EmptyComparableT>(mint, maxt, string.Empty);

			ArgumentHelper.EnsureNotNullWithinClosedRange                  (new EmptyComparable (), new(), new(), string.Empty);
			ArgumentHelper.EnsureNotNullWithinClosedRange<EmptyComparableT>(new EmptyComparableT(), new(), new(), string.Empty);

			new EmptyComparable ().EnsureNotNullWithinClosedRange                  (new(), new(), string.Empty);
			new EmptyComparableT().EnsureNotNullWithinClosedRange<EmptyComparableT>(new(), new(), string.Empty);

			ArgumentHelper.EnsureNotNullWithinOpenRange                  (new EmptyComparable (), min,  max,  string.Empty);
			ArgumentHelper.EnsureNotNullWithinOpenRange<EmptyComparableT>(new EmptyComparableT(), mint, maxt, string.Empty);

			new EmptyComparable ().EnsureNotNullWithinOpenRange                  (min,  max,  string.Empty);
			new EmptyComparableT().EnsureNotNullWithinOpenRange<EmptyComparableT>(mint, maxt, string.Empty);

			ArgumentHelper.ThrowIfNull(new(), string.Empty);

			new object().ThrowIfNull(string.Empty);
		}

		[TestMethod()]
		public void ArrayExtensionsTest()
		{
			EmptyClass?[] arrayOfT;
			arrayOfT = ArrayExtensions.Combine<EmptyClass>(Array.Empty<EmptyClass>());
			arrayOfT = ArrayExtensions.Combine<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = ArrayExtensions.Combine<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = ArrayExtensions.Combine<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass[]>());
			arrayOfT = Array.Empty<EmptyClass>().Combine<EmptyClass>();
			arrayOfT = Array.Empty<EmptyClass>().Combine<EmptyClass>(Array.Empty<EmptyClass>());
			arrayOfT = Array.Empty<EmptyClass>().Combine<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = Array.Empty<EmptyClass>().Combine<EmptyClass>(Array.Empty<EmptyClass[]>());

			object?[] arrayOfObject;
			arrayOfObject = ArrayExtensions.Combine(Array.Empty<object>());
			arrayOfObject = ArrayExtensions.Combine(Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = ArrayExtensions.Combine(Array.Empty<object>(), Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = ArrayExtensions.Combine(Array.Empty<object>(), Array.Empty<object[]>());
			arrayOfObject = Array.Empty<object>().Combine();
			arrayOfObject = Array.Empty<object>().Combine(Array.Empty<object>());
			arrayOfObject = Array.Empty<object>().Combine(Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = Array.Empty<object>().Combine(Array.Empty<object[]>());
		}

		[TestMethod()]
		public void ArrayExtensionsAsyncTest()
		{
			this.ArrayExtensionsAsyncTestCore().GetAwaiter().GetResult();
		}

		private async Task ArrayExtensionsAsyncTestCore()
		{
			EmptyClass?[] arrayOfT;
			arrayOfT = await ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>());
			arrayOfT = await ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = await ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = await ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass[]>());
			arrayOfT = await Array.Empty<EmptyClass>().CombineAsync<EmptyClass>();
			arrayOfT = await Array.Empty<EmptyClass>().CombineAsync<EmptyClass>(Array.Empty<EmptyClass>());
			arrayOfT = await Array.Empty<EmptyClass>().CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = await Array.Empty<EmptyClass>().CombineAsync<EmptyClass>(Array.Empty<EmptyClass[]>());

			ConfiguredTaskAwaitable<EmptyClass?[]> awaitableOfT;
			awaitableOfT = ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>());
			arrayOfT = await awaitableOfT;
			awaitableOfT = ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = await awaitableOfT;
			awaitableOfT = ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = await awaitableOfT;
			awaitableOfT = ArrayExtensions.CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass[]>());
			arrayOfT = await awaitableOfT;
			awaitableOfT = Array.Empty<EmptyClass>().CombineAsync<EmptyClass>();
			arrayOfT = await awaitableOfT;
			awaitableOfT = Array.Empty<EmptyClass>().CombineAsync<EmptyClass>(Array.Empty<EmptyClass>());
			arrayOfT = await awaitableOfT;
			awaitableOfT = Array.Empty<EmptyClass>().CombineAsync<EmptyClass>(Array.Empty<EmptyClass>(), Array.Empty<EmptyClass>());
			arrayOfT = await awaitableOfT;
			awaitableOfT = Array.Empty<EmptyClass>().CombineAsync<EmptyClass>(Array.Empty<EmptyClass[]>());
			arrayOfT = await awaitableOfT;

			object?[] arrayOfObject;
			arrayOfObject = await ArrayExtensions.CombineAsync(Array.Empty<object>());
			arrayOfObject = await ArrayExtensions.CombineAsync(Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = await ArrayExtensions.CombineAsync(Array.Empty<object>(), Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = await ArrayExtensions.CombineAsync(Array.Empty<object>(), Array.Empty<object[]>());
			arrayOfObject = await Array.Empty<object>().CombineAsync();
			arrayOfObject = await Array.Empty<object>().CombineAsync(Array.Empty<object>());
			arrayOfObject = await Array.Empty<object>().CombineAsync(Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = await Array.Empty<object>().CombineAsync(Array.Empty<object[]>());

			ConfiguredTaskAwaitable<object?[]> awaitableOfObject;
			awaitableOfObject = ArrayExtensions.CombineAsync(Array.Empty<object>());
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = ArrayExtensions.CombineAsync(Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = ArrayExtensions.CombineAsync(Array.Empty<object>(), Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = ArrayExtensions.CombineAsync(Array.Empty<object>(), Array.Empty<object[]>());
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = Array.Empty<object>().CombineAsync();
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = Array.Empty<object>().CombineAsync(Array.Empty<object>());
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = Array.Empty<object>().CombineAsync(Array.Empty<object>(), Array.Empty<object>());
			arrayOfObject = await awaitableOfObject;
			awaitableOfObject = Array.Empty<object>().CombineAsync(Array.Empty<object[]>());
			arrayOfObject = await awaitableOfObject;
		}

		[TestMethod()]
		public void ConsoleUtilTest()
		{
			ConsoleUtil.WriteHorizontalRule();
			ConsoleUtil.WriteHorizontalRule('=');
			ConsoleUtil.Print(VersionInfo.Library);
			VersionInfo.Library.Print();

			// 存在が確認できれば良い。
			Action pause = ConsoleUtil.Pause;
			Func<string, bool> readYesNo = ConsoleUtil.ReadYesNo;
			Func<SecureString> readPassword = () => ConsoleUtil.ReadPassword();
			Func<char, SecureString> readPassword2 = ConsoleUtil.ReadPassword;
		}

		[TestMethod()]
		public void ControlCharsReplaceModeTest()
		{
			ControlCharsReplaceMode mode;
			mode = ControlCharsReplaceMode.RemoveAll;
			mode = ControlCharsReplaceMode.ConvertToText;
			mode = ControlCharsReplaceMode.ConvertToIcon;
			mode = ControlCharsReplaceMode.ConvertToString;
			mode = ControlCharsReplaceMode.ConvertToSymbol;
			mode = ControlCharsReplaceMode.ConvertToMark;
			mode = ControlCharsReplaceMode.ConvertToPictogram;
			mode = ControlCharsReplaceMode.ConvertToPicture;
			mode = ControlCharsReplaceMode.ConvertToLogo;
			mode = ControlCharsReplaceMode.ConvertToEmoji;

			Assert.AreEqual(ControlCharsReplaceMode.ConvertToText, ControlCharsReplaceMode.ConvertToString);
			Assert.AreEqual(ControlCharsReplaceMode.ConvertToText, ControlCharsReplaceMode.ConvertToSymbol);
			Assert.AreEqual(ControlCharsReplaceMode.ConvertToText, ControlCharsReplaceMode.ConvertToMark);
			Assert.AreEqual(ControlCharsReplaceMode.ConvertToIcon, ControlCharsReplaceMode.ConvertToPictogram);
			Assert.AreEqual(ControlCharsReplaceMode.ConvertToIcon, ControlCharsReplaceMode.ConvertToPicture);
			Assert.AreEqual(ControlCharsReplaceMode.ConvertToIcon, ControlCharsReplaceMode.ConvertToLogo);
			Assert.AreEqual(ControlCharsReplaceMode.ConvertToIcon, ControlCharsReplaceMode.ConvertToEmoji);
		}

		[TestMethod()]
		public void DisposableBaseTest()
		{
			bool boolVal;
			ValueTask valueTask;
			var disp = new EmptyDisposableBase();
			disp.EnsureNotDisposedTest();
			disp.ThrowIfDisposedTest();
			boolVal = disp.IsDisposing;
			boolVal = disp.IsDisposed;
			disp.Dispose();
			valueTask = disp.DisposeAsync();
		}

		[TestMethod()]
		public void DisposableBaseAsyncTest()
		{
			this.DisposableBaseAsyncTestCore().GetAwaiter().GetResult();
		}

		private async Task DisposableBaseAsyncTestCore()
		{
			await using ConfiguredAsyncDisposable x = new EmptyDisposableBase().ConfigureAwait(false);
		}

		[TestMethod()]
		public void LanguageUtilsTest()
		{
			CultureInfo cinfo;
			cinfo = LanguageUtils.SetCulture("en");
			cinfo = LanguageUtils.SetCulture("ja");
		}

		[TestMethod()]
		public void SecureStringExtensionsTest()
		{
			bool boolVal;
			var sstr = new SecureString();
			boolVal = SecureStringExtensions.IsEqualWith(sstr, sstr);
			boolVal = sstr.IsEqualWith(sstr);
		}

		[TestMethod()]
		public void SerializationInfoExtensionsTest()
		{
			object objValue;
			string strValue;
			double numValue;

			var sinfo = new SerializationInfo(typeof(EmptySerializable), new FormatterConverter());
			var empty = new EmptySerializable();

			empty.GetObjectData(sinfo, default);

			objValue = SerializationInfoExtensions.GetValue<object>(sinfo, "obj");
			strValue = SerializationInfoExtensions.GetValue<string>(sinfo, "str");
			numValue = SerializationInfoExtensions.GetValue<double>(sinfo, "num");
			objValue = sinfo.GetValue<object>("obj");
			strValue = sinfo.GetValue<string>("str");
			numValue = sinfo.GetValue<double>("num");
		}

		[TestMethod()]
		public void StringExtensionsTest()
		{
			bool boolVal;
			string strVal;
			char[] charArray;
			ReadOnlyMemory<char> charRom;
			ReadOnlySpan<char> charRos;

			boolVal   = StringExtensions.TryToBoolean(string.Empty, out boolVal);
			boolVal   = StringExtensions.ToBoolean("true");
			strVal    = StringExtensions.Abridge(string.Empty, 4);
			strVal    = StringExtensions.RemoveControlChars(string.Empty);
			strVal    = StringExtensions.RemoveControlChars(string.Empty, true);
			strVal    = StringExtensions.RemoveControlChars(string.Empty,               ControlCharsReplaceMode.RemoveAll);
			strVal    = StringExtensions.RemoveControlChars(string.Empty,               ControlCharsReplaceMode.RemoveAll, false);
			strVal    = StringExtensions.RemoveControlChars(string.Empty,               ControlCharsReplaceMode.RemoveAll, false, true);
			strVal    = StringExtensions.RemoveControlChars(string.Empty,               ControlCharsReplaceMode.RemoveAll, false, true, false);
			charArray = StringExtensions.RemoveControlChars(Array.Empty<char>(),        ControlCharsReplaceMode.RemoveAll);
			charArray = StringExtensions.RemoveControlChars(Array.Empty<char>(),        ControlCharsReplaceMode.RemoveAll, false);
			charArray = StringExtensions.RemoveControlChars(Array.Empty<char>(),        ControlCharsReplaceMode.RemoveAll, false, true);
			charArray = StringExtensions.RemoveControlChars(Array.Empty<char>(),        ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRom   = StringExtensions.RemoveControlChars(new Memory<char>(),         ControlCharsReplaceMode.RemoveAll);
			charRom   = StringExtensions.RemoveControlChars(new Memory<char>(),         ControlCharsReplaceMode.RemoveAll, false);
			charRom   = StringExtensions.RemoveControlChars(new Memory<char>(),         ControlCharsReplaceMode.RemoveAll, false, true);
			charRom   = StringExtensions.RemoveControlChars(new Memory<char>(),         ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRom   = StringExtensions.RemoveControlChars(new ReadOnlyMemory<char>(), ControlCharsReplaceMode.RemoveAll);
			charRom   = StringExtensions.RemoveControlChars(new ReadOnlyMemory<char>(), ControlCharsReplaceMode.RemoveAll, false);
			charRom   = StringExtensions.RemoveControlChars(new ReadOnlyMemory<char>(), ControlCharsReplaceMode.RemoveAll, false, true);
			charRom   = StringExtensions.RemoveControlChars(new ReadOnlyMemory<char>(), ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRos   = StringExtensions.RemoveControlChars(new ReadOnlySpan<char>(),   ControlCharsReplaceMode.RemoveAll);
			charRos   = StringExtensions.RemoveControlChars(new ReadOnlySpan<char>(),   ControlCharsReplaceMode.RemoveAll, false);
			charRos   = StringExtensions.RemoveControlChars(new ReadOnlySpan<char>(),   ControlCharsReplaceMode.RemoveAll, false, true);
			charRos   = StringExtensions.RemoveControlChars(new ReadOnlySpan<char>(),   ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRos   = StringExtensions.RemoveControlChars(new Span<char>(),           ControlCharsReplaceMode.RemoveAll);
			charRos   = StringExtensions.RemoveControlChars(new Span<char>(),           ControlCharsReplaceMode.RemoveAll, false);
			charRos   = StringExtensions.RemoveControlChars(new Span<char>(),           ControlCharsReplaceMode.RemoveAll, false, true);
			charRos   = StringExtensions.RemoveControlChars(new Span<char>(),           ControlCharsReplaceMode.RemoveAll, false, true, false);

			boolVal   = string.Empty.TryToBoolean(out boolVal);
			boolVal   = "true".ToBoolean();
			strVal    = string.Empty.Abridge(4);
			strVal    = string.Empty.RemoveControlChars();
			strVal    = string.Empty.RemoveControlChars(true);
			strVal    = string.Empty              .RemoveControlChars(ControlCharsReplaceMode.RemoveAll);
			strVal    = string.Empty              .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false);
			strVal    = string.Empty              .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true);
			strVal    = string.Empty              .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true, false);
			charArray = Array.Empty<char>()       .RemoveControlChars(ControlCharsReplaceMode.RemoveAll);
			charArray = Array.Empty<char>()       .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false);
			charArray = Array.Empty<char>()       .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true);
			charArray = Array.Empty<char>()       .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRom   = new Memory<char>()        .RemoveControlChars(ControlCharsReplaceMode.RemoveAll);
			charRom   = new Memory<char>()        .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false);
			charRom   = new Memory<char>()        .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true);
			charRom   = new Memory<char>()        .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRom   = new ReadOnlyMemory<char>().RemoveControlChars(ControlCharsReplaceMode.RemoveAll);
			charRom   = new ReadOnlyMemory<char>().RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false);
			charRom   = new ReadOnlyMemory<char>().RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true);
			charRom   = new ReadOnlyMemory<char>().RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRos   = new ReadOnlySpan<char>()  .RemoveControlChars(ControlCharsReplaceMode.RemoveAll);
			charRos   = new ReadOnlySpan<char>()  .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false);
			charRos   = new ReadOnlySpan<char>()  .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true);
			charRos   = new ReadOnlySpan<char>()  .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true, false);
			charRos   = new Span<char>()          .RemoveControlChars(ControlCharsReplaceMode.RemoveAll);
			charRos   = new Span<char>()          .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false);
			charRos   = new Span<char>()          .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true);
			charRos   = new Span<char>()          .RemoveControlChars(ControlCharsReplaceMode.RemoveAll, false, true, false);
		}

		[Obsolete()]
		[TestMethod()]
		public void StringExtensionsObsoleteTest()
		{
			string strVal;
			strVal = StringExtensions.FitToLine(string.Empty);
			strVal = strVal.FitToLine();
		}

		[TestMethod()]
		public void StringUtilTest()
		{
			string str;

			str = StringUtil.GetRandomText();
			str = StringUtil.GetRandomText(1, 2);
			str = StringUtil.GetRandomText(3);
			str = StringUtil.GetRandomText(5, 6, new Random());
			str = StringUtil.GetRandomText(7, new Random());
		}

		[TestMethod()]
		public void VersionInfoTest()
		{
			VersionInfo.PrintAllAssemblies();

			var vi = VersionInfo.Library;
			vi = new VersionInfo(typeof(PublicAPICompatibilityTests).Assembly);

			Assembly asm;
			string str;
			Version? ver;

			asm = vi.Assembly;
			str = vi.Name;
			str = vi.DisplayName;
			str = vi.Copyright;
			ver = vi.Version;
			str = vi.CodeName;
			str = vi.GetCaption();
			str = vi.GetFullVersionString();
			str = vi.GetVersionString();

			vi.Print();
		}

		private sealed class EmptyClass { }

		private sealed class EmptyComparable : IComparable
		{
			private readonly int _value;

			internal EmptyComparable() { }
			internal EmptyComparable(int value)
			{
				_value = value;
			}

			public int CompareTo(object? obj)
			{
				if (obj is EmptyComparable e) {
					return _value.CompareTo(e._value);
				} else {
					return 0;
				}
			}

			public override string ToString()
			{
				return _value.ToString();
			}
		}

		private sealed class EmptyComparableT : IComparable<EmptyComparableT>
		{
			private readonly int _value;

			internal EmptyComparableT() { }
			internal EmptyComparableT(int value)
			{
				_value = value;
			}

			public int CompareTo(EmptyComparableT? other)
			{
				if (other is null) {
					return 0;
				} else {
					return _value.CompareTo(other._value);
				}
			}

			public override string ToString()
			{
				return _value.ToString();
			}
		}

		private sealed class EmptyDisposableBase : DisposableBase
		{
			public void EnsureNotDisposedTest()
			{
				this.EnsureNotDisposed();
			}

			public void ThrowIfDisposedTest()
			{
				this.ThrowIfDisposed();
			}

			protected override void Dispose(bool disposing)
			{
				base.Dispose(disposing);
			}

			protected override async ValueTask DisposeAsyncCore()
			{
				await base.DisposeAsyncCore();
			}
		}

		[Serializable()]
		private sealed class EmptySerializable : ISerializable
		{
			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				info.AddValue("obj", new object());
				info.AddValue("str", string.Empty);
				info.AddValue("num", 0.0D);
			}
		}
	}
}
