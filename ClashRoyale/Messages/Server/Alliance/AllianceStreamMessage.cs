namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance.Stream;

    public class AllianceStreamMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24719;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        public StreamEntry[] Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Entries">The entries.</param>
        public AllianceStreamMessage(StreamEntry[] Entries)
        {
            this.Entries = Entries;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.Entries.Length);

            for (int I = 0; I < this.Entries.Length; I++)
            {
                this.Stream.WriteVInt(this.Entries[I].Type);
                this.Entries[I].Encode(this.Stream);
            }
        }
    }
}