namespace ClashRoyale.Compression.LZMA.Compress.LZ
{
    using System.IO;

    public class OutWindow
    {
        public uint TrainSize;

        private byte[] Buffer;

        private uint Pos;

        private Stream Stream;

        private uint StreamPos;

        private uint WindowSize;

        public void CopyBlock(uint Distance, uint Len)
        {
            uint pos = this.Pos - Distance - 1;
            if (pos >= this.WindowSize)
            {
                pos += this.WindowSize;
            }

            for (; Len > 0; Len--)
            {
                if (pos >= this.WindowSize)
                {
                    pos = 0;
                }

                this.Buffer[this.Pos++] = this.Buffer[pos++];
                if (this.Pos >= this.WindowSize)
                {
                    this.Flush();
                }
            }
        }

        public void Create(uint WindowSize)
        {
            if (this.WindowSize != WindowSize)
            {
                // System.GC.Collect();
                this.Buffer = new byte[WindowSize];
            }

            this.WindowSize = WindowSize;
            this.Pos = 0;
            this.StreamPos = 0;
        }

        public void Flush()
        {
            uint size = this.Pos - this.StreamPos;
            if (size == 0)
            {
                return;
            }

            this.Stream.Write(this.Buffer, (int)this.StreamPos, (int)size);
            if (this.Pos >= this.WindowSize)
            {
                this.Pos = 0;
            }

            this.StreamPos = this.Pos;
        }

        public byte GetByte(uint Distance)
        {
            uint pos = this.Pos - Distance - 1;
            if (pos >= this.WindowSize)
            {
                pos += this.WindowSize;
            }

            return this.Buffer[pos];
        }

        public void Init(Stream Stream, bool Solid)
        {
            this.ReleaseStream();
            this.Stream = Stream;
            if (!Solid)
            {
                this.StreamPos = 0;
                this.Pos = 0;
                this.TrainSize = 0;
            }
        }

        public void PutByte(byte B)
        {
            this.Buffer[this.Pos++] = B;
            if (this.Pos >= this.WindowSize)
            {
                this.Flush();
            }
        }

        public void ReleaseStream()
        {
            this.Flush();
            this.Stream = null;
        }

        public bool Train(Stream Stream)
        {
            long len = Stream.Length;
            uint size = len < this.WindowSize ? (uint)len : this.WindowSize;
            this.TrainSize = size;
            Stream.Position = len - size;
            this.StreamPos = this.Pos = 0;
            while (size > 0)
            {
                uint CurSize = this.WindowSize - this.Pos;
                if (size < CurSize)
                {
                    CurSize = size;
                }

                int NumReadBytes = Stream.Read(this.Buffer, (int)this.Pos, (int)CurSize);
                if (NumReadBytes == 0)
                {
                    return false;
                }

                size -= (uint)NumReadBytes;
                this.Pos += (uint)NumReadBytes;
                this.StreamPos += (uint)NumReadBytes;
                if (this.Pos == this.WindowSize)
                {
                    this.StreamPos = this.Pos = 0;
                }
            }

            return true;
        }
    }
}