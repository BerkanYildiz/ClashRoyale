namespace ClashRoyale.Logic.Alliance.Stream
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json.Linq;

    public class AllianceEventStreamEntry : StreamEntry
    {
        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        public override int Type
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
        public void SetKickEvent()
        {
            this.EventType = 1;
        }

        /// <summary>
        /// Sets the accept event.
        /// </summary>
        public void SetAcceptEvent()
        {
            this.EventType = 2;
        }
        
        /// <summary>
        /// Sets the join event.
        /// </summary>
        public void SetJoinEvent()
        {
            this.EventType = 3;
        }
        
        /// <summary>
        /// Sets the leave event.
        /// </summary>
        public void SetLeaveEvent()
        {
            this.EventType = 4;
        }

        /// <summary>
        /// Sets the promotion event.
        /// </summary>
        public void SetPromoteEvent()
        {
            this.EventType = 5;
        }

        /// <summary>
        /// Sets the demote event.
        /// </summary>
        public void SetDemoteEvent()
        {
            this.EventType = 6;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
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
        public override void Load(JToken JToken)
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
        public override JObject Save()
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
 