/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Globalization;

namespace TakymLib
{
	/// <summary>
	///  言語に関する便利関数を提供します。
	///  このクラスは静的クラスです。
	/// </summary>
	public static class LanguageUtils
	{
		/// <summary>
		///  既定の言語を設定します。
		/// </summary>
		/// <param name="locale">設定する言語のロケールです。</param>
		/// <returns>新しい既定の言語を表す<see cref="System.Globalization.CultureInfo"/>オブジェクトです。</returns>
		public static CultureInfo SetCulture(string locale)
		{
			var cinfo = CultureInfo.GetCultureInfo(locale);
			CultureInfo.DefaultThreadCurrentCulture   = cinfo;
			CultureInfo.DefaultThreadCurrentUICulture = cinfo;
			CultureInfo.CurrentCulture                = cinfo;
			CultureInfo.CurrentUICulture              = cinfo;
			return cinfo;
		}
	}
}
