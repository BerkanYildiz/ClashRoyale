namespace ClashRoyale.Compression.Lzma.Compress.LZ
{
    using System;
    using System.IO;

    using ClashRoyale.Compression.Lzma.Common;

    public class BinTree : InWindow, IMatchFinder
    {
        private const uint kBT2HashSize = 1 << 16;

        private const uint kEmptyHashValue = 0;

        private const uint kHash2Size = 1 << 10;

        private const uint kHash3Offset = BinTree.kHash2Size;

        private const uint kHash3Size = 1 << 16;

        private const uint kMaxValForNormalize = ((UInt32)1 << 31) - 1;

        private const uint kStartMaxLen = 1;

        private uint _cutValue = 0xFF;

        private uint _cyclicBufferPos;

        private uint _cyclicBufferSize;

        private uint[] _hash;

        private uint _hashMask;

        private uint _hashSizeSum;

        private uint _matchMaxLen;

        private uint[] _son;

        private bool HASH_ARRAY = true;

        private uint kFixHashSize = BinTree.kHash2Size + BinTree.kHash3Size;

        private uint kMinMatchCheck = 4;

        private uint kNumHashDirectBytes;

        public void Create(uint historySize, uint keepAddBufferBefore, uint matchMaxLen, uint keepAddBufferAfter)
        {
            if (historySize > BinTree.kMaxValForNormalize - 256)
            {
                throw new Exception();
            }

            this._cutValue = 16 + (matchMaxLen >> 1);
            uint windowReservSize = (historySize + keepAddBufferBefore + matchMaxLen + keepAddBufferAfter) / 2 + 256;
            this.Create(historySize + keepAddBufferBefore, matchMaxLen + keepAddBufferAfter, windowReservSize);
            this._matchMaxLen = matchMaxLen;
            uint cyclicBufferSize = historySize + 1;
            if (this._cyclicBufferSize != cyclicBufferSize)
            {
                this._son = new uint[(this._cyclicBufferSize = cyclicBufferSize) * 2];
            }

            uint hs = BinTree.kBT2HashSize;
            if (this.HASH_ARRAY)
            {
                hs = historySize - 1;
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

                this._hashMask = hs;
                hs++;
                hs += this.kFixHashSize;
            }

            if (hs != this._hashSizeSum)
            {
                this._hash = new uint[this._hashSizeSum = hs];
            }
        }

        public new byte GetIndexByte(int index)
        {
            return base.GetIndexByte(index);
        }

        public uint GetMatches(uint[] distances)
        {
            uint lenLimit;
            if (this._pos + this._matchMaxLen <= this._streamPos)
            {
                lenLimit = this._matchMaxLen;
            }
            else
            {
                lenLimit = this._streamPos - this._pos;
                if (lenLimit < this.kMinMatchCheck)
                {
                    this.MovePos();
                    return 0;
                }
            }

            uint offset = 0;
            uint matchMinPos = this._pos > this._cyclicBufferSize ? this._pos - this._cyclicBufferSize : 0;
            uint cur = this._bufferOffset + this._pos;
            uint maxLen = BinTree.kStartMaxLen; // to avoid items for len < hashSize;
            uint hashValue, hash2Value = 0, hash3Value = 0;
            if (this.HASH_ARRAY)
            {
                uint temp = CRC.Table[this._bufferBase[cur]] ^ this._bufferBase[cur + 1];
                hash2Value = temp & (BinTree.kHash2Size - 1);
                temp ^= (UInt32)this._bufferBase[cur + 2] << 8;
                hash3Value = temp & (BinTree.kHash3Size - 1);
                hashValue = (temp ^ (CRC.Table[this._bufferBase[cur + 3]] << 5)) & this._hashMask;
            }
            else
            {
                hashValue = this._bufferBase[cur] ^ ((UInt32)this._bufferBase[cur + 1] << 8);
            }

            uint curMatch = this._hash[this.kFixHashSize + hashValue];
            if (this.HASH_ARRAY)
            {
                uint curMatch2 = this._hash[hash2Value];
                uint curMatch3 = this._hash[BinTree.kHash3Offset + hash3Value];
                this._hash[hash2Value] = this._pos;
                this._hash[BinTree.kHash3Offset + hash3Value] = this._pos;
                if (curMatch2 > matchMinPos)
                {
                    if (this._bufferBase[this._bufferOffset + curMatch2] == this._bufferBase[cur])
                    {
                        distances[offset++] = maxLen = 2;
                        distances[offset++] = this._pos - curMatch2 - 1;
                    }
                }

                if (curMatch3 > matchMinPos)
                {
                    if (this._bufferBase[this._bufferOffset + curMatch3] == this._bufferBase[cur])
                    {
                        if (curMatch3 == curMatch2)
                        {
                            offset -= 2;
                        }

                        distances[offset++] = maxLen = 3;
                        distances[offset++] = this._pos - curMatch3 - 1;
                        curMatch2 = curMatch3;
                    }
                }

                if (offset != 0 && curMatch2 == curMatch)
                {
                    offset -= 2;
                    maxLen = BinTree.kStartMaxLen;
                }
            }

            this._hash[this.kFixHashSize + hashValue] = this._pos;
            uint ptr0 = (this._cyclicBufferPos << 1) + 1;
            uint ptr1 = this._cyclicBufferPos << 1;
            uint len0, len1;
            len0 = len1 = this.kNumHashDirectBytes;
            if (this.kNumHashDirectBytes != 0)
            {
                if (curMatch > matchMinPos)
                {
                    if (this._bufferBase[this._bufferOffset + curMatch + this.kNumHashDirectBytes] != this._bufferBase[cur + this.kNumHashDirectBytes])
                    {
                        distances[offset++] = maxLen = this.kNumHashDirectBytes;
                        distances[offset++] = this._pos - curMatch - 1;
                    }
                }
            }

            uint count = this._cutValue;
            while (true)
            {
                if (curMatch <= matchMinPos || count-- == 0)
                {
                    this._son[ptr0] = this._son[ptr1] = BinTree.kEmptyHashValue;
                    break;
                }

                uint delta = this._pos - curMatch;
                uint cyclicPos = (delta <= this._cyclicBufferPos ? this._cyclicBufferPos - delta : this._cyclicBufferPos - delta + this._cyclicBufferSize) << 1;
                uint pby1 = this._bufferOffset + curMatch;
                uint len = Math.Min(len0, len1);
                if (this._bufferBase[pby1 + len] == this._bufferBase[cur + len])
                {
                    while (++len != lenLimit)
                    {
                        if (this._bufferBase[pby1 + len] != this._bufferBase[cur + len])
                        {
                            break;
                        }
                    }

                    if (maxLen < len)
                    {
                        distances[offset++] = maxLen = len;
                        distances[offset++] = delta - 1;
                        if (len == lenLimit)
                        {
                            this._son[ptr1] = this._son[cyclicPos];
                            this._son[ptr0] = this._son[cyclicPos + 1];
                            break;
                        }
                    }
                }

                if (this._bufferBase[pby1 + len] < this._bufferBase[cur + len])
                {
                    this._son[ptr1] = curMatch;
                    ptr1 = cyclicPos + 1;
                    curMatch = this._son[ptr1];
                    len1 = len;
                }
                else
                {
                    this._son[ptr0] = curMatch;
                    ptr0 = cyclicPos;
                    curMatch = this._son[ptr0];
                    len0 = len;
                }
            }

            this.MovePos();
            return offset;
        }

        public new uint GetMatchLen(int index, uint distance, uint limit)
        {
            return base.GetMatchLen(index, distance, limit);
        }

        public new uint GetNumAvailableBytes()
        {
            return base.GetNumAvailableBytes();
        }

        public new void Init()
        {
            base.Init();
            for (uint i = 0; i < this._hashSizeSum; i++)
            {
                this._hash[i] = BinTree.kEmptyHashValue;
            }

            this._cyclicBufferPos = 0;
            this.ReduceOffsets(-1);
        }

        public new void MovePos()
        {
            if (++this._cyclicBufferPos >= this._cyclicBufferSize)
            {
                this._cyclicBufferPos = 0;
            }

            base.MovePos();
            if (this._pos == BinTree.kMaxValForNormalize)
            {
                this.Normalize();
            }
        }

        public new void ReleaseStream()
        {
            base.ReleaseStream();
        }

        public void SetCutValue(uint cutValue)
        {
            this._cutValue = cutValue;
        }

        public new void SetStream(Stream stream)
        {
            base.SetStream(stream);
        }

        public void SetType(int numHashBytes)
        {
            this.HASH_ARRAY = numHashBytes > 2;
            if (this.HASH_ARRAY)
            {
                this.kNumHashDirectBytes = 0;
                this.kMinMatchCheck = 4;
                this.kFixHashSize = BinTree.kHash2Size + BinTree.kHash3Size;
            }
            else
            {
                this.kNumHashDirectBytes = 2;
                this.kMinMatchCheck = 2 + 1;
                this.kFixHashSize = 0;
            }
        }

        public void Skip(uint num)
        {
            do
            {
                uint lenLimit;
                if (this._pos + this._matchMaxLen <= this._streamPos)
                {
                    lenLimit = this._matchMaxLen;
                }
                else
                {
                    lenLimit = this._streamPos - this._pos;
                    if (lenLimit < this.kMinMatchCheck)
                    {
                        this.MovePos();
                        continue;
                    }
                }

                uint matchMinPos = this._pos > this._cyclicBufferSize ? this._pos - this._cyclicBufferSize : 0;
                uint cur = this._bufferOffset + this._pos;
                uint hashValue;
                if (this.HASH_ARRAY)
                {
                    uint temp = CRC.Table[this._bufferBase[cur]] ^ this._bufferBase[cur + 1];
                    uint hash2Value = temp & (BinTree.kHash2Size - 1);
                    this._hash[hash2Value] = this._pos;
                    temp ^= (UInt32)this._bufferBase[cur + 2] << 8;
                    uint hash3Value = temp & (BinTree.kHash3Size - 1);
                    this._hash[BinTree.kHash3Offset + hash3Value] = this._pos;
                    hashValue = (temp ^ (CRC.Table[this._bufferBase[cur + 3]] << 5)) & this._hashMask;
                }
                else
                {
                    hashValue = this._bufferBase[cur] ^ ((UInt32)this._bufferBase[cur + 1] << 8);
                }

                uint curMatch = this._hash[this.kFixHashSize + hashValue];
                this._hash[this.kFixHashSize + hashValue] = this._pos;
                uint ptr0 = (this._cyclicBufferPos << 1) + 1;
                uint ptr1 = this._cyclicBufferPos << 1;
                uint len0, len1;
                len0 = len1 = this.kNumHashDirectBytes;
                uint count = this._cutValue;
                while (true)
                {
                    if (curMatch <= matchMinPos || count-- == 0)
                    {
                        this._son[ptr0] = this._son[ptr1] = BinTree.kEmptyHashValue;
                        break;
                    }

                    uint delta = this._pos - curMatch;
                    uint cyclicPos = (delta <= this._cyclicBufferPos ? this._cyclicBufferPos - delta : this._cyclicBufferPos - delta + this._cyclicBufferSize) << 1;
                    uint pby1 = this._bufferOffset + curMatch;
                    uint len = Math.Min(len0, len1);
                    if (this._bufferBase[pby1 + len] == this._bufferBase[cur + len])
                    {
                        while (++len != lenLimit)
                        {
                            if (this._bufferBase[pby1 + len] != this._bufferBase[cur + len])
                            {
                                break;
                            }
                        }

                        if (len == lenLimit)
                        {
                            this._son[ptr1] = this._son[cyclicPos];
                            this._son[ptr0] = this._son[cyclicPos + 1];
                            break;
                        }
                    }

                    if (this._bufferBase[pby1 + len] < this._bufferBase[cur + len])
                    {
                        this._son[ptr1] = curMatch;
                        ptr1 = cyclicPos + 1;
                        curMatch = this._son[ptr1];
                        len1 = len;
                    }
                    else
                    {
                        this._son[ptr0] = curMatch;
                        ptr0 = cyclicPos;
                        curMatch = this._son[ptr0];
                        len0 = len;
                    }
                }

                this.MovePos();
            }
            while (--num != 0);
        }

        private void Normalize()
        {
            uint subValue = this._pos - this._cyclicBufferSize;
            this.NormalizeLinks(this._son, this._cyclicBufferSize * 2, subValue);
            this.NormalizeLinks(this._hash, this._hashSizeSum, subValue);
            this.ReduceOffsets((Int32)subValue);
        }

        private void NormalizeLinks(uint[] items, uint numItems, uint subValue)
        {
            for (uint i = 0; i < numItems; i++)
            {
                uint value = items[i];
                if (value <= subValue)
                {
                    value = BinTree.kEmptyHashValue;
                }
                else
                {
                    value -= subValue;
                }

                items[i] = value;
            }
        }
    }
}