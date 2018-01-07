namespace ClashRoyale.Messages.Server.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;

    public class AllianceDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 26550;
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

        public Clan Clan;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataMessage"/> class.
        /// </summary>
        /// <param name="Clan">The clan.</param>
        public AllianceDataMessage(Clan Clan)
        {
            this.Clan = Clan;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Clan.Encode(this.Stream);
        }
    }
}