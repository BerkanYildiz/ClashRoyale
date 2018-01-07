namespace ClashRoyale.Logic.Alliance.Stream
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json.Linq;

    public class ChatStreamEntry : StreamEntry
    {
        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        public override int Type
        {
            get
            {
                return 2;
            }
        }

        public string Message;

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
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);

            Stream.WriteString(this.Message);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public override void Load(JToken JToken)
        {
            base.Load(JToken);

            JsonHelper.GetJsonString(JToken, "message", out this.Message);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public override JObject Save()
        {
            JObject Json = base.Save();

            Json.Add("message", this.Message);

            return Json;
        }
    }
}