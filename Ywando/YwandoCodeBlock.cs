/****
 * Ywando
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

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

		/// <summary>
		///  現在のコードブロックに格納されている命令を順番通りに実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		public override void Invoke(ExecutionContext context)
		{
			var insts = _insts;
			for (int i = 0; i < insts.Length; ++i) {
				insts[i].Invoke(context);
			}
		}

		/// <summary>
		///  現在のコードブロックに格納されている命令を順番通り非同期的に実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <returns>この処理の非同期操作です。</returns>
		public override async ValueTask InvokeAsync(ExecutionContext context)
		{
			var insts = _insts;
			for (int i = 0; i < insts.Length; ++i) {
				await insts[i].InvokeAsync(context);
			}
		}
	}
}
