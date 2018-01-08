namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Inbox;

    public class InboxListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24445;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        public InboxEntry[] Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxListMessage"/> class.
        /// </summary>
        public InboxListMessage()
        {
            // InboxListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxListMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public InboxListMessage(ByteStream Stream) : base(Stream)
        {
            // InboxListMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboxListMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        public InboxListMessage(params InboxEntry[] Entries)
        {
            this.Entries = Entries;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Entries = new InboxEntry[this.Stream.ReadVInt()];

            for (int i = 0; i < this.Entries.Length; i++)
            {
                InboxEntry Entry = new InboxEntry();
                Entry.Decode(this.Stream);
                this.Entries[i] = Entry;
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Entries.Length);

            foreach (InboxEntry Entry in this.Entries)
            {
                Entry.Encode(this.Stream);
            }
        }
    }
}