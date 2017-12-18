namespace ClashRoyale.Server.Logic.Alliance.Stream
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Server.Logic.Player;

    using Newtonsoft.Json.Linq;

    internal class JoinRequestAllianceStreamEntry : StreamEntry
    {
        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 3;
            }
        }

        private string SenderMessage;
        private string ResponderName;

        private int RequestState;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinRequestAllianceStreamEntry"/> class.
        /// </summary>
        public JoinRequestAllianceStreamEntry()
        {
            // JoinRequestAllianceStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinRequestAllianceStreamEntry"/> class.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Message">The message.</param>
        public JoinRequestAllianceStreamEntry(Player Sender, string Message) : base(Sender)
        {
            this.SenderMessage  = Message;
            this.RequestState   = 1;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ByteStream Stream)
        {
            base.Encode(Stream);

            Stream.WriteString(this.SenderMessage);
            Stream.WriteString(this.ResponderName);
            Stream.WriteVInt(this.RequestState);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal override void Load(JToken JToken)
        {
            base.Load(JToken);

            JsonHelper.GetJsonString(JToken, "sender_message", out this.SenderMessage);
            JsonHelper.GetJsonString(JToken, "responder_name", out this.ResponderName);
            JsonHelper.GetJsonNumber(JToken, "request_state", out this.RequestState);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("sender_message", this.SenderMessage);
            Json.Add("responder_name", this.ResponderName);
            Json.Add("request_state", this.RequestState);

            return Json;
        }

        /// <summary>
        /// Refuses the request.
        /// </summary>
        /// <param name="ResponderName">Name of the responder.</param>
        internal void RefuseRequest(string ResponderName)
        {
            if (this.RequestState == 1)
            {
                this.RequestState   = 3;
                this.ResponderName  = ResponderName;
            }
        }

        /// <summary>
        /// Accepts the request.
        /// </summary>
        /// <param name="ResponderName">Name of the responder.</param>
        internal void AcceptRequest(string ResponderName)
        {
            if (this.RequestState == 1)
            {
                this.RequestState   = 2;
                this.ResponderName  = ResponderName;
            }
        }
    }
}