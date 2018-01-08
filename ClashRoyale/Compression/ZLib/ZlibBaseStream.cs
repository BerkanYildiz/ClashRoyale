namespace ClashRoyale.Compression.ZLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    internal enum ZlibStreamFlavor
    {
        ZLIB = 1950,

        DEFLATE = 1951,

        GZIP = 1952
    }

    internal class ZlibBaseStream : Stream
    {
        internal protected ZlibCodec _z; // deferred init... new ZlibCodec();

        internal protected StreamMode _streamMode = StreamMode.Undefined;

        internal protected FlushType _flushMode;

        internal protected ZlibStreamFlavor _flavor;

        internal protected CompressionMode _compressionMode;

        internal protected CompressionLevel _level;

        internal protected bool _leaveOpen;

        internal protected byte[] _workingBuffer;

        internal protected int _bufferSize = ZlibConstants.WorkingBufferSizeDefault;

        internal protected byte[] _buf1 = new byte[1];

        internal protected Stream _stream;

        internal protected CompressionStrategy Strategy = CompressionStrategy.Default;

        // workitem 7159
        private readonly CRC32 crc;

        internal protected string _GzipFileName;

        internal protected string _GzipComment;

        internal protected DateTime _GzipMtime;

        internal protected int _gzipHeaderByteCount;

        internal int Crc32
        {
            get
            {
                if (this.crc == null)
                {
                    return 0;
                }

                return this.crc.Crc32Result;
            }
        }

        public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen)
        {
            this._flushMode = FlushType.None;

            // this._workingBuffer = new byte[WORKING_BUFFER_SIZE_DEFAULT];
            this._stream = stream;
            this._leaveOpen = leaveOpen;
            this._compressionMode = compressionMode;
            this._flavor = flavor;
            this._level = level;

            // workitem 7159
            if (flavor == ZlibStreamFlavor.GZIP)
            {
                this.crc = new CRC32();
            }
        }

        internal protected bool _wantCompress => this._compressionMode == CompressionMode.Compress;

        private ZlibCodec z
        {
            get
            {
                if (this._z == null)
                {
                    bool wantRfc1950Header = this._flavor == ZlibStreamFlavor.ZLIB;
                    this._z = new ZlibCodec();
                    if (this._compressionMode == CompressionMode.Decompress)
                    {
                        this._z.InitializeInflate(wantRfc1950Header);
                    }
                    else
                    {
                        this._z.Strategy = this.Strategy;
                        this._z.InitializeDeflate(this._level, wantRfc1950Header);
                    }
                }

                return this._z;
            }
        }

        private byte[] workingBuffer
        {
            get
            {
                if (this._workingBuffer == null)
                {
                    this._workingBuffer = new byte[this._bufferSize];
                }

                return this._workingBuffer;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // workitem 7159 calculate the CRC on the unccompressed data (before writing)
            if (this.crc != null)
            {
                this.crc.SlurpBlock(buffer, offset, count);
            }

            if (this._streamMode == StreamMode.Undefined)
            {
                this._streamMode = StreamMode.Writer;
            }
            else if (this._streamMode != StreamMode.Writer)
            {
                throw new ZlibException("Cannot Write after Reading.");
            }

            if (count == 0)
            {
                return;
            }

            // first reference of z property will initialize the private var _z
            this.z.InputBuffer = buffer;
            this._z.NextIn = offset;
            this._z.AvailableBytesIn = count;
            bool done = false;
            do
            {
                this._z.OutputBuffer = this.workingBuffer;
                this._z.NextOut = 0;
                this._z.AvailableBytesOut = this._workingBuffer.Length;
                int rc = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
                if (rc != ZlibConstants.Z_OK && rc != ZlibConstants.Z_STREAM_END)
                {
                    throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
                }

                // if (_workingBuffer.Length - _z.AvailableBytesOut > 0)
                this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                done = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;

                // If GZIP and de-compress, we're done when 8 bytes remain.
                if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
                {
                    done = this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0;
                }
            }
            while (!done);
        }

        private void finish()
        {
            if (this._z == null)
            {
                return;
            }

            if (this._streamMode == StreamMode.Writer)
            {
                bool done = false;
                do
                {
                    this._z.OutputBuffer = this.workingBuffer;
                    this._z.NextOut = 0;
                    this._z.AvailableBytesOut = this._workingBuffer.Length;
                    int rc = this._wantCompress ? this._z.Deflate(FlushType.Finish) : this._z.Inflate(FlushType.Finish);
                    if (rc != ZlibConstants.Z_STREAM_END && rc != ZlibConstants.Z_OK)
                    {
                        string verb = (this._wantCompress ? "de" : "in") + "flating";
                        if (this._z.Message == null)
                        {
                            throw new ZlibException(string.Format("{0}: (rc = {1})", verb, rc));
                        }

                        throw new ZlibException(verb + ": " + this._z.Message);
                    }

                    if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
                    {
                        this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
                    }

                    done = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;

                    // If GZIP and de-compress, we're done when 8 bytes remain.
                    if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
                    {
                        done = this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0;
                    }
                }
                while (!done);

                this.Flush();

                // workitem 7159
                if (this._flavor == ZlibStreamFlavor.GZIP)
                {
                    if (this._wantCompress)
                    {
                        // Emit the GZIP trailer: CRC32 and size mod 2^32
                        int c1 = this.crc.Crc32Result;
                        this._stream.Write(BitConverter.GetBytes(c1), 0, 4);
                        int c2 = (int)(this.crc.TotalBytesRead & 0x00000000FFFFFFFF);
                        this._stream.Write(BitConverter.GetBytes(c2), 0, 4);
                    }
                    else
                    {
                        throw new ZlibException("Writing with decompression is not supported.");
                    }
                }
            }

            // workitem 7159
            else if (this._streamMode == StreamMode.Reader)
            {
                if (this._flavor == ZlibStreamFlavor.GZIP)
                {
                    if (!this._wantCompress)
                    {
                        // workitem 8501: handle edge case (decompress empty stream)
                        if (this._z.TotalBytesOut == 0L)
                        {
                            return;
                        }

                        // Read and potentially verify the GZIP trailer: CRC32 and size mod 2^32
                        byte[] trailer = new byte[8];

                        // workitems 8679 & 12554
                        if (this._z.AvailableBytesIn < 8)
                        {
                            // Make sure we have read to the end of the stream
                            Array.Copy(this._z.InputBuffer, this._z.NextIn, trailer, 0, this._z.AvailableBytesIn);
                            int bytesNeeded = 8 - this._z.AvailableBytesIn;
                            int bytesRead = this._stream.Read(trailer, this._z.AvailableBytesIn, bytesNeeded);
                            if (bytesNeeded != bytesRead)
                            {
                                throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this._z.AvailableBytesIn + bytesRead));
                            }
                        }
                        else
                        {
                            Array.Copy(this._z.InputBuffer, this._z.NextIn, trailer, 0, trailer.Length);
                        }

                        int crc32_expected = BitConverter.ToInt32(trailer, 0);
                        int crc32_actual = this.crc.Crc32Result;
                        int isize_expected = BitConverter.ToInt32(trailer, 4);
                        int isize_actual = (int)(this._z.TotalBytesOut & 0x00000000FFFFFFFF);
                        if (crc32_actual != crc32_expected)
                        {
                            throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32_actual, crc32_expected));
                        }

                        if (isize_actual != isize_expected)
                        {
                            throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", isize_actual, isize_expected));
                        }
                    }
                    else
                    {
                        throw new ZlibException("Reading with compression is not supported.");
                    }
                }
            }
        }

        private void end()
        {
            if (this.z == null)
            {
                return;
            }

            if (this._wantCompress)
            {
                this._z.EndDeflate();
            }
            else
            {
                this._z.EndInflate();
            }

            this._z = null;
        }

        public override void Close()
        {
            if (this._stream == null)
            {
                return;
            }

            try
            {
                this.finish();
            }
            finally
            {
                this.end();
                if (!this._leaveOpen)
                {
                    this._stream.Close();
                }

                this._stream = null;
            }
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();

            // _outStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value);
        }

#if NOT
        public int Read()
        {
            if (Read(_buf1, 0, 1) == 0)
                return 0;

            // calculate CRC after reading
            if (crc!=null)
                crc.SlurpBlock(_buf1, 0, 1);
            return (_buf1[0] & 0xFF);
        }
#endif

        private bool nomoreinput;

        private string ReadZeroTerminatedString()
        {
            List<byte> list = new List<byte>();
            bool done = false;
            do
            {
                // workitem 7740
                int n = this._stream.Read(this._buf1, 0, 1);
                if (n != 1)
                {
                    throw new ZlibException("Unexpected EOF reading GZIP header.");
                }

                if (this._buf1[0] == 0)
                {
                    done = true;
                }
                else
                {
                    list.Add(this._buf1[0]);
                }
            }
            while (!done);

            byte[] a = list.ToArray();
            return GZipStream.iso8859dash1.GetString(a, 0, a.Length);
        }

        private int _ReadAndValidateGzipHeader()
        {
            int totalBytesRead = 0;

            // read the header on the first read
            byte[] header = new byte[10];
            int n = this._stream.Read(header, 0, header.Length);

            // workitem 8501: handle edge case (decompress empty stream)
            if (n == 0)
            {
                return 0;
            }

            if (n != 10)
            {
                throw new ZlibException("Not a valid GZIP stream.");
            }

            if (header[0] != 0x1F || header[1] != 0x8B || header[2] != 8)
            {
                throw new ZlibException("Bad GZIP header.");
            }

            int timet = BitConverter.ToInt32(header, 4);
            this._GzipMtime = GZipStream._unixEpoch.AddSeconds(timet);
            totalBytesRead += n;
            if ((header[3] & 0x04) == 0x04)
            {
                // read and discard extra field
                n = this._stream.Read(header, 0, 2); // 2-byte length field
                totalBytesRead += n;
                short extraLength = (short)(header[0] + header[1] * 256);
                byte[] extra = new byte[extraLength];
                n = this._stream.Read(extra, 0, extra.Length);
                if (n != extraLength)
                {
                    throw new ZlibException("Unexpected end-of-file reading GZIP header.");
                }

                totalBytesRead += n;
            }

            if ((header[3] & 0x08) == 0x08)
            {
                this._GzipFileName = this.ReadZeroTerminatedString();
            }

            if ((header[3] & 0x10) == 0x010)
            {
                this._GzipComment = this.ReadZeroTerminatedString();
            }

            if ((header[3] & 0x02) == 0x02)
            {
                this.Read(this._buf1, 0, 1); // CRC16, ignore
            }

            return totalBytesRead;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // According to MS documentation, any implementation of the IO.Stream.Read function must:
            // (a) throw an exception if offset & count reference an invalid part of the buffer, or
            // if count < 0, or if buffer is null (b) return 0 only upon EOF, or if count = 0 (c) if
            // not EOF, then return at least 1 byte, up to <count> bytes
            if (this._streamMode == StreamMode.Undefined)
            {
                if (!this._stream.CanRead)
                {
                    throw new ZlibException("The stream is not readable.");
                }

                // for the first read, set up some controls.
                this._streamMode = StreamMode.Reader;

                // (The first reference to _z goes through the private accessor which may initialize it.)
                this.z.AvailableBytesIn = 0;
                if (this._flavor == ZlibStreamFlavor.GZIP)
                {
                    this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();

                    // workitem 8501: handle edge case (decompress empty stream)
                    if (this._gzipHeaderByteCount == 0)
                    {
                        return 0;
                    }
                }
            }

            if (this._streamMode != StreamMode.Reader)
            {
                throw new ZlibException("Cannot Read after Writing.");
            }

            if (count == 0)
            {
                return 0;
            }

            if (this.nomoreinput && this._wantCompress)
            {
                return 0; // workitem 8557
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (offset < buffer.GetLowerBound(0))
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (offset + count > buffer.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("count");
            }

            int rc = 0;

            // set up the output of the deflate/inflate codec:
            this._z.OutputBuffer = buffer;
            this._z.NextOut = offset;
            this._z.AvailableBytesOut = count;

            // This is necessary in case _workingBuffer has been resized. (new byte[]) (The first
            // reference to _workingBuffer goes through the private accessor which may initialize it.)
            this._z.InputBuffer = this.workingBuffer;
            do
            {
                // need data in _workingBuffer in order to deflate/inflate. Here, we check if we have any.
                if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
                {
                    // No data available, so try to Read data from the captive stream.
                    this._z.NextIn = 0;
                    this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
                    if (this._z.AvailableBytesIn == 0)
                    {
                        this.nomoreinput = true;
                    }
                }

                // we have data in InputBuffer; now compress or decompress as appropriate
                rc = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
                if (this.nomoreinput && rc == ZlibConstants.Z_BUF_ERROR)
                {
                    return 0;
                }

                if (rc != ZlibConstants.Z_OK && rc != ZlibConstants.Z_STREAM_END)
                {
                    throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", this._wantCompress ? "de" : "in", rc, this._z.Message));
                }

                if ((this.nomoreinput || rc == ZlibConstants.Z_STREAM_END) && this._z.AvailableBytesOut == count)
                {
                    break; // nothing more to read
                }
            }

            // while (_z.AvailableBytesOut == count && rc == ZlibConstants.Z_OK);
            while (this._z.AvailableBytesOut > 0 && !this.nomoreinput && rc == ZlibConstants.Z_OK);

            // workitem 8557 is there more room in output?
            if (this._z.AvailableBytesOut > 0)
            {
                if (rc == ZlibConstants.Z_OK && this._z.AvailableBytesIn == 0)
                {
                    // deferred
                }

                // are we completely done reading?
                if (this.nomoreinput)
                {
                    // and in compression?
                    if (this._wantCompress)
                    {
                        // no more input data available; therefore we flush to try to complete the read
                        rc = this._z.Deflate(FlushType.Finish);
                        if (rc != ZlibConstants.Z_OK && rc != ZlibConstants.Z_STREAM_END)
                        {
                            throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", rc, this._z.Message));
                        }
                    }
                }
            }

            rc = count - this._z.AvailableBytesOut;

            // calculate CRC after reading
            if (this.crc != null)
            {
                this.crc.SlurpBlock(buffer, offset, rc);
            }

            return rc;
        }

        public override bool CanRead => this._stream.CanRead;

        public override bool CanSeek => this._stream.CanSeek;

        public override bool CanWrite => this._stream.CanWrite;

        public override long Length => this._stream.Length;

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        internal enum StreamMode
        {
            Writer,

            Reader,

            Undefined
        }

        public static void CompressString(string s, Stream compressor)
        {
            byte[] uncompressed = Encoding.UTF8.GetBytes(s);
            using (compressor)
            {
                compressor.Write(uncompressed, 0, uncompressed.Length);
            }
        }

        public static void CompressBuffer(byte[] b, Stream compressor)
        {
            // workitem 8460
            using (compressor)
            {
                compressor.Write(b, 0, b.Length);
            }
        }

        public static string UncompressString(byte[] compressed, Stream decompressor)
        {
            // workitem 8460
            byte[] working = new byte[1024];
            Encoding encoding = Encoding.UTF8;
            using (MemoryStream output = new MemoryStream())
            {
                using (decompressor)
                {
                    int n;
                    while ((n = decompressor.Read(working, 0, working.Length)) != 0)
                    {
                        output.Write(working, 0, n);
                    }
                }

                // reset to allow read from start
                output.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(output, encoding);
                return sr.ReadToEnd();
            }
        }

        public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
        {
            // workitem 8460
            byte[] working = new byte[1024];
            using (MemoryStream output = new MemoryStream())
            {
                using (decompressor)
                {
                    int n;
                    while ((n = decompressor.Read(working, 0, working.Length)) != 0)
                    {
                        output.Write(working, 0, n);
                    }
                }

                return output.ToArray();
            }
        }
    }
}