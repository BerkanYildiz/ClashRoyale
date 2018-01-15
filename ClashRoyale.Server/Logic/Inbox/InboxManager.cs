namespace ClashRoyale.Logic.Inbox
{
    using System.Collections.Generic;

    using ClashRoyale.Extensions;

    public static class InboxManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InboxManager"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        public static List<InboxEntry> Entries;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
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
        /// Decodes the entries from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public static void Decode(ByteStream Stream)
        {
            int EntryCount = Stream.ReadVInt();

            for (int i = 0; i < EntryCount; i++)
            {
                InboxEntry Entry = new InboxEntry();
                Entry.Decode(Stream);
                InboxManager.Entries.Add(Entry);
            }
        }

        /// <summary>
        /// Encodes the entries into the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public static void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteInt(InboxManager.Entries.Count);

            foreach (InboxEntry Entry in InboxManager.Entries)
            {
                Entry.Encode(Stream);
            }
        }
    }
}