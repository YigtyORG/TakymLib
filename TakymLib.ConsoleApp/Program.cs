/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Text;
using TakymLib.Text;

namespace TakymLib.ConsoleApp
{
	internal static class Program
	{
		private static void Run()
		{
			DownloadLatestDefinitionAndConvert();
		}

		private static void DownloadLatestDefinitionAndConvert()
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
			Console.WriteLine(sb.ToString());
		}

		[STAThread()]
		private static int Main(string[] args)
		{
			try {
				Run();
				return 0;
			} catch (Exception e) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine();
				Console.Error.WriteLine(e.ToString());
				Console.ResetColor();
				return e.HResult;
			}
		}
	}
}
