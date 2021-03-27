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
	public readonly struct Range : IEquatable<Range>
	{
		public Index Start { get; }
		public Index End   { get; }

		public Range(Index start, Index end)
		{
			this.Start = start;
			this.End   = end;
		}

		public (int Offset, int Length) GetOffsetAndLength(int length)
		{
			int start = this.Start.GetOffset(length);
			int end   = this.End  .GetOffset(length);
			return (start, end - start);
		}

		public override bool Equals(object obj)
		{
			if (obj is Range other) {
				return this.Equals(other);
			} else {
				return false;
			}
		}

		public bool Equals(Range other)
		{
			return this.Start.Equals(other.Start) && this.End.Equals(other.End);
		}

		public override int GetHashCode()
		{
			return this.Start.GetHashCode() ^ this.End.GetHashCode();
		}
	}
}

#pragma warning restore CS1591 // 公開されている型またはメンバーの XML コメントがありません
#endif
