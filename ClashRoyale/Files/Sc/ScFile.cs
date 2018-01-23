namespace ClashRoyale.Files.Sc
{
    using System.IO;
    using System.Threading.Tasks;

    public class ScFile
    {
        public FileInfo File;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScFile" /> class.
        /// </summary>
        public ScFile()
        {
            // ScFile.
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScFile" /> class.
        /// </summary>
        /// <param name="File">The file.</param>
        public ScFile(FileInfo File) : this()
        {
            this.File = File;
        }

        /// <summary>
        ///     Gets the clean name of the current sc file.
        /// </summary>
        public string ScName
        {
            get
            {
                string Name = this.File.Name.Replace(".sc", string.Empty);

                if (this.IsTextureFile)
                {
                    Name = Name.Replace("_tex", string.Empty);

                    if (this.IsMultiRes)
                    {
                        Name = Name.Replace("_highres", string.Empty);
                        Name = Name.Replace("_lowres", string.Empty);
                    }
                }

                return Name;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this sc file is in high resolution.
        /// </summary>
        public bool IsHighRes
        {
            get
            {
                return this.File.Name.Contains("_highres_");
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this sc file is in low resolution.
        /// </summary>
        public bool IsLowRes
        {
            get
            {
                return this.File.Name.Contains("_lowres_");
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this sc file is in multiple resolution.
        /// </summary>
        public bool IsMultiRes
        {
            get
            {
                if (this.IsHighRes || this.IsLowRes)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is a <see cref="ScTexture" />.
        /// </summary>
        public bool IsTextureFile
        {
            get
            {
                return this.GetType() == typeof(ScTexture);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is a <see cref="ScInfo" />.
        /// </summary>
        public bool IsInfoFile
        {
            get
            {
                return this.GetType() == typeof(ScInfo);
            }
        }

        /// <summary>
        ///     Reads this instance.
        /// </summary>
        public virtual Task Read()
        {
            return Task.CompletedTask;
        }
    }
}