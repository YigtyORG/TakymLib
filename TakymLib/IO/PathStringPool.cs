/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Collections.Generic;

namespace TakymLib.IO
{
	internal static class PathStringPool
	{
		private static readonly Dictionary<object, PathString> _cache;

		static PathStringPool()
		{
			_cache = new();
		}

		internal static PathString Get()
		{
			return Get(Environment.CurrentDirectory);
		}

		internal static PathString Get(string path)
		{
			path.EnsureNotNull(nameof(path));
			if (!_cache.TryGetValue(path, out var result)) {
				result = new PathString(path);
				_cache.Add(result, result);
			}
			return result;
		}
	}
}
