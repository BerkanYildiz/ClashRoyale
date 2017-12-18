namespace ClashRoyale.Compression.LZMA.Compress.LZ
{
    using System;
    using System.IO;

    using ClashRoyale.Compression.LZMA.Common;

    public class BinTree : InWindow, IMatchFinder
    {
        private const uint kBT2HashSize = 1 << 16;

        private const uint kEmptyHashValue = 0;

        private const uint kHash2Size = 1 << 10;

        private const uint kHash3Offset = BinTree.kHash2Size;

        private const uint kHash3Size = 1 << 16;

        private const uint kMaxValForNormalize = ((UInt32)1 << 31) - 1;

        private const uint kStartMaxLen = 1;

        private uint CutValue = 0xFF;

        private uint CyclicBufferPos;

        private uint CyclicBufferSize;

        private uint[] Hash;

        private uint HashMask;

        private uint HashSizeSum;

        private uint MatchMaxLen;

        private uint[] Son;

        private bool HashArray = true;

        private uint KFixHashSize = BinTree.kHash2Size + BinTree.kHash3Size;

        private uint KMinMatchCheck = 4;

        private uint KNumHashDirectBytes;

        public void Create(uint HistorySize, uint KeepAddBufferBefore, uint MatchMaxLen, uint KeepAddBufferAfter)
        {
            if (HistorySize > BinTree.kMaxValForNormalize - 256)
            {
                throw new Exception();
            }

            this.CutValue = 16 + (MatchMaxLen >> 1);
            uint WindowReservSize = (HistorySize + KeepAddBufferBefore + MatchMaxLen + KeepAddBufferAfter) / 2 + 256;
            this.Create(HistorySize + KeepAddBufferBefore, MatchMaxLen + KeepAddBufferAfter, WindowReservSize);
            this.MatchMaxLen = MatchMaxLen;
            uint CyclicBufferSize = HistorySize + 1;
            if (this.CyclicBufferSize != CyclicBufferSize)
            {
                this.Son = new uint[(this.CyclicBufferSize = CyclicBufferSize) * 2];
            }

            uint hs = BinTree.kBT2HashSize;
            if (this.HashArray)
            {
                hs = HistorySize - 1;
                hs |= hs >> 1;
                hs |= hs >> 2;
                hs |= hs >> 4;
                hs |= hs >> 8;
                hs >>= 1;
                hs |= 0xFFFF;
                if (hs > 1 << 24)
                {
                    hs >>= 1;
                }

                this.HashMask = hs;
                hs++;
                hs += this.KFixHashSize;
            }

            if (hs != this.HashSizeSum)
            {
                this.Hash = new uint[this.HashSizeSum = hs];
            }
        }

        public new byte GetIndexByte(int Index)
        {
            return base.GetIndexByte(Index);
        }

        public uint GetMatches(uint[] Distances)
        {
            uint LenLimit;
            if (this.Pos + this.MatchMaxLen <= this.StreamPos)
            {
                LenLimit = this.MatchMaxLen;
            }
            else
            {
                LenLimit = this.StreamPos - this.Pos;
                if (LenLimit < this.KMinMatchCheck)
                {
                    this.MovePos();
                    return 0;
                }
            }

            uint offset = 0;
            uint MatchMinPos = this.Pos > this.CyclicBufferSize ? this.Pos - this.CyclicBufferSize : 0;
            uint cur = this.BufferOffset + this.Pos;
            uint MaxLen = BinTree.kStartMaxLen; // to avoid items for len < hashSize;
            uint HashValue, Hash2Value = 0, Hash3Value = 0;
            if (this.HashArray)
            {
                uint temp = Crc.Table[this.BufferBase[cur]] ^ this.BufferBase[cur + 1];
                Hash2Value = temp & (BinTree.kHash2Size - 1);
                temp ^= (UInt32)this.BufferBase[cur + 2] << 8;
                Hash3Value = temp & (BinTree.kHash3Size - 1);
                HashValue = (temp ^ (Crc.Table[this.BufferBase[cur + 3]] << 5)) & this.HashMask;
            }
            else
            {
                HashValue = this.BufferBase[cur] ^ ((UInt32)this.BufferBase[cur + 1] << 8);
            }

            uint CurMatch = this.Hash[this.KFixHashSize + HashValue];
            if (this.HashArray)
            {
                uint CurMatch2 = this.Hash[Hash2Value];
                uint CurMatch3 = this.Hash[BinTree.kHash3Offset + Hash3Value];
                this.Hash[Hash2Value] = this.Pos;
                this.Hash[BinTree.kHash3Offset + Hash3Value] = this.Pos;
                if (CurMatch2 > MatchMinPos)
                {
                    if (this.BufferBase[this.BufferOffset + CurMatch2] == this.BufferBase[cur])
                    {
                        Distances[offset++] = MaxLen = 2;
                        Distances[offset++] = this.Pos - CurMatch2 - 1;
                    }
                }

                if (CurMatch3 > MatchMinPos)
                {
                    if (this.BufferBase[this.BufferOffset + CurMatch3] == this.BufferBase[cur])
                    {
                        if (CurMatch3 == CurMatch2)
                        {
                            offset -= 2;
                        }

                        Distances[offset++] = MaxLen = 3;
                        Distances[offset++] = this.Pos - CurMatch3 - 1;
                        CurMatch2 = CurMatch3;
                    }
                }

                if (offset != 0 && CurMatch2 == CurMatch)
                {
                    offset -= 2;
                    MaxLen = BinTree.kStartMaxLen;
                }
            }

            this.Hash[this.KFixHashSize + HashValue] = this.Pos;
            uint ptr0 = (this.CyclicBufferPos << 1) + 1;
            uint ptr1 = this.CyclicBufferPos << 1;
            uint len0, len1;
            len0 = len1 = this.KNumHashDirectBytes;
            if (this.KNumHashDirectBytes != 0)
            {
                if (CurMatch > MatchMinPos)
                {
                    if (this.BufferBase[this.BufferOffset + CurMatch + this.KNumHashDirectBytes] != this.BufferBase[cur + this.KNumHashDirectBytes])
                    {
                        Distances[offset++] = MaxLen = this.KNumHashDirectBytes;
                        Distances[offset++] = this.Pos - CurMatch - 1;
                    }
                }
            }

            uint count = this.CutValue;
            while (true)
            {
                if (CurMatch <= MatchMinPos || count-- == 0)
                {
                    this.Son[ptr0] = this.Son[ptr1] = BinTree.kEmptyHashValue;
                    break;
                }

                uint delta = this.Pos - CurMatch;
                uint CyclicPos = (delta <= this.CyclicBufferPos ? this.CyclicBufferPos - delta : this.CyclicBufferPos - delta + this.CyclicBufferSize) << 1;
                uint pby1 = this.BufferOffset + CurMatch;
                uint len = Math.Min(len0, len1);
                if (this.BufferBase[pby1 + len] == this.BufferBase[cur + len])
                {
                    while (++len != LenLimit)
                    {
                        if (this.BufferBase[pby1 + len] != this.BufferBase[cur + len])
                        {
                            break;
                        }
                    }

                    if (MaxLen < len)
                    {
                        Distances[offset++] = MaxLen = len;
                        Distances[offset++] = delta - 1;
                        if (len == LenLimit)
                        {
                            this.Son[ptr1] = this.Son[CyclicPos];
                            this.Son[ptr0] = this.Son[CyclicPos + 1];
                            break;
                        }
                    }
                }

                if (this.BufferBase[pby1 + len] < this.BufferBase[cur + len])
                {
                    this.Son[ptr1] = CurMatch;
                    ptr1 = CyclicPos + 1;
                    CurMatch = this.Son[ptr1];
                    len1 = len;
                }
                else
                {
                    this.Son[ptr0] = CurMatch;
                    ptr0 = CyclicPos;
                    CurMatch = this.Son[ptr0];
                    len0 = len;
                }
            }

            this.MovePos();
            return offset;
        }

        public new uint GetMatchLen(int Index, uint Distance, uint Limit)
        {
            return base.GetMatchLen(Index, Distance, Limit);
        }

        public new uint GetNumAvailableBytes()
        {
            return base.GetNumAvailableBytes();
        }

        public new void Init()
        {
            base.Init();
            for (uint i = 0; i < this.HashSizeSum; i++)
            {
                this.Hash[i] = BinTree.kEmptyHashValue;
            }

            this.CyclicBufferPos = 0;
            this.ReduceOffsets(-1);
        }

        public new void MovePos()
        {
            if (++this.CyclicBufferPos >= this.CyclicBufferSize)
            {
                this.CyclicBufferPos = 0;
            }

            base.MovePos();
            if (this.Pos == BinTree.kMaxValForNormalize)
            {
                this.Normalize();
            }
        }

        public new void ReleaseStream()
        {
            base.ReleaseStream();
        }

        public void SetCutValue(uint CutValue)
        {
            this.CutValue = CutValue;
        }

        public new void SetStream(Stream Stream)
        {
            base.SetStream(Stream);
        }

        public void SetType(int NumHashBytes)
        {
            this.HashArray = NumHashBytes > 2;
            if (this.HashArray)
            {
                this.KNumHashDirectBytes = 0;
                this.KMinMatchCheck = 4;
                this.KFixHashSize = BinTree.kHash2Size + BinTree.kHash3Size;
            }
            else
            {
                this.KNumHashDirectBytes = 2;
                this.KMinMatchCheck = 2 + 1;
                this.KFixHashSize = 0;
            }
        }

        public void Skip(uint Num)
        {
            do
            {
                uint LenLimit;
                if (this.Pos + this.MatchMaxLen <= this.StreamPos)
                {
                    LenLimit = this.MatchMaxLen;
                }
                else
                {
                    LenLimit = this.StreamPos - this.Pos;
                    if (LenLimit < this.KMinMatchCheck)
                    {
                        this.MovePos();
                        continue;
                    }
                }

                uint MatchMinPos = this.Pos > this.CyclicBufferSize ? this.Pos - this.CyclicBufferSize : 0;
                uint cur = this.BufferOffset + this.Pos;
                uint HashValue;
                if (this.HashArray)
                {
                    uint temp = Crc.Table[this.BufferBase[cur]] ^ this.BufferBase[cur + 1];
                    uint Hash2Value = temp & (BinTree.kHash2Size - 1);
                    this.Hash[Hash2Value] = this.Pos;
                    temp ^= (UInt32)this.BufferBase[cur + 2] << 8;
                    uint Hash3Value = temp & (BinTree.kHash3Size - 1);
                    this.Hash[BinTree.kHash3Offset + Hash3Value] = this.Pos;
                    HashValue = (temp ^ (Crc.Table[this.BufferBase[cur + 3]] << 5)) & this.HashMask;
                }
                else
                {
                    HashValue = this.BufferBase[cur] ^ ((UInt32)this.BufferBase[cur + 1] << 8);
                }

                uint CurMatch = this.Hash[this.KFixHashSize + HashValue];
                this.Hash[this.KFixHashSize + HashValue] = this.Pos;
                uint ptr0 = (this.CyclicBufferPos << 1) + 1;
                uint ptr1 = this.CyclicBufferPos << 1;
                uint len0, len1;
                len0 = len1 = this.KNumHashDirectBytes;
                uint count = this.CutValue;
                while (true)
                {
                    if (CurMatch <= MatchMinPos || count-- == 0)
                    {
                        this.Son[ptr0] = this.Son[ptr1] = BinTree.kEmptyHashValue;
                        break;
                    }

                    uint delta = this.Pos - CurMatch;
                    uint CyclicPos = (delta <= this.CyclicBufferPos ? this.CyclicBufferPos - delta : this.CyclicBufferPos - delta + this.CyclicBufferSize) << 1;
                    uint pby1 = this.BufferOffset + CurMatch;
                    uint len = Math.Min(len0, len1);
                    if (this.BufferBase[pby1 + len] == this.BufferBase[cur + len])
                    {
                        while (++len != LenLimit)
                        {
                            if (this.BufferBase[pby1 + len] != this.BufferBase[cur + len])
                            {
                                break;
                            }
                        }

                        if (len == LenLimit)
                        {
                            this.Son[ptr1] = this.Son[CyclicPos];
                            this.Son[ptr0] = this.Son[CyclicPos + 1];
                            break;
                        }
                    }

                    if (this.BufferBase[pby1 + len] < this.BufferBase[cur + len])
                    {
                        this.Son[ptr1] = CurMatch;
                        ptr1 = CyclicPos + 1;
                        CurMatch = this.Son[ptr1];
                        len1 = len;
                    }
                    else
                    {
                        this.Son[ptr0] = CurMatch;
                        ptr0 = CyclicPos;
                        CurMatch = this.Son[ptr0];
                        len0 = len;
                    }
                }

                this.MovePos();
            }
            while (--Num != 0);
        }

        private void Normalize()
        {
            uint SubValue = this.Pos - this.CyclicBufferSize;
            this.NormalizeLinks(this.Son, this.CyclicBufferSize * 2, SubValue);
            this.NormalizeLinks(this.Hash, this.HashSizeSum, SubValue);
            this.ReduceOffsets((Int32)SubValue);
        }

        private void NormalizeLinks(uint[] Items, uint NumItems, uint SubValue)
        {
            for (uint i = 0; i < NumItems; i++)
            {
                uint value = Items[i];
                if (value <= SubValue)
                {
                    value = BinTree.kEmptyHashValue;
                }
                else
                {
                    value -= SubValue;
                }

                Items[i] = value;
            }
        }
    }
}