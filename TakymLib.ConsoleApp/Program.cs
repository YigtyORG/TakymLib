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
				if (range.Type == EastAsianWidthType.Invalid) {
					continue;
				}
				if (range.Start > ushort.MaxValue || range.End > ushort.MaxValue) {
					continue;
				}
				if (range.Start == range.End) {
					sb.AppendFormat("\'\\u{0:X04}\'", range.Start);
				} else {
					sb.AppendFormat(">= \'\\u{0:X04}\' and <= \'\\u{1:X04}\'", range.Start, range.End);
				}
				sb.Append(" => ");
				sb.Append(nameof(EastAsianWidthType));
				sb.Append('.');
				sb.Append(range.Type);
				sb.AppendLine(",");
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
