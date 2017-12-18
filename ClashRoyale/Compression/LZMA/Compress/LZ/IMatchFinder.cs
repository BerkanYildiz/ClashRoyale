namespace ClashRoyale.Compression.LZMA.Compress.LZ
{
    using System.IO;

    internal interface IInWindowStream
    {
        byte GetIndexByte(int Index);

        uint GetMatchLen(int Index, uint Distance, uint Limit);

        uint GetNumAvailableBytes();

        void Init();

        void ReleaseStream();

        void SetStream(Stream InStream);
    }

    internal interface IMatchFinder : IInWindowStream
    {
        void Create(uint HistorySize, uint KeepAddBufferBefore, uint MatchMaxLen, uint KeepAddBufferAfter);

        uint GetMatches(uint[] Distances);

        void Skip(uint Num);
    }
}