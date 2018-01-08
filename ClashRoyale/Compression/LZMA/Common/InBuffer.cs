namespace ClashRoyale.Compression.Lzma.Common
{
    using System.IO;

    public class InBuffer
    {
        private readonly byte[] m_Buffer;

        private readonly uint m_BufferSize;

        private uint m_Limit;

        private uint m_Pos;

        private ulong m_ProcessedSize;

        private Stream m_Stream;

        private bool m_StreamWasExhausted;

        public InBuffer(uint bufferSize)
        {
            this.m_Buffer = new byte[bufferSize];
            this.m_BufferSize = bufferSize;
        }

        public ulong GetProcessedSize()
        {
            return this.m_ProcessedSize + this.m_Pos;
        }

        public void Init(Stream stream)
        {
            this.m_Stream = stream;
            this.m_ProcessedSize = 0;
            this.m_Limit = 0;
            this.m_Pos = 0;
            this.m_StreamWasExhausted = false;
        }

        public bool ReadBlock()
        {
            if (this.m_StreamWasExhausted)
            {
                return false;
            }

            this.m_ProcessedSize += this.m_Pos;
            int aNumProcessedBytes = this.m_Stream.Read(this.m_Buffer, 0, (int)this.m_BufferSize);
            this.m_Pos = 0;
            this.m_Limit = (uint)aNumProcessedBytes;
            this.m_StreamWasExhausted = aNumProcessedBytes == 0;
            return !this.m_StreamWasExhausted;
        }

        public bool ReadByte(byte b)
        {
            // check it
            if (this.m_Pos >= this.m_Limit)
            {
                if (!this.ReadBlock())
                {
                    return false;
                }
            }

            b = this.m_Buffer[this.m_Pos++];
            return true;
        }

        public byte ReadByte()
        {
            // return (byte)m_Stream.ReadByte();
            if (this.m_Pos >= this.m_Limit)
            {
                if (!this.ReadBlock())
                {
                    return 0xFF;
                }
            }

            return this.m_Buffer[this.m_Pos++];
        }

        public void ReleaseStream()
        {
            // m_Stream.Close();
            this.m_Stream = null;
        }
    }
}