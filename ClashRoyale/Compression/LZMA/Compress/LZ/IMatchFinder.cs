namespace ClashRoyale.Compression.Lzma.Compress.LZ
{
    using System.IO;

    internal interface IInWindowStream
    {
        byte GetIndexByte(int index);

        uint GetMatchLen(int index, uint distance, uint limit);

        uint GetNumAvailableBytes();

        void Init();

        void ReleaseStream();

        void SetStream(Stream inStream);
    }

    internal interface IMatchFinder : IInWindowStream
    {
        void Create(uint historySize, uint keepAddBufferBefore, uint matchMaxLen, uint keepAddBufferAfter);

        uint GetMatches(uint[] distances);

        void Skip(uint num);
    }
}