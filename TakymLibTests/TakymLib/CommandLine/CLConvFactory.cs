/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using TakymLib.CommandLine;

namespace TakymLibTests.TakymLib.CommandLine
{
	internal static class CLConvFactory
	{
		internal static CommandLineConverter Create(params string[] args)
		{
			return new(args);
		}
	}
}
