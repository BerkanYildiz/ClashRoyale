namespace ClashRoyale.Server.Logic.Stream
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;

    using Newtonsoft.Json.Linq;

    internal class AllianceEventStreamEntry : StreamEntry
    {
        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 4;
            }
        }

        private string TargetName;

        private int TargetHighId;
        private int TargetLowId;

        private int EventType;

        /*
            DEFAULT             = 0,
            KICK_MEMBER         = 1,
            ACCEPT_MEMBER       = 2,
            JOIN_ALLIANCE       = 3,
            LEAVE_ALLIANCE      = 4,
            PROMOTE_MEMBER      = 5,
            DEPROMOTE_MEMBER    = 6
        */

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceEventStreamEntry"/> class.
        /// </summary>
        public AllianceEventStreamEntry()
        {
            // AllianceEventStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceEventStreamEntry"/> class.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Target">The target.</param>
        public AllianceEventStreamEntry(Player Sender, Player Target) : base(Sender)
        {
            this.TargetHighId   = Target.HighId;
            this.TargetLowId    = Target.LowId;
            this.TargetName     = Target.Name;
        }

        /// <summary>
        /// Sets the kick event.
        /// </summary>
        internal void SetKickEvent()
        {
            this.EventType = 1;
        }

        /// <summary>
        /// Sets the accept event.
        /// </summary>
        internal void SetAcceptEvent()
        {
            this.EventType = 2;
        }
        
        /// <summary>
        /// Sets the join event.
        /// </summary>
        internal void SetJoinEvent()
        {
            this.EventType = 3;
        }
        
        /// <summary>
        /// Sets the leave event.
        /// </summary>
        internal void SetLeaveEvent()
        {
            this.EventType = 4;
        }

        /// <summary>
        /// Sets the promotion event.
        /// </summary>
        internal void SetPromoteEvent()
        {
            this.EventType = 5;
        }

        /// <summary>
        /// Sets the demote event.
        /// </summary>
        internal void SetDemoteEvent()
        {
            this.EventType = 6;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ByteStream Stream)
        {
            base.Encode(Stream);

            Stream.WriteVInt(this.EventType);
            Stream.WriteVInt(this.TargetHighId);
            Stream.WriteVInt(this.TargetLowId);
            Stream.WriteString(this.TargetName);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal override void Load(JToken JToken)
        {
            base.Load(JToken);

            JsonHelper.GetJsonNumber(JToken, "eventType", out this.EventType);
            JsonHelper.GetJsonNumber(JToken, "targetHighID", out this.TargetHighId);
            JsonHelper.GetJsonNumber(JToken, "targetLowID", out this.TargetLowId);
            JsonHelper.GetJsonString(JToken, "targetName", out this.TargetName);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("eventType", this.EventType);
            Json.Add("targetHighID", this.TargetHighId);
            Json.Add("targetLowID", this.TargetLowId);
            Json.Add("targetName", this.TargetName);

            return Json;
        }
    }
}
 