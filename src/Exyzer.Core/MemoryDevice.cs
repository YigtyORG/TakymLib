/****
 * Exyzer
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

using System;

namespace Exyzer
{
	/// <summary>
	///  メモリを表します。
	///  このクラスは抽象クラスです。
	/// </summary>
	public abstract class MemoryDevice : IDevice
	{
		/// <inheritdoc/>
		public virtual string Name => nameof(MemoryDevice);

		/// <inheritdoc/>
		public abstract Guid Guid { get; }

		/// <summary>
		///  メモリデータを取得します。
		/// </summary>
		public abstract Span<byte> Data { get; }

		/// <summary>
		///  外部からの読み取りが可能かどうかを示す論理値を取得します。
		/// </summary>
		/// <returns>
		///  読み取り可能である場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public abstract bool CanRead { get; }

		/// <summary>
		///  外部からの書き込みが可能かどうかを示す論理値を取得します。
		/// </summary>
		/// <returns>
		///  書き込み可能である場合は<see langword="true"/>、
		///  それ以外の場合は<see langword="false"/>を返します。
		/// </returns>
		public abstract bool CanWrite { get; }

		/// <inheritdoc/>
		public IOResult Input(int address, out int data)
		{
			if (this.CanRead) {
				if (address < 0) {
					data = 0;
					return IOResult.OutOfRange;
				}
				var span = this.Data;
				if (address >= span.Length - 4) {
					data = 0;
					return IOResult.OutOfRange;
				}
				data = BitConverter.ToInt32(span[address..]);
				return IOResult.Success;
			} else {
				data = 0;
				return IOResult.AccessDenied;
			}
		}

		/// <inheritdoc/>
		public IOResult Input(long address, out long data)
		{
			if (this.CanRead) {
				if (address < 0) {
					data = 0;
					return IOResult.OutOfRange;
				}
				var span = this.Data;
				if (address >= span.Length - 8 || address > int.MaxValue) {
					data = 0;
					return IOResult.OutOfRange;
				}
				data = BitConverter.ToInt64(span[((int)(address))..]);
				return IOResult.Success;
			} else {
				data = 0;
				return IOResult.AccessDenied;
			}
		}

		/// <inheritdoc/>
		public IOResult Output(int address, int data)
		{
			if (this.CanWrite) {
				if (address < 0) {
					return IOResult.OutOfRange;
				}
				var span = this.Data;
				if (address >= span.Length - 4) {
					return IOResult.OutOfRange;
				}
				if (BitConverter.TryWriteBytes(span[address..], data)) {
					return IOResult.Success;
				} else {
					return IOResult.Failed;
				}
			} else {
				return IOResult.AccessDenied;
			}
		}

		/// <inheritdoc/>
		public IOResult Output(long address, long data)
		{
			if (this.CanWrite) {
				if (address < 0) {
					return IOResult.OutOfRange;
				}
				var span = this.Data;
				if (address >= span.Length - 8 || address > int.MaxValue) {
					return IOResult.OutOfRange;
				}
				if (BitConverter.TryWriteBytes(span[((int)(address))..], data)) {
					return IOResult.Success;
				} else {
					return IOResult.Failed;
				}
			} else {
				return IOResult.AccessDenied;
			}
		}
	}
}
