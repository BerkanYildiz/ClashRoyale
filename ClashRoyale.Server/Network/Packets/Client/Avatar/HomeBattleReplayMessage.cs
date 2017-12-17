namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Text;
    using System.Threading;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Battle;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Math;
    using ClashRoyale.Server.Logic.RoyalTV;
    using ClashRoyale.Server.Logic.RoyalTV.Entry;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class HomeBattleReplayMessage : Message
    {
        internal ArenaData ArenaData;
        internal LogicLong ReplayId;

        internal int ReplayShardId;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14114;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
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
        internal override void Decode()
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
        internal override async void Process()
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