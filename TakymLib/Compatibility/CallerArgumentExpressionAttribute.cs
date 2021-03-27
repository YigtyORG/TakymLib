/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if !NETCOREAPP3_1_OR_GREATER
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class CallerArgumentExpressionAttribute : Attribute
	{
		public string ParameterName { get; }

		public CallerArgumentExpressionAttribute(string name)
		{
			this.ParameterName = name;
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
