namespace ClashRoyale.Server.Logic.RoyalTV.Entry
{
    using System;

    using ClashRoyale.Server.Extensions;

    using Newtonsoft.Json;

    internal class RoyalTvEntry
    {
        [JsonProperty("runningID")]     internal int RunningId;

        [JsonProperty("replayHighID")]  internal int ReplayHighId;
        [JsonProperty("replayLowID")]   internal int ReplayLowId;
        [JsonProperty("replayShardID")] internal int ReplayShardId;
        [JsonProperty("viewCount")]     internal int ViewCount;

        [JsonProperty("creation")]      internal DateTime Creation;

        [JsonProperty("battleLog")]     internal string BattleLogJson;

        /// <summary>
        /// Gets the age in seconds.
        /// </summary>
        internal int AgeSeconds
        {
            get
            {
                return (int) DateTime.UtcNow.Subtract(this.Creation).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the replay id.
        /// </summary>
        internal long ReplayId
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
            // RoyalTvEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoyalTvEntry"/> class.
        /// </summary>
        public RoyalTvEntry(BattleLog BattleLog)
        {
            this.BattleLogJson  = BattleLog.SaveJson().ToString(Formatting.None);
            this.ReplayHighId   = BattleLog.HighId;
            this.ReplayLowId    = BattleLog.LowId;

            this.Creation       = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream Stream)
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