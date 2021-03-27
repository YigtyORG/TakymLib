/****
 * TakymLib
 * Copyright (C) 2020-2021 Yigty.ORG; all rights reserved.
 * Copyright (C) 2020-2021 Takym.
 *
 * distributed under the MIT License.
****/

#if NET48
#pragma warning disable CS1591 // 公開されている型またはメンバーの XML コメントがありません

namespace System
{
	public readonly struct Index : IEquatable<Index>
	{
		public int  Value     { get; }
		public bool IsFromEnd { get; }

		public Index(int value, bool isFromEnd = false)
		{
			this.Value     = value;
			this.IsFromEnd = isFromEnd;
		}

		public int GetOffset(int length)
		{
			if (this.IsFromEnd) {
				return length - this.Value;
			} else {
				return this.Value;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is Index other) {
				return this.Equals(other);
			} else {
				return false;
			}
		}

		public bool Equals(Index other)
		{
			return this.Value == other.Value && this.IsFromEnd == other.IsFromEnd;
		}

		public override int GetHashCode()
		{
			return this.Value.GetHashCode() ^ this.IsFromEnd.GetHashCode();
		}

		public static implicit operator Index(int index) => new(index);
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
