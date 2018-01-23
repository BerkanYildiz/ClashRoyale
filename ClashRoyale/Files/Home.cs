namespace ClashRoyale.Files
{
    using System.IO;
    using System.Text;
    using Newtonsoft.Json.Linq;

    public static class Home
    {
        public static JObject Json;

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Home" /> has been already initalized.
        /// </summary>
        public static bool Initalized { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Home" /> class.
        /// </summary>
        public static void Initialize()
        {
            if (Home.Initalized)
            {
                return;
            }

            string FileName = "starting_home.json";

            if (Config.IsMaxedServer)
            {
                FileName = "starting_home_max.json";
            }

            if (Directory.Exists("Gamefiles/level/"))
            {
                if (File.Exists("Gamefiles/level/" + FileName))
                {
                    string RawFile = File.ReadAllText("Gamefiles/level/" + FileName, Encoding.UTF8);

                    if (!string.IsNullOrEmpty(RawFile))
                    {
                        Home.Json = JObject.Parse(RawFile);
                    }
                    else
                    {
                        Logging.Error(typeof(Home), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                    }
                }
                else
                {
                    Logging.Error(typeof(Home), "File.Exists(Path) != true at Initialize().");
                }
            }
            else
            {
                Logging.Error(typeof(Home), "Directory.Exists(Path) != true at Initialize().");
            }

            Home.Initalized = true;
        }
    }
}