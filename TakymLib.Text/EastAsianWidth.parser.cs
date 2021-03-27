﻿/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace TakymLib.Text
{
	partial class EastAsianWidth
	{
		/// <summary>
		///  最新の東アジアの文字幅の定義データをダウンロードのダウンロード元のアドレスを取得します。
		/// </summary>
		/// <remarks>
		///  The Unicode Consortium が定める利用規約(http://www.unicode.org/terms_of_use.html)に従って利用してください。
		/// </remarks>
		public const string LatestDefinitionUrl = "https://www.unicode.org/Public/UCD/latest/ucd/EastAsianWidth.txt";

		/// <summary>
		///  最新の東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <remarks>
		///  The Unicode Consortium が定める利用規約(http://www.unicode.org/terms_of_use.html)に従って利用してください。
		/// </remarks>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		public static CustomEastAsianWidth DownloadLatestDefinition()
		{
			return DownloadDefinitionFrom(LatestDefinitionUrl);
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す文字列です。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(string url)
		{
			using (var wc = new WebClient()) {
				return DownloadDefinitionFrom(new Uri(url), wc);
			}
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す文字列です。</param>
		/// <param name="client">ダウンロードに利用するクライアントです。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(string url, WebClient client)
		{
			return DownloadDefinitionFrom(new Uri(url), client);
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す<see cref="System.Uri"/>オブジェクトです。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(Uri url)
		{
			using (var wc = new WebClient()) {
				return DownloadDefinitionFrom(url, wc);
			}
		}

		/// <summary>
		///  指定されたアドレスから東アジアの文字幅の定義データをダウンロードします。
		/// </summary>
		/// <param name="url">アドレスを表す<see cref="System.Uri"/>オブジェクトです。</param>
		/// <param name="client">ダウンロードに利用するクライアントです。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		public static CustomEastAsianWidth DownloadDefinitionFrom(Uri url, WebClient client)
		{
			url   .EnsureNotNull(nameof(url));
			client.EnsureNotNull(nameof(client));
			return Parse(client.DownloadString(url));
		}

		/// <summary>
		///  指定された文字列を解析します。
		/// </summary>
		/// <param name="s">解析する文字列です。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		public static CustomEastAsianWidth Parse(string? s)
		{
			using (var sr = new StringReader(s ?? string.Empty)) {
				return Parse(sr);
			}
		}

		/// <summary>
		///  指定されたストリームから入力される文字列を解析します。
		/// </summary>
		/// <param name="stream">ストリームです。</param>
		/// <param name="encoding">文字コードです。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static CustomEastAsianWidth Parse(Stream stream, Encoding? encoding = null)
		{
			stream.EnsureNotNull(nameof(stream));
			using (var sr = new StreamReader(stream, encoding ?? Encoding.UTF8, true, -1, true)) {
				return Parse(sr);
			}
		}

		/// <summary>
		///  指定されたテキストリーダーから入力される文字列を解析します。
		/// </summary>
		/// <param name="tr">テキストリーダーです。</param>
		/// <returns>解析結果を格納している<see cref="TakymLib.Text.CustomEastAsianWidth"/>オブジェクトです。</returns>
		/// <exception cref="System.ArgumentNullException"/>
		public static CustomEastAsianWidth Parse(TextReader tr)
		{
			tr.EnsureNotNull(nameof(tr));
			var ranges = new List<(int Start, int End, EastAsianWidthType Type)>();
			while (tr.ReadLine() is not null and string line) {
				ranges.Add(ParseLine(line));
			}
			return new(ranges.AsReadOnly());
		}

#pragma warning disable TakymLib_EAWInvalid // 型またはメンバーが旧型式です
		/// <summary>
		///  指定された行を解析し、範囲で表された東アジアの文字幅の列挙値へ変換します。
		/// </summary>
		/// <param name="line">解析する一行の文字列です。</param>
		/// <returns>範囲で表された東アジアの文字幅の列挙値です。</returns>
		public static (int Start, int End, EastAsianWidthType Type) ParseLine(string? line)
		{
			if (string.IsNullOrEmpty(line)) {
				return default;
			}
			int start = 0, end = 0, flags = 0;
			var type = EastAsianWidthType.Invalid;
			for (int i = 0; i < line.Length; ++i) {
				char ch = line[i];
				if (ch == ' ' || ch == '\t') {
					continue;
				}
				if ((flags & 2) == 2) {
					switch (ch) {
					case 'A':
						type = EastAsianWidthType.Ambiguous;
						break;
					case 'F':
						type = EastAsianWidthType.FullWidth;
						break;
					case 'H':
						type = EastAsianWidthType.HalfWidth;
						break;
					case 'N':
						type = EastAsianWidthType.Neutral;
						break;
					case 'a':
						type = EastAsianWidthType.Narrow;
						break;
					case 'W':
						type = EastAsianWidthType.Wide;
						break;
					case '#':
						goto end;
					}
				} else {
					switch (ch) {
					case >= '0' and <= '9':
						if ((flags & 1) == 1) {
							end <<= 4;
							end  |= ch - '0';
						} else {
							start <<= 4;
							start  |= ch - '0';
						}
						break;
					case >= 'A' and <= 'F':
						if ((flags & 1) == 1) {
							end <<= 4;
							end  |= (ch - 'A') + 0x0A;
						} else {
							start <<= 4;
							start  |= (ch - 'A') + 0x0A;
						}
						break;
					case >= 'a' and <= 'f':
						if ((flags & 1) == 1) {
							end <<= 4;
							end  |= (ch - 'a') + 0x0A;
						} else {
							start <<= 4;
							start  |= (ch - 'a') + 0x0A;
						}
						break;
					case '.':
						flags |= 1;
						break;
					case ';':
						flags |= 2;
						break;
					case '#':
						goto end;
					}
				}
			}
end:
			if ((flags & 1) == 1) {
				return (start, end, type);
			} else {
				return (start, start, type);
			}
		}
#pragma warning restore TakymLib_EAWInvalid // 型またはメンバーが旧型式です
	}
}