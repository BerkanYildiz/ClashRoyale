namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Text;
    using System.Threading;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.RoyalTV;
    using ClashRoyale.Logic.RoyalTV.Entry;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class HomeBattleReplayMessage : Message
    {
        internal ArenaData ArenaData;
        internal LogicLong ReplayId;

        internal int ReplayShardId;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14114;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public HomeBattleReplayMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // HomeBattleReplayMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ReplayId = this.Stream.ReadLong();

            if (this.Stream.ReadBoolean())
            {
                this.Stream.ReadLong();
            }

            this.ReplayShardId = this.Stream.ReadVInt();

            this.Stream.ReadVInt();
            this.Stream.ReadVInt();
            this.Stream.ReadVInt();

            this.Stream.ReadBoolean();
            this.Stream.ReadBoolean();
            this.Stream.ReadBoolean();
            this.Stream.ReadBoolean();
            this.Stream.ReadBoolean();

            this.ArenaData = this.Stream.DecodeData<ArenaData>();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        public override async void Process()
        {
            if (!this.ReplayId.IsZero)
            {
                BattleLog BattleLog = await Battles.Get(this.ReplayId.HigherInt, this.ReplayId.LowerInt);

                if (BattleLog != null)
                {
                    int ChannelIdx = RoyalTvManager.GetChannelArenaData(this.ArenaData);

                    if (ChannelIdx != -1)
                    {
                        RoyalTvEntry TvEntry = RoyalTvManager.GetEntryByIdx(ChannelIdx, this.ReplayShardId);

                        if (TvEntry != null)
                        {
                            Interlocked.Increment(ref TvEntry.ViewCount);
                        }
                    }

                    byte[] Decompressed = Encoding.UTF8.GetBytes(BattleLog.ReplayJson);
                    byte[] Compressed   = Encoding.UTF8.GetBytes(BattleLog.ReplayJson);
                    // byte[] Compressed   = ZLibHelper.CompressByteArray(Decompressed);

                    this.Device.NetworkManager.SendMessage(new HomeBattleReplayDataMessage(this.Device, Compressed));
                }
                else
                { 
                    Logging.Info(this.GetType(), "Unable to get the replay id " + this.ReplayId + ".");
                }
            }
            else
            {
                Logging.Info(this.GetType(), "Replay id is empty.");
            }
        }
    }
}