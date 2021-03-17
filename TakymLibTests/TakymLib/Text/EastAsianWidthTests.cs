/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TakymLib.Text;

namespace TakymLibTests.TakymLib.Text
{
	[TestClass()]
	public class EastAsianWidthTests
	{
		[TestMethod()]
		public void DownloadLatestDefinitionAndConvert()
		{
			var sb     = new StringBuilder();
			var ranges = EastAsianWidth.DownloadLatestDefinition().Ranges;
			int count  = ranges.Count;
			for (int i = 0; i < count; ++i) {
				var range = ranges[i];
				if (range.Start == range.End) {
					sb.Append("\'\\u");
					sb.AppendFormat("{0:X04}", ((ushort)(range.Start)));
					sb.Append('\'');
				} else {
					sb.Append(">= \'\\u");
					sb.AppendFormat("{0:X04}", ((ushort)(range.Start)));
					sb.Append("\' and <= \'\\u");
					sb.AppendFormat("{0:X04}", ((ushort)(range.End)));
					sb.Append('\'');
				}
				sb.Append(" => ");
				sb.Append(nameof(EastAsianWidthType));
				sb.Append('.');
				sb.Append(range.Type);
				sb.Append(',');
			}
			Assert.Fail(sb.ToString());
		}
	}
}
