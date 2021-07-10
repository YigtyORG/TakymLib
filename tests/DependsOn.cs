/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Globalization;
using TakymLib;

namespace TakymLibTests
{
	public static class DependsOn
	{
		public static void Language(string? locale = null)
		{
			LanguageUtility.SetCulture(locale);
		}

		public static void DefaultCulture(string? locale = null)
		{
			CultureInfo.DefaultThreadCurrentCulture = LanguageUtility.GetCulture(locale);
		}

		public static void DefaultUICulture(string? locale = null)
		{
			CultureInfo.DefaultThreadCurrentUICulture = LanguageUtility.GetCulture(locale);
		}

		public static void CurrentCulture(string? locale = null)
		{
			CultureInfo.CurrentCulture = LanguageUtility.GetCulture(locale);
		}

		public static void CurrentUICulture(string? locale = null)
		{
			CultureInfo.CurrentUICulture = LanguageUtility.GetCulture(locale);
		}
	}
}
