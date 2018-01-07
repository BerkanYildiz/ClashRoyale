namespace ClashRoyale.Logic.Alliance.Stream
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json.Linq;

    public class DonateStreamEntry : StreamEntry
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
        public override void Encode(ChecksumEncoder Stream)
        {
            base.Encode(Stream);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        public override void Load(JToken JToken)
        {
            base.Load(JToken);
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        public override JObject Save()
        {
            JObject Json = base.Save();
            return Json;
        }
    }
}