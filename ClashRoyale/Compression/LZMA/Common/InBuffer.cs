namespace ClashRoyale.Compression.LZMA.Common
{
    using System.IO;

    public class InBuffer
    {
        private readonly byte[] MBuffer;

        private readonly uint MBufferSize;

        private uint MLimit;

        private uint MPos;

        private ulong MProcessedSize;

        private Stream MStream;

        private bool MStreamWasExhausted;

        public InBuffer(uint BufferSize)
        {
            this.MBuffer = new byte[BufferSize];
            this.MBufferSize = BufferSize;
        }

        public ulong GetProcessedSize()
        {
            return this.MProcessedSize + this.MPos;
        }

        public void Init(Stream Stream)
        {
            this.MStream = Stream;
            this.MProcessedSize = 0;
            this.MLimit = 0;
            this.MPos = 0;
            this.MStreamWasExhausted = false;
        }

        public bool ReadBlock()
        {
            if (this.MStreamWasExhausted)
            {
                return false;
            }

            this.MProcessedSize += this.MPos;
            int ANumProcessedBytes = this.MStream.Read(this.MBuffer, 0, (int)this.MBufferSize);
            this.MPos = 0;
            this.MLimit = (uint)ANumProcessedBytes;
            this.MStreamWasExhausted = ANumProcessedBytes == 0;
            return !this.MStreamWasExhausted;
        }

        public bool ReadByte(byte B)
        {
            // check it
            if (this.MPos >= this.MLimit)
            {
                if (!this.ReadBlock())
                {
                    return false;
                }
            }

            B = this.MBuffer[this.MPos++];
            return true;
        }

        public byte ReadByte()
        {
            // return (byte)m_Stream.ReadByte();
            if (this.MPos >= this.MLimit)
            {
                if (!this.ReadBlock())
                {
                    return 0xFF;
                }
            }

            return this.MBuffer[this.MPos++];
        }

        public void ReleaseStream()
        {
            // m_Stream.Close();
            this.MStream = null;
        }
    }
}