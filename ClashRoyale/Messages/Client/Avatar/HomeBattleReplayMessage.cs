namespace ClashRoyale.Messages.Client.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Maths;

    public class HomeBattleReplayMessage : Message
    {
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

        public ArenaData ArenaData;
        public LogicLong ReplayId;

        public int ReplayShardId;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayMessage"/> class.
        /// </summary>
        public HomeBattleReplayMessage()
        {
            // HomeBattleReplayMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeBattleReplayMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public HomeBattleReplayMessage(ByteStream Stream) : base(Stream)
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
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteLong(this.ReplayId);

            this.Stream.WriteBoolean(false);

            if (false)
            {
                this.Stream.WriteLong(0);
            }

            this.Stream.WriteVInt(this.ReplayShardId);

            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);
            this.Stream.WriteVInt(0);

            this.Stream.WriteBoolean(false);
            this.Stream.WriteBoolean(false);
            this.Stream.WriteBoolean(false);
            this.Stream.WriteBoolean(false);
            this.Stream.WriteBoolean(false);

            this.Stream.EncodeData(this.ArenaData);
        }
    }
}