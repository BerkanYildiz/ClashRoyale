namespace ClashRoyale.Files
{
    using System.IO;
    using Newtonsoft.Json.Linq;

    public static class Fingerprint
    {
        public static JObject Json;

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has been already initialized.
        /// </summary>
        public static bool Initialized { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the patch is custom.
        /// </summary>
        public static bool IsCustom { get; set; }

        /// <summary>
        ///     Gets the masterhash of the patch.
        /// </summary>
        public static string Masterhash
        {
            get
            {
                return Fingerprint.Json["sha"].ToObject<string>();
            }
        }

        /// <summary>
        ///     Gets the version of the patch.
        /// </summary>
        public static string[] Version
        {
            get
            {
                return Fingerprint.Json["version"].ToObject<string>().Split('.');
            }
        }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (Fingerprint.Initialized)
            {
                return;
            }

            if (File.Exists(@"Gamefiles\fingerprint.json"))
            {
                string RawFile = File.ReadAllText(Directory.GetCurrentDirectory() + @"\Gamefiles\fingerprint.json");

                if (!string.IsNullOrEmpty(RawFile))
                {
                    Fingerprint.Json = JObject.Parse(RawFile);
                }
                else
                {
                    Logging.Error(typeof(Fingerprint), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                }
            }
            else
            {
                Logging.Error(typeof(Fingerprint), "File.Exists(Fingerprint) != true at Initialize().");
            }

            string CustomPath = Directory.GetCurrentDirectory() + "\\Patchs\\";

            if (File.Exists(CustomPath + "VERSION"))
            {
                string[] VersionFile = File.ReadAllLines(CustomPath + "VERSION");

                if (!string.IsNullOrEmpty(VersionFile[0]))
                {
                    if (VersionFile.Length > 1 && !string.IsNullOrEmpty(VersionFile[1]))
                    {
                        string Masterhash = VersionFile[1];

                        if (File.Exists(CustomPath + Masterhash + "\\fingerprint.json"))
                        {
                            string RawFile = File.ReadAllText(CustomPath + Masterhash + "\\fingerprint.json");

                            if (!string.IsNullOrEmpty(RawFile))
                            {
                                Fingerprint.Json = JObject.Parse(RawFile);
                                Fingerprint.IsCustom = true;
                            }
                            else
                            {
                                Logging.Error(typeof(Fingerprint), "string.IsNullOrEmpty(RawFile) == true at Initialize().");
                            }
                        }
                        else
                        {
                            Logging.Error(typeof(Fingerprint), "File.Exists(CustomSha) != true at Initialize().");
                        }
                    }
                }
                else
                {
                    Logging.Error(typeof(Fingerprint), "string.IsNullOrEmpty(VersionFile[0]) == true at Initialize().");
                }
            }
            else
            {
                Logging.Error(typeof(Fingerprint), "File.Exists(VersionFile) != true at Initialize().");
            }

            Fingerprint.Initialized = true;
        }
    }
}