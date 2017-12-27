namespace ClashRoyale.Client.Network.Packets.Client
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;

    internal class ChangeAvatarNameMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 19863;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Attack;
            }
        }

        private string _name;
        private int _nameState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameMessage"/> class.
        /// </summary>
        public ChangeAvatarNameMessage(Bot Bot) : base(Bot)
        {
            // ChangeAvatarNameMessage. 
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this._name = this.Stream.ReadString();
            this._nameState = this.Stream.ReadVInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteString(this._name);
            this.Stream.WriteVInt(this._nameState);
        }
    }
}