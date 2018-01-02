namespace ClashRoyale.Server.Network.Packets.Server
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance.Stream;
    using ClashRoyale.Messages;

    internal class AllianceStreamMessage : Message
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

        private readonly StreamEntry[] Entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Entries">The entries.</param>
        public AllianceStreamMessage(Device Device, StreamEntry[] Entries) : base(Device)
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