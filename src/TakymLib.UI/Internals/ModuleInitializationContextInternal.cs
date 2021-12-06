/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Collections.Generic;
using TakymLib.Extensibility;

namespace TakymLib.UI.Internals
{
	internal sealed class ModuleInitializationContextInternal : ModuleInitializationContext
	{
		internal List<FeatureModule> Modules { get; }

		public ModuleInitializationContextInternal()
		{
			this.Modules = new();
		}
	}
}
