namespace ClashRoyale.Compression.Lzma
{
    using System;
    using System.IO;

    /// <summary>
    /// The exception that is thrown when an error in input stream occurs during decoding.
    /// </summary>
    internal class DataErrorException : ApplicationException
    {
        public DataErrorException()
            : base("Data Error")
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when the value of an argument is outside the allowable range.
    /// </summary>
    internal class InvalidParamException : ApplicationException
    {
        public InvalidParamException()
            : base("Invalid Parameter")
        {
        }
    }

    public interface ICodeProgress
    {
        /// <summary>
        /// Callback progress.
        /// </summary>
        /// <param name="InSize">input size. -1 if unknown.</param>
        /// <param name="OutSize">output size. -1 if unknown.</param>
        void SetProgress(long InSize, long OutSize);
    }

    public interface ICoder
    {
        /// <summary>
        /// Codes streams.
        /// </summary>
        /// <param name="InStream">input Stream.</param>
        /// <param name="OutStream">output Stream.</param>
        /// <param name="InSize">input Size. -1 if unknown.</param>
        /// <param name="OutSize">output Size. -1 if unknown.</param>
        /// <param name="Progress">callback progress reference.</param>
        /// <exception cref="DataErrorException">if input stream is not valid</exception>
        void Code(Stream InStream, Stream OutStream, long InSize, long OutSize, ICodeProgress Progress);
    }

    /*
	public interface ICoder2
	{
		 void Code(ISequentialInStream []inStreams,
				const UInt64 []inSizes,
				ISequentialOutStream []outStreams,
				UInt64 []outSizes,
				ICodeProgress progress);
	};
  */

    /// <summary>
    /// Provides the fields that represent properties idenitifiers for compressing.
    /// </summary>
    public enum CoderPropId
    {
        /// <summary>
        /// Specifies default property.
        /// </summary>
        DefaultProp = 0,

        /// <summary>
        /// Specifies size of dictionary.
        /// </summary>
        DictionarySize,

        /// <summary>
        /// Specifies size of memory for PPM*.
        /// </summary>
        UsedMemorySize,

        /// <summary>
        /// Specifies order for PPM methods.
        /// </summary>
        Order,

        /// <summary>
        /// Specifies Block Size.
        /// </summary>
        BlockSize,

        /// <summary> Specifies number of postion state bits for LZMA (0 <= x <= 4). </summary>
        PosStateBits,

        /// <summary> Specifies number of literal context bits for LZMA (0 <= x <= 8). </summary>
        LitContextBits,

        /// <summary> Specifies number of literal position bits for LZMA (0 <= x <= 4). </summary>
        LitPosBits,

        /// <summary>
        /// Specifies number of fast bytes for LZ*.
        /// </summary>
        NumFastBytes,

        /// <summary>
        /// Specifies match finder. LZMA: "BT2", "BT4" or "BT4B".
        /// </summary>
        MatchFinder,

        /// <summary>
        /// Specifies the number of match finder cyckes.
        /// </summary>
        MatchFinderCycles,

        /// <summary>
        /// Specifies number of passes.
        /// </summary>
        NumPasses,

        /// <summary>
        /// Specifies number of algorithm.
        /// </summary>
        Algorithm,

        /// <summary>
        /// Specifies the number of threads.
        /// </summary>
        NumThreads,

        /// <summary>
        /// Specifies mode with end marker.
        /// </summary>
        EndMarker
    }

    public interface ISetCoderProperties
    {
        void SetCoderProperties(CoderPropId[] PropIDs, object[] Properties);
    }

    public interface IWriteCoderProperties
    {
        void WriteCoderProperties(Stream OutStream);
    }

    public interface ISetDecoderProperties
    {
        void SetDecoderProperties(byte[] Properties);
    }
}