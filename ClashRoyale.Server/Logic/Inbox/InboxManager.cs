namespace ClashRoyale.Server.Logic
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic.Entry;

    internal static class InboxManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InboxManager"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        private static List<InboxEntry> Entries;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (InboxManager.Initialized)
            {
                return;
            }

            InboxManager.Entries    = new List<InboxEntry>();
            InboxEntry Entry        = new InboxEntry();

            Entry.Title = "GobelinLand";

            Entry.Text += "Welcome to GobelinLand's Clash Royale server !\n";
            Entry.Text += "Please keep in mind this server is NOT affiliated with Supercell !\n";
            Entry.Text += "You can help use improve our server by reporting the bugs at GobelinLand.FR !\n";
            Entry.Text += "Berkan, Administrator.";

            Entry.Url = "https://www.gobelinland.fr/?ua=supercell-in-game";
            Entry.ButtonText = "Visit !";
            
            InboxManager.Entries.Add(Entry);
            InboxManager.Initialized = true;
        }

        /// <summary>
        /// Encodes the entries into the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal static void Encode(ByteStream Stream)
        {
            Stream.WriteInt(InboxManager.Entries.Count);

            foreach (InboxEntry Entry in InboxManager.Entries)
            {
                Entry.Encode(Stream);
            }
        }
    }
}