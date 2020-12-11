/****
 * Exrecodel - "Extensible Regulation/Convention Descriptor Language"
 *    「拡張可能な規則/規約記述言語」
 * Copyright (C) 2020 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020 Takym.
 *
 * distributed under the MIT License.
****/

using System.Text;
using Exrecodel.Properties;

namespace Exrecodel.InternalImplementations
{
	internal static class ConverterHelper
	{
		internal static void AppendStartContactInfo(this StringBuilder sb, XrcdlContactInformation info)
		{
			string? caption = HtmlTexts.ResourceManager.GetString("ContactInfo_" + info.GetContactType(), info.Metadata.Language);
			sb.Append($"<address class=\"{info.GetContactType()}\">");
			sb.Append($"<h3>{caption}</h3>");
		}

		internal static void AppendEndContactInfo(this StringBuilder sb)
		{
			sb.Append("</address>");
		}
	}
}
