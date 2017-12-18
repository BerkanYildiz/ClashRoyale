namespace ClashRoyale.Proxy
{
    using System.IO;

    internal class Launcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Launcher"/> class.
        /// </summary>
        internal static void Initialize()
        {
            Launcher.Folders();
            Launcher.Logs();
        }

        /// <summary>
        /// Checks the folders.
        /// </summary>
        internal static void Folders()
        {
            if (!Directory.Exists("Library"))
            {
                Directory.CreateDirectory("Library");
            }

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }

        /// <summary>
        /// Checks the logs.
        /// </summary>
        internal static void Logs()
        {
            string[] Directories        = Directory.GetDirectories(Directory.GetCurrentDirectory() + "\\Logs");

            foreach (string Path in Directories)
            {
                DirectoryInfo Directory = new DirectoryInfo(Path);
                FileInfo[] Files        = Directory.GetFiles();

                if (Files.Length > 0)
                {
                    DirectoryInfo SubDir = Directory.CreateSubdirectory(Files[0].CreationTime.ToString("dd-MM-yy HH-mm-ss"));

                    foreach (FileInfo File in Files)
                    {
                        // File.MoveTo(SubDir.FullName + "\\" + File.Name);
                    }
                }
            }
        }
    }
}