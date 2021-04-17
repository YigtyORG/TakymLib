/****
 * Ywando
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>のコードブロックを表します。
	/// </summary>
	[DataContract(IsReference = true)]
	public class YwandoCodeBlock : YwandoInstruction
	{
		private YwandoInstruction[] _insts;

		/// <summary>
		///  現在のコードブロックに格納されている命令の配列を取得または設定します。
		/// </summary>
		[DataMember(IsRequired = true)]
		public YwandoInstruction[] Instructions
		{
			get => _insts;
			set => _insts = value ?? Array.Empty<YwandoInstruction>();
		}

		/// <summary>
		///  型'<see cref="Ywando.YwandoCodeBlock"/>'の新しいインスタンスを生成します。
		/// </summary>
		public YwandoCodeBlock()
		{
			_insts = Array.Empty<YwandoInstruction>();
		}
	}
}
