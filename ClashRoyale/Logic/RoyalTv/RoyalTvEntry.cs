namespace ClashRoyale.Logic.RoyalTv
{
    using System;

    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Battle;

    using Newtonsoft.Json;

    public class RoyalTvEntry
    {
        [JsonProperty("runningID")]     public int RunningId;

        [JsonProperty("replayHighID")]  public int ReplayHighId;
        [JsonProperty("replayLowID")]   public int ReplayLowId;
        [JsonProperty("replayShardID")] public int ReplayShardId;
        [JsonProperty("viewCount")]     public int ViewCount;

        [JsonProperty("creation")]      public DateTime Creation;

        [JsonProperty("battleLog")]     public string BattleLogJson;

        /// <summary>
        /// Gets the age in seconds.
        /// </summary>
        public int AgeSeconds
        {
            get
            {
                return (int) DateTime.UtcNow.Subtract(this.Creation).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the replay id.
        /// </summary>
        public long ReplayId
        {
            get
            {
                return (long) this.ReplayHighId << 32 | (uint) this.ReplayLowId;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoyalTvEntry"/> class.
        /// </summary>
        public RoyalTvEntry()
        {
            this.Creation       = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoyalTvEntry"/> class.
        /// </summary>
        /// <param name="BattleLog">The battle log.</param>
        public RoyalTvEntry(BattleLog BattleLog) : this()
        {
            this.BattleLogJson  = BattleLog.SaveJson().ToString(Formatting.None);
            this.ReplayHighId   = BattleLog.HighId;
            this.ReplayLowId    = BattleLog.LowId;
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
        {
            this.BattleLogJson = Stream.ReadString();

            if (Stream.ReadBoolean())
            {
                // ...
            }

            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();

            this.ViewCount = Stream.ReadVInt();

            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();

            this.RunningId = Stream.ReadVInt();

            if (Stream.ReadBoolean())
            {
                this.ReplayShardId  = Stream.ReadVInt();
                this.ReplayHighId   = Stream.ReadInt();
                this.ReplayLowId    = Stream.ReadInt();
            }
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteString(this.BattleLogJson);
            Stream.WriteBoolean(true);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(this.ViewCount);
            Stream.WriteVInt(0);
            Stream.WriteVInt(1);
            Stream.WriteVInt(this.AgeSeconds);
            Stream.WriteVInt(this.RunningId);

            Stream.WriteBoolean(this.ReplayId != 0);
            
            if (this.ReplayId != 0)
            {
                Stream.WriteVInt(this.ReplayShardId);
                Stream.WriteLong(this.ReplayId);
            }
        }
    }
}