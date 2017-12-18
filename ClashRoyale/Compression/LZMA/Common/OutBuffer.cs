namespace ClashRoyale.Compression.LZMA.Common
{
    using System.IO;

    public class OutBuffer
    {
        private readonly byte[] MBuffer;

        private readonly uint MBufferSize;

        private uint MPos;

        private ulong MProcessedSize;

        private Stream MStream;

        public OutBuffer(uint BufferSize)
        {
            this.MBuffer = new byte[BufferSize];
            this.MBufferSize = BufferSize;
        }

        public void CloseStream()
        {
            this.MStream.Close();
        }

        public void FlushData()
        {
            if (this.MPos == 0)
            {
                return;
            }

            this.MStream.Write(this.MBuffer, 0, (int)this.MPos);
            this.MPos = 0;
        }

        public void FlushStream()
        {
            this.MStream.Flush();
        }

        public ulong GetProcessedSize()
        {
            return this.MProcessedSize + this.MPos;
        }

        public void Init()
        {
            this.MProcessedSize = 0;
            this.MPos = 0;
        }

        public void ReleaseStream()
        {
            this.MStream = null;
        }

        public void SetStream(Stream Stream)
        {
            this.MStream = Stream;
        }

        public void WriteByte(byte B)
        {
            this.MBuffer[this.MPos++] = B;
            if (this.MPos >= this.MBufferSize)
            {
                this.FlushData();
            }
        }
    }
}