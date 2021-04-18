/****
 * Ywando
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ywando
{
	/// <summary>
	///  <see langword="Ywando"/>の命令を表します。
	/// </summary>
	[DataContract(IsReference = true)]
	public abstract class YwandoInstruction
	{
		/// <summary>
		///  上書きされた場合は、この命令を実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		public abstract void Invoke(ExecutionContext context);

		/// <summary>
		///  上書きされた場合は、この命令を非同期的に実行します。
		/// </summary>
		/// <param name="context">実行文脈情報です。</param>
		/// <returns>この処理の非同期操作です。</returns>
		public virtual ValueTask InvokeAsync(ExecutionContext context)
		{
			this.Invoke(context);
			return default;
		}
	}
}
