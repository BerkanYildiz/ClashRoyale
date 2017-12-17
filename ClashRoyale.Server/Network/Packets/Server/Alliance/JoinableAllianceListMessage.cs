namespace ClashRoyale.Server.Network.Packets.Server
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance.Entries;
    using ClashRoyale.Server.Logic.Enums;

    internal class JoinableAllianceListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24304;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private readonly List<AllianceHeaderEntry> Alliances;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinableAllianceListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Alliances">The alliances.</param>
        public JoinableAllianceListMessage(Device Device, List<AllianceHeaderEntry> Alliances) : base(Device)
        {
            this.Alliances = Alliances;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        internal override void Encode()
        {
            this.Stream.WriteVInt(this.Alliances.Count);

            this.Alliances.ForEach(Alliance =>
            {
                Alliance.Encode(this.Stream);
            });
        }
    }
}