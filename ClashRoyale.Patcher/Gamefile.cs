namespace ClashRoyale.Patcher
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    using ClashRoyale.Compression.Lzma;
    using ClashRoyale.Compression.Lzma.Compress.LZMA;

    using Newtonsoft.Json.Linq;

    internal class Gamefile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gamefile"/> class.
        /// </summary>
        internal Gamefile()
        {
            // Gamefile.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamefile"/> class.
        /// </summary>
        /// <param name="Input">The input.</param>
        internal Gamefile(FileInfo Input)
        {
            this.Input  = Input;
            this.Folder = Directory.CreateDirectory(Input.DirectoryName.Replace("\\Gamefiles\\", "\\Patchs\\" + Program.SHA + "\\"));

            this.Output = new FileInfo(this.Folder.FullName + "\\" + Input.Name);
            this.Buffer = File.ReadAllBytes(Input.FullName);
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        internal FileInfo Input
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        private FileInfo Output
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the folder.
        /// </summary>
        internal DirectoryInfo Folder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the buffer.
        /// </summary>
        internal byte[] Buffer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Gamefile"/> is finished.
        /// </summary>
        internal bool Finished
        {
            get;
            set;
        }

        internal JObject ToJson
        {
            get
            {
                JObject JObject = new JObject();

                string Path = this.Output.FullName.Replace(Program.Gamefiles.Output.FullName, string.Empty).Replace("\\", "\\/");

                if (this.Output.Directory.Name == "Gamefiles")
                {
                    Path = this.Output.Name;
                }

                if (this.Output.Name.Contains("highres"))
                {
                    JObject.Add("defer", true);
                }

                JObject.Add("file", Path);
                JObject.Add("sha", this.Checksum);

                return JObject;
            }
        }

        internal string Checksum
        {
            get
            {
                using (var SHA = new SHA1CryptoServiceProvider())
                {
                    using (FileStream Stream = this.Output.OpenRead())
                    {
                        return BitConverter.ToString(SHA.ComputeHash(Stream)).Replace("-", string.Empty).ToLower();
                    }
                }
            }
        }

        /// <summary>
        /// Compresses this instance.
        /// </summary>
        internal void Process()
        {
            Console.WriteLine("[*] Processing " + this.Input.Name + "..");

            if (this.Input.Extension == ".csv" || this.Input.Extension == ".sc")
            {
                if (this.Input.Extension == ".sc")
                {
                    using (MemoryStream Stream = new MemoryStream(this.Buffer))
                    {
                        using (BinaryReader Reader = new BinaryReader(Stream))
                        {
                            char[] Prefix = Reader.ReadChars(2);

                            if (string.Join("", Prefix) == "SC")
                            {
                                Console.WriteLine("[*] Skipping compression of " + this.Output.Name + this.Output.Extension);
                                this.Output = this.Input.CopyTo(this.Output.FullName, true);
                                return;
                            }
                        }
                    }
                }

                Encoder Compresser = new Encoder();

                using (MemoryStream IStream = new MemoryStream(this.Buffer))
                {
                    using (FileStream OStream = this.Output.Create())
                    {
                        CoderPropId[] CoderPropIDs = new CoderPropId[8]
                        {
                            CoderPropId.DictionarySize, CoderPropId.PosStateBits,   CoderPropId.LitContextBits,     CoderPropId.LitPosBits,
                            CoderPropId.Algorithm,      CoderPropId.NumFastBytes,   CoderPropId.MatchFinder,        CoderPropId.EndMarker
                        };

                        object[] Properties = new object[8]
                        {
                            262144, 2, 3, 0, 2, 32, "bt4", false
                        };

                        Compresser.SetCoderProperties(CoderPropIDs, Properties);
                        Compresser.WriteCoderProperties(OStream);

                        OStream.Write(BitConverter.GetBytes(IStream.Length), 0, 4);

                        Compresser.Code(IStream, OStream, IStream.Length, -1L, null);
                    }

                    if (this.Input.Extension == ".sc")
                    {
                        int Version     = 1;
                        byte[] Header   = new byte[26];

                        using (BinaryWriter Writer = new BinaryWriter(new MemoryStream(Header)))
                        {
                            Writer.Write("SC".ToCharArray());
                            Writer.Write(BitConverter.GetBytes(Version).Reverse().ToArray());

                            using (var MD5Hash = MD5.Create())
                            {
                                byte[] MD5 = MD5Hash.ComputeHash(this.Buffer);

                                Writer.Write(BitConverter.GetBytes(MD5.Length).Reverse().ToArray());
                                Writer.Write(MD5);
                            }
                        }

                        byte[] OldData = File.ReadAllBytes(this.Output.FullName);
                        byte[] NewData = Header.Concat(OldData).ToArray();

                        this.Output.Delete();

                        using (FileStream OStream = this.Output.Create())
                        {
                            OStream.Write(NewData, 0, NewData.Length);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    this.Output = this.Input.CopyTo(this.Output.FullName, true);
                }
                catch (Exception)
                {
                    Console.WriteLine("[*] Couldn't copy " + this.Output.Name + " to the patch folder, please do it manualy.");
                }
            }
        }
    }
}