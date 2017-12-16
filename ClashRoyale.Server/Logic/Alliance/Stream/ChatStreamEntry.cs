namespace ClashRoyale.Server.Logic.Stream
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;

    using Newtonsoft.Json.Linq;

    internal class ChatStreamEntry : StreamEntry
    {
        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        internal override int Type
        {
            get
            {
                return 2;
            }
        }

        internal string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatStreamEntry"/> class.
        /// </summary>
        public ChatStreamEntry()
        {
            // ChatStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatStreamEntry"/> class.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="Message">The message.</param>
        public ChatStreamEntry(Player Sender, string Message) : base(Sender)
        {
            this.Message = Message;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ByteStream Stream)
        {
            base.Encode(Stream);

            Stream.WriteString(this.Message);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal override void Load(JToken JToken)
        {
            base.Load(JToken);

            JsonHelper.GetJsonString(JToken, "message", out this.Message);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("message", this.Message);

            return Json;
        }
    }
}