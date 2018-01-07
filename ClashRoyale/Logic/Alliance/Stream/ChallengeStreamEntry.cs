namespace ClashRoyale.Logic.Alliance.Stream
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Maths;

    using Newtonsoft.Json.Linq;

    public class ChallengeStreamEntry : StreamEntry
    {
        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        public override int Type
        {
            get
            {
                return 10;
            }
        }

        private LogicLong AcceptingAvatarId;

        private string AcceptingAvatar;
        private string Message;

        private int SpectatorCount;
        private int SenderScore;

        private bool TournamentMode;
        private bool Closed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengeStreamEntry"/> class.
        /// </summary>
        public ChallengeStreamEntry()
        {
            // ChallengeStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChallengeStreamEntry"/> class.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        public ChallengeStreamEntry(Player Sender) : base(Sender)
        {
            // ChallengeStreamEntry.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteString(this.Message);

            Stream.WriteBoolean(this.AcceptingAvatar != null);

            if (this.AcceptingAvatar != null)
            {
                Stream.WriteString(this.AcceptingAvatar);
            }

            Stream.WriteVInt(this.SenderScore);
            Stream.WriteBoolean(this.Closed);
            Stream.WriteBoolean(this.TournamentMode);
            Stream.WriteVInt(this.SpectatorCount);
            Stream.WriteBoolean(this.AcceptingAvatarId != 0);

            if (this.AcceptingAvatarId != 0)
            {
                Stream.WriteLong(this.AcceptingAvatarId);
            }

            Stream.EncodeData(null);

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public override void Load(JToken JToken)
        {
            base.Load(JToken);

            JsonHelper.GetJsonString(JToken, "accepting_avatar", out this.AcceptingAvatar);
            JsonHelper.GetJsonNumber(JToken, "spectator_count", out this.SpectatorCount);
            JsonHelper.GetJsonString(JToken, "message", out this.Message);
            JsonHelper.GetJsonNumber(JToken, "sender_score", out this.SenderScore);
            JsonHelper.GetJsonBoolean(JToken, "tournament_mode", out this.TournamentMode);
            JsonHelper.GetJsonBoolean(JToken, "closed", out this.Closed);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("accepting_avatar", this.AcceptingAvatar);
            Json.Add("spectator_count", this.SpectatorCount);
            Json.Add("message", this.Message);
            Json.Add("sender_score", this.SenderScore);
            Json.Add("tournament_mode", this.TournamentMode);
            Json.Add("closed", this.Closed);

            return Json;
        }
    }
}