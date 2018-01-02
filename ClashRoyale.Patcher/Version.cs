namespace ClashRoyale.Patcher
{
    using System;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json.Linq;

    internal class Version
    {
        internal int Major;
        internal int Minor;
        internal int Revision;

        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        internal Version()
        {
            this.Read();
        }

        /// <summary>
        /// Gets the version as a string.
        /// </summary>
        internal string ToString
        {
            get
            {
                return this.Major + "." + this.Minor + "." + this.Revision;
            }
        }

        /// <summary>
        /// Reads the latest patch version.
        /// </summary>
        internal void Read()
        {
            if (!File.Exists(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION"))
            {
                Directory.CreateDirectory("Patchs");

                using (FileStream FS = File.Create(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION"))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + "\\Gamefiles\\fingerprint.json"))
                    {
                        JObject JSON = JObject.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + "\\Gamefiles\\fingerprint.json"));

                        byte[] VersionBuff = Encoding.UTF8.GetBytes((string)JSON["version"]);

                        FS.Write(VersionBuff, 0, VersionBuff.Length);
                    }
                }
            }

            string[] Lines = File.ReadAllLines(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION");

            if (string.IsNullOrEmpty(Lines[0]))
            {
                this.Major = 1;
            }
            else
            {
                string[] TVersion = Lines[0].Split('.');

                this.Major      = int.Parse(TVersion[0]);
                this.Minor      = int.Parse(TVersion[1]);
                this.Revision   = int.Parse(TVersion[2]);

                this.Revision = this.Revision + 1;

                if (this.Revision == 9)
                {
                    this.Revision = 0;
                    this.Minor += 1;
                }

                if (this.Minor == 3000)
                {
                    this.Minor = 2000;
                    this.Revision += 1;
                }
            }
        }

        /// <summary>
        /// Writes the version to the version file.
        /// </summary>
        internal void Write()
        {
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\Patchs\\VERSION", this.ToString + Environment.NewLine + Program.SHA);
        }
    }
}