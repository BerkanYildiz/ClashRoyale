namespace ClashRoyale.Patcher
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class Program
    {
        internal static Gamefiles Gamefiles;
        internal static Version Version;

        internal static string SHA = string.Empty;

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        private static string Checksum
        {
            get
            {
                long Time = (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                using (MemoryStream Stream = new MemoryStream(BitConverter.GetBytes(Time)))
                {
                    using (SHA1Managed SHA1 = new SHA1Managed())
                    {
                        byte[] Hash = SHA1.ComputeHash(Stream);

                        StringBuilder String = new StringBuilder(2 * Hash.Length);

                        foreach (byte Byte in Hash)
                        {
                            String.AppendFormat("{0:X2}", Byte);
                        }

                        return String.ToString().ToLower();
                    }
                }
            }
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        private static void Main()
        {
            Program.SHA         = Program.Checksum;

            Program.Version     = new Version();
            Program.Gamefiles   = new Gamefiles();

            Console.WriteLine("[*] We stored " + Program.Gamefiles.Files.Count + " files.");
            Console.WriteLine("[*] The output path is \"" + Program.Gamefiles.Output.FullName + "\".");
            Console.WriteLine("[*] The SHA is " + Program.SHA + ".");

            Console.Title       = "GL - Patcher | " + Program.SHA + " | " + Program.Gamefiles.Files.Count + " files | 2017 C";

            foreach (Gamefile Gamefile in Program.Gamefiles.Files)
            {
                Gamefile.Process();
            }

            JObject Json        = new JObject();

            Json.Add("files",   Program.Gamefiles.ToJson);
            Json.Add("sha",     Program.SHA);
            Json.Add("version", Program.Version.ToString);

            File.WriteAllText(Program.Gamefiles.Output.FullName + "\\fingerprint.json", Json.ToString(Formatting.None).Replace(@"\\", @"\"));

            Program.Version.Write();

            Console.WriteLine("[*] Done.");
            System.Threading.Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}