/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.Net;
using System.Text;
using TakymLib.Text;

namespace TakymLib.ConsoleApp
{
	internal static class Program
	{
		private static void Run()
		{
			//DownloadLatestDefinitionAndConvert();
			using (var wc = new WebClient()) {
				DownloadEAWDefinitionAndConvert("4.1.0", wc);
				DownloadEAWDefinitionAndConvert("5.0.0", wc);
				DownloadEAWDefinitionAndConvert("5.1.0", wc);
				DownloadEAWDefinitionAndConvert("5.2.0", wc);
				DownloadEAWDefinitionAndConvert("6.0.0", wc);
				DownloadEAWDefinitionAndConvert("6.1.0", wc);
				DownloadEAWDefinitionAndConvert("6.2.0", wc);
				DownloadEAWDefinitionAndConvert("6.3.0", wc);
				DownloadEAWDefinitionAndConvert("7.0.0", wc);
				DownloadEAWDefinitionAndConvert("8.0.0", wc);
				DownloadEAWDefinitionAndConvert("9.0.0", wc);
				DownloadEAWDefinitionAndConvert("10.0.0", wc);
				DownloadEAWDefinitionAndConvert("11.0.0", wc);
				DownloadEAWDefinitionAndConvert("12.0.0", wc);
				DownloadEAWDefinitionAndConvert("12.1.0", wc);
				DownloadEAWDefinitionAndConvert("13.0.0", wc);
			}
		}

		private static void DownloadEAWDefinitionAndConvert(string version, WebClient wc)
		{
			var ranges = EastAsianWidth.DownloadDefinitionFrom($"https://www.unicode.org/Public/{version}/ucd/EastAsianWidth.txt", wc).Ranges;
			int count  = ranges.Count;
			using (var fs = new FileStream(version + ".txt", FileMode.Create, FileAccess.Write, FileShare.None))
			using (var sw = new StreamWriter(fs, Encoding.UTF8)) {
				for (int i = 0; i < count; ++i) {
					var range = ranges[i];
					if (range.Type == EastAsianWidthType.Invalid) {
						continue;
					}
					if (range.Start > ushort.MaxValue || range.End > ushort.MaxValue) {
						continue;
					}
					if (range.Start == range.End) {
						sw.Write("\'\\u{0:X04}\'", range.Start);
					} else {
						sw.Write(">= \'\\u{0:X04}\' and <= \'\\u{1:X04}\'", range.Start, range.End);
					}
					sw.Write(" => ");
					sw.Write(nameof(EastAsianWidthType));
					sw.Write('.');
					sw.Write(range.Type);
					sw.WriteLine(",");
				}
			}
			Console.WriteLine(version);
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
				VersionInfo.Current.Print();
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
