namespace ClashRoyale.Server.Logic.Stream
{
    using ClashRoyale.Server.Extensions;

    using Newtonsoft.Json.Linq;

    internal class DonateStreamEntry : StreamEntry
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DonateStreamEntry"/> class.
        /// </summary>
        public DonateStreamEntry()
        {
            // DonateStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DonateStreamEntry"/> class.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        public DonateStreamEntry(Player Sender) : base(Sender)
        {
            // DonateStreamEntry.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ByteStream Stream)
        {
            base.Encode(Stream);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal override void Load(JToken JToken)
        {
            base.Load(JToken);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal override JObject Save()
        {
            JObject Json = base.Save();
            return Json;
        }
    }
}