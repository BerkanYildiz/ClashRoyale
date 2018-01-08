namespace ClashRoyale.Compression.Lzma.Common
{
    using System.IO;

    public class OutBuffer
    {
        private readonly byte[] m_Buffer;

        private readonly uint m_BufferSize;

        private uint m_Pos;

        private ulong m_ProcessedSize;

        private Stream m_Stream;

        public OutBuffer(uint bufferSize)
        {
            this.m_Buffer = new byte[bufferSize];
            this.m_BufferSize = bufferSize;
        }

        public void CloseStream()
        {
            this.m_Stream.Close();
        }

        public void FlushData()
        {
            if (this.m_Pos == 0)
            {
                return;
            }

            this.m_Stream.Write(this.m_Buffer, 0, (int)this.m_Pos);
            this.m_Pos = 0;
        }

        public void FlushStream()
        {
            this.m_Stream.Flush();
        }

        public ulong GetProcessedSize()
        {
            return this.m_ProcessedSize + this.m_Pos;
        }

        public void Init()
        {
            this.m_ProcessedSize = 0;
            this.m_Pos = 0;
        }

        public void ReleaseStream()
        {
            this.m_Stream = null;
        }

        public void SetStream(Stream stream)
        {
            this.m_Stream = stream;
        }

        public void WriteByte(byte b)
        {
            this.m_Buffer[this.m_Pos++] = b;
            if (this.m_Pos >= this.m_BufferSize)
            {
                this.FlushData();
            }
        }
    }
}