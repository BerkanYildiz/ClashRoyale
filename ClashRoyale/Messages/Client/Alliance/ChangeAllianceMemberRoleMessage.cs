namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class ChangeAllianceMemberRoleMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14306;
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

        public long MemberId;
        public int NewRole;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAllianceMemberRoleMessage"/> class.
        /// </summary>
        public ChangeAllianceMemberRoleMessage()
        {
            // ChangeAllianceMemberRoleMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAllianceMemberRoleMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public ChangeAllianceMemberRoleMessage(ByteStream Stream) : base(Stream)
        {
            // ChangeAllianceMemberRoleMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.MemberId   = this.Stream.ReadLong();
            this.NewRole    = this.Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteLong(this.MemberId);
            this.Stream.WriteVInt(this.NewRole);
        }
    }
}