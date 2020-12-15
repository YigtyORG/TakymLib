/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Globalization;
using Exrecodel.ContactInfo;
using Exrecodel.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Exrecodel.Tests
{
	[TestClass()]
	public class ContactListTests
	{
		[TestMethod()]
		public void CreateTest()
		{
			var obj = XrcdlDocument.Create();
			Assert.IsNotNull(obj, nameof(obj));

			var meta = obj.GetMetadata();
			Assert.IsNotNull(meta, nameof(meta));

			var list = meta.Contacts;
			Assert.IsNotNull(list, nameof(list));
		}

		[TestMethod()]
		public void AppendTest()
		{
			var list1 = XrcdlDocument.Create()?.GetMetadata()?.Contacts;
			Assert.IsNotNull(list1, nameof(list1));

			var list2 = list1!; // suppress warnings

			// Address
			var addr = list2.AppendAddressInfo("ABCDEF", 1234);
			Assert.IsNotNull(addr, nameof(addr));
			Assert.AreEqual("ABCDEF", addr.Address);
			Assert.AreEqual(1234,     addr.PostCode);

			// Email
			var mail = list2.AppendEmailInfo("a@b.com", "件名", "本文");
			Assert.IsNotNull(mail, nameof(mail));
			Assert.AreEqual("a@b.com", mail.Address);
			Assert.AreEqual("件名",    mail.Subject);
			Assert.AreEqual("本文",    mail.Body);

			// Phone
			var tel = list2.AppendTelInfo("123-456-789");
			var fax = list2.AppendFaxInfo("456-789-123");
			var sms = list2.AppendSmsInfo("789-123-456");
			Assert.IsNotNull(tel, nameof(tel));
			Assert.IsNotNull(fax, nameof(fax));
			Assert.IsNotNull(sms, nameof(sms));
			Assert.AreEqual(XrcdlPhoneNumberType.Telephone,           tel.Type);
			Assert.AreEqual(XrcdlPhoneNumberType.Facsimile,           fax.Type);
			Assert.AreEqual(XrcdlPhoneNumberType.ShortMessageService, sms.Type);
			tel.Normalize();
			fax.Normalize();
			sms.Normalize();
			Assert.AreEqual("123-456-789", tel.Number);
			Assert.AreEqual("456-789-123", fax.Number);
			Assert.AreEqual("789-123-456", sms.Number);

			// Link
			var link = list2.AppendLinkInfo(new Uri("https://abc.com/hello/world"), "Hello, World!!");
			Assert.IsNotNull(link, nameof(link));
			Assert.AreEqual("https://abc.com/hello/world", link.Link.OriginalString);
			Assert.AreEqual("Hello, World!!",              link.Title);

			// SNS
			var sns = list2.AppendSocialAccountInfo("foobar", new Uri("https://www.hoge.net/user/"), "FugaPiyo");
			Assert.IsNotNull(sns, nameof(sns));
			Assert.AreEqual("foobar",                     sns.AccountName);
			Assert.AreEqual("https://www.hoge.net/user/", sns.UriPrefix.OriginalString);
			Assert.AreEqual("FugaPiyo",                   sns.ServiceName);

			Assert.IsTrue(list2.Contains(addr), nameof(addr));
			Assert.IsTrue(list2.Contains(mail), nameof(mail));
			Assert.IsTrue(list2.Contains(tel),  nameof(tel));
			Assert.IsTrue(list2.Contains(fax),  nameof(fax));
			Assert.IsTrue(list2.Contains(sms),  nameof(sms));
			Assert.IsTrue(list2.Contains(link), nameof(link));
			Assert.IsTrue(list2.Contains(sns),  nameof(sns));
		}

		[TestMethod()]
		public void ConverterTest()
		{
			// Ensure the current culture is: ja.
			CultureInfo.CurrentCulture   = CultureInfo.GetCultureInfo("ja");
			CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

			var list = XrcdlDocument.Create().GetMetadata().Contacts;
			list.AppendAddressInfo("XYZ");
			list.AppendAddressInfo("ABCDEF", 1234);
			list.AppendEmailInfo("a@b.com");
			list.AppendEmailInfo("a@b.com", "件名", "本文");
			list.AppendTelInfo("123-456-789");
			list.AppendFaxInfo("456-789-123");
			list.AppendSmsInfo("789-123-456");
			list.AppendLinkInfo(new Uri("https://abc.com/index.html"));
			list.AppendLinkInfo(new Uri("https://abc.com/hello/world"), "Hello, World!!");
			list.AppendSocialAccountInfo("foobar", new Uri("https://www.hoge.net/user/"));
			list.AppendSocialAccountInfo("foobar", new Uri("https://www.hoge.net/user/"), "FugaPiyo");

			Assert.Fail(list.ConvertToHtml());
		}
	}
}
