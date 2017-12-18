namespace ClashRoyale.Compression.LZMA.Compress.LZ
{
    using System;
    using System.IO;

    public class InWindow
    {
        public uint BlockSize; // Size of Allocated memory block

        public byte[] BufferBase; // pointer to buffer with data

        public uint BufferOffset;

        public uint Pos; // offset (from _buffer) of curent byte

        public uint StreamPos; // offset (from _buffer) of first not read byte from Stream

        private uint KeepSizeAfter; // how many BYTEs must be kept buffer after _pos

        private uint KeepSizeBefore; // how many BYTEs must be kept in buffer before _pos

        private uint PointerToLastSafePosition;

        private uint PosLimit; // offset (from _buffer) of first byte when new block reading must be done

        private Stream Stream;

        private bool StreamEndWasReached; // if (true) then _streamPos shows real end of stream

        public void Create(uint KeepSizeBefore, uint KeepSizeAfter, uint KeepSizeReserv)
        {
            this.KeepSizeBefore = KeepSizeBefore;
            this.KeepSizeAfter = KeepSizeAfter;
            uint BlockSize = KeepSizeBefore + KeepSizeAfter + KeepSizeReserv;
            if (this.BufferBase == null || this.BlockSize != BlockSize)
            {
                this.Free();
                this.BlockSize = BlockSize;
                this.BufferBase = new byte[this.BlockSize];
            }

            this.PointerToLastSafePosition = this.BlockSize - KeepSizeAfter;
        }

        public byte GetIndexByte(int Index)
        {
            return this.BufferBase[this.BufferOffset + this.Pos + Index];
        }

        // index + limit have not to exceed _keepSizeAfter;
        public uint GetMatchLen(int Index, uint Distance, uint Limit)
        {
            if (this.StreamEndWasReached)
            {
                if (this.Pos + Index + Limit > this.StreamPos)
                {
                    Limit = this.StreamPos - (UInt32)(this.Pos + Index);
                }
            }

            Distance++;

            // Byte *pby = _buffer + (size_t)_pos + index;
            uint pby = this.BufferOffset + this.Pos + (UInt32)Index;
            uint i;
            for (i = 0; i < Limit && this.BufferBase[pby + i] == this.BufferBase[pby + i - Distance]; i++)
            {
                ;
            }

            return i;
        }

        public uint GetNumAvailableBytes()
        {
            return this.StreamPos - this.Pos;
        }

        public void Init()
        {
            this.BufferOffset = 0;
            this.Pos = 0;
            this.StreamPos = 0;
            this.StreamEndWasReached = false;
            this.ReadBlock();
        }

        public void MoveBlock()
        {
            uint offset = this.BufferOffset + this.Pos - this.KeepSizeBefore;

            // we need one additional byte, since MovePos moves on 1 byte.
            if (offset > 0)
            {
                offset--;
            }

            uint NumBytes = this.BufferOffset + this.StreamPos - offset;

            // check negative offset ????
            for (uint i = 0; i < NumBytes; i++)
            {
                this.BufferBase[i] = this.BufferBase[offset + i];
            }

            this.BufferOffset -= offset;
        }

        public void MovePos()
        {
            this.Pos++;
            if (this.Pos > this.PosLimit)
            {
                uint PointerToPostion = this.BufferOffset + this.Pos;
                if (PointerToPostion > this.PointerToLastSafePosition)
                {
                    this.MoveBlock();
                }

                this.ReadBlock();
            }
        }

        public virtual void ReadBlock()
        {
            if (this.StreamEndWasReached)
            {
                return;
            }

            while (true)
            {
                int size = (int)(0 - this.BufferOffset + this.BlockSize - this.StreamPos);
                if (size == 0)
                {
                    return;
                }

                int NumReadBytes = this.Stream.Read(this.BufferBase, (int)(this.BufferOffset + this.StreamPos), size);
                if (NumReadBytes == 0)
                {
                    this.PosLimit = this.StreamPos;
                    uint PointerToPostion = this.BufferOffset + this.PosLimit;
                    if (PointerToPostion > this.PointerToLastSafePosition)
                    {
                        this.PosLimit = this.PointerToLastSafePosition - this.BufferOffset;
                    }

                    this.StreamEndWasReached = true;
                    return;
                }

                this.StreamPos += (UInt32)NumReadBytes;
                if (this.StreamPos >= this.Pos + this.KeepSizeAfter)
                {
                    this.PosLimit = this.StreamPos - this.KeepSizeAfter;
                }
            }
        }

        public void ReduceOffsets(int SubValue)
        {
            this.BufferOffset += (UInt32)SubValue;
            this.PosLimit -= (UInt32)SubValue;
            this.Pos -= (UInt32)SubValue;
            this.StreamPos -= (UInt32)SubValue;
        }

        public void ReleaseStream()
        {
            this.Stream = null;
        }

        public void SetStream(Stream Stream)
        {
            this.Stream = Stream;
        }

        private void Free()
        {
            this.BufferBase = null;
        }
    }
}