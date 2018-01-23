namespace ClashRoyale.Files.Sc
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public static class ScFiles
    {
        private static List<ScTexture> Textures;
        private static List<ScInfo> Infos;

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            string[] Files = Directory.GetFiles("Gamefiles/sc/", "*.sc");

            ScFiles.Textures = new List<ScTexture>(Files.Length);
            ScFiles.Infos = new List<ScInfo>(Files.Length);

            foreach (string FilePath in Files)
            {
                FileInfo File = new FileInfo(FilePath);

                if (FilePath.EndsWith("_tex.sc"))
                {
                    ScFiles.Textures.Add(new ScTexture(File));
                }
                else
                {
                    ScFiles.Infos.Add(new ScInfo(File));
                }
            }

            Task.Run(() =>
            {
                foreach (ScTexture ScTexture in ScFiles.Textures)
                {
                    ScTexture.Read().Wait(2500);
                }

                foreach (ScInfo ScInfo in ScFiles.Infos)
                {
                    ScInfo.Read().Wait(5000);
                }
            });

            Logging.Info(typeof(ScFiles), "Loaded " + Files.Length + " SC files.");
        }

        /// <summary>
        ///     Search and return the correct <see cref="ScFile" /> according to the given <see cref="ScTexture" /> file.
        /// </summary>
        /// <param name="Path">The path.</param>
        public static ScInfo GetScInfoFile(ScTexture ScTexture)
        {
            ScInfo ScFile = ScFiles.Infos.Find(T => ScTexture.ScName == T.ScName);

            if (ScFile != null)
            {
                return ScFile;
            }

            return null;
        }

        /// <summary>
        ///     Search and return the correct <see cref="ScTexture" /> according to the given <see cref="ScInfo" /> file, and the
        ///     specified resolution.
        /// </summary>
        /// <param name="Path">The path.</param>
        public static ScTexture GetScTextureFile(ScInfo ScInfo)
        {
            ScTexture ScFile = ScFiles.Textures.Find(T => ScInfo.ScName == T.ScName);

            if (ScFile != null)
            {
                return ScFile;
            }

            return null;
        }

        /// <summary>
        ///     Search and return the correct <see cref="ScTexture" /> according to the given <see cref="ScInfo" /> file, and the
        ///     specified resolution.
        /// </summary>
        /// <param name="Path">The path.</param>
        /// <param name="HighRes">The resolution.</param>
        public static ScTexture GetScTextureFile(ScInfo ScInfo, bool HighRes)
        {
            ScTexture ScFile = ScFiles.Textures.Find(T => ScInfo.ScName == T.ScName && T.IsHighRes == HighRes);

            if (ScFile != null)
            {
                return ScFile;
            }

            return null;
        }
    }
}