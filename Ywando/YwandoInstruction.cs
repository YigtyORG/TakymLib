/****
 * Ywando
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.Serialization;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>の命令を表します。
	/// </summary>
	[DataContract(IsReference = true)]
	public abstract class YwandoInstruction
	{
	}
}
