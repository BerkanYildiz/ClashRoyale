namespace ClashRoyale.Server.Network.Packets.Server.RoyalTv
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.RoyalTV.Entry;

    internal class RoyalTvContentMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 24405;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.RoyalTv;
            }
        }

        private readonly ArenaData ArenaData;
        private readonly List<RoyalTvEntry> MostViewedList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoyalTvContentMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="RoyalTvEntries">The royal tv entries.</param>
        /// <param name="ArenaData">The arena data.</param>
        public RoyalTvContentMessage(Device Device, List<RoyalTvEntry> RoyalTvEntries, ArenaData ArenaData) : base(Device)
        {
            this.ArenaData      = ArenaData;
            this.MostViewedList = RoyalTvEntries;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode()
        {
            if (this.MostViewedList != null)
            {
                this.Stream.WriteVInt(this.MostViewedList.Count);

                this.MostViewedList.ForEach(Viewed =>
                {
                    Viewed.Encode(this.Stream);
                });
            }
            else
            {
                this.Stream.WriteVInt(-1);
            }

            this.Stream.EncodeData(this.ArenaData);
        }
    }
}