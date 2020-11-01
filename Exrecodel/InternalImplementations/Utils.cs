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
using System.Xml;

namespace Exrecodel.InternalImplementations
{
	internal static class Utils
	{
		internal static CultureInfo? FormatProviderToCultureInfo(IFormatProvider? formatProvider)
		{
			if (formatProvider is CultureInfo ci) {
				return ci;
			} else if (formatProvider?.GetFormat(typeof(CultureInfo)) is CultureInfo ci2) {
				return ci2;
			} else {
				return CultureInfo.CurrentCulture;
			}
		}

		internal static XmlElement GetElement(Func<string, XmlElement> createElemFunc, XmlElement elem, params string[] names)
		{
			for (int i = 0; i < names.Length; ++i) {
				if (elem[names[i]] == null) {
					elem.AppendChild(createElemFunc(names[i]));
				}
				elem = elem[names[i]];
			}
			return elem;
		}
	}
}
