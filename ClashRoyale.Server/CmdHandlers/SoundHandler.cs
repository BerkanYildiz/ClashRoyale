namespace ClashRoyale.CmdHandlers
{
    using System;

    using ClashRoyale.Files.Sound;

    internal static class SoundHandler
    {
        private static SoundFile SoundFile;

        /// <summary>
        /// Handles the specified arguments.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Handle(params string[] Args)
        {
            if (Args.Length < 2)
            {
                Console.WriteLine("[*] Missing arguments, please use a valid command.");
            }

            if (Args[1] == "select")
            {
                SoundHandler.Select(Args);
            }
            else if (Args[1] == "play")
            {
                SoundHandler.Play(Args);
            }
        }

        /// <summary>
        /// Selects the specified sound.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Select(params string[] Args)
        {
            if (Args.Length < 3)
            {
                Console.WriteLine("[*] Missing arguments, please use a valid command.");
            }

            SoundHandler.SoundFile = SoundFiles.GetEffectFile(Args[2]);

            if (SoundHandler.SoundFile == null)
            {
                SoundHandler.SoundFile = SoundFiles.GetMusicFile(Args[2]);
            }

            if (SoundHandler.SoundFile != null)
            {
                Console.WriteLine("[*] Selected the specified sound.");
            }
            else
            {
                Console.WriteLine("[*] Invalid arguments, please select an existing sound.");
            }
        }

        /// <summary>
        /// Plays the previously selected sound.
        /// </summary>
        /// <param name="Args">The arguments.</param>
        internal static void Play(params string[] Args)
        {
            if (SoundHandler.SoundFile != null)
            {
                SoundHandler.SoundFile.Play().ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine("[*] Invalid arguments, please select a sound first.");
            }
        }
    }
}
