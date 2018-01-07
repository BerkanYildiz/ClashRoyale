namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;

    public class AllianceOnlineStatusUpdatedMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24457;
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

        public int MemberOnline;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceOnlineStatusUpdatedMessage"/> class.
        /// </summary>
        /// <param name="MemberOnline">The member online.</param>
        public AllianceOnlineStatusUpdatedMessage(int MemberOnline)
        {
            this.MemberOnline = MemberOnline;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.MemberOnline = this.Stream.ReadVInt();

            for (int i = 0; i < this.Stream.ReadVInt(); i++)
            {
                // TODO.
            }
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteVInt(this.MemberOnline);
            this.Stream.WriteVInt(0); // Array
        }
    }
}