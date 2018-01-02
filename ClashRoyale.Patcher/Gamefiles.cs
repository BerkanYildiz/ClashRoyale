namespace ClashRoyale.Patcher
{
    using System.Collections.Generic;
    using System.IO;

    using Newtonsoft.Json.Linq;

    internal class Gamefiles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gamefiles"/> class.
        /// </summary>
        internal Gamefiles()
        {
            this.GetDirectories();
            this.GetFiles();
        }

        /// <summary>
        /// Gets or sets the input folder.
        /// </summary>
        internal DirectoryInfo Input
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output folder.
        /// </summary>
        internal DirectoryInfo Output
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the input gamefiles.
        /// </summary>
        internal List<Gamefile> Files
        {
            get;
            set;
        }

        internal JArray ToJson
        {
            get
            {
                JArray JArray = new JArray();

                foreach (Gamefile Gamefile in this.Files)
                {
                    JArray.Add(Gamefile.ToJson);
                }

                return JArray;
            }
        }

        /// <summary>
        /// Gets the directories and store them as variables.
        /// </summary>
        internal void GetDirectories()
        {
            this.Input = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Gamefiles\\");
            this.Output = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Patchs\\" + Program.SHA + "\\");

            if (!this.Output.Exists)
            {
                this.Output.Create();
            }
        }

        /// <summary>
        /// Gets the input files.
        /// </summary>
        internal void GetFiles()
        {
            this.Files = new List<Gamefile>();

            foreach (FileInfo File in this.Input.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (!File.DirectoryName.Contains("disabled"))
                {
                    if (File.Extension == ".json")
                    {
                        if (File.Directory.Name == "Gamefiles")
                        {
                            this.Files.Add(new Gamefile(File));
                        }
                    }
                    else
                    {
                        this.Files.Add(new Gamefile(File));
                    }
                }
            }
        }
    }
}