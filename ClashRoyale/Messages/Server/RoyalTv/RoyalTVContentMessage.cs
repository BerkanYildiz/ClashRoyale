namespace ClashRoyale.Messages.Server.RoyalTv
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.RoyalTv;

    public class RoyalTvContentMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 20073;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.RoyalTv;
            }
        }

        public ArenaData ArenaData;
        public List<RoyalTvEntry> MostViewedList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoyalTvContentMessage"/> class.
        /// </summary>
        /// <param name="RoyalTvEntries">The royal tv entries.</param>
        /// <param name="ArenaData">The arena data.</param>
        public RoyalTvContentMessage(List<RoyalTvEntry> RoyalTvEntries, ArenaData ArenaData)
        {
            this.ArenaData      = ArenaData;
            this.MostViewedList = RoyalTvEntries;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
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