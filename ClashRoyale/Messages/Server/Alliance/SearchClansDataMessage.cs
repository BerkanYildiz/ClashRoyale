namespace ClashRoyale.Messages.Server.Alliance
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Logic.Alliance.Entries;

    public class SearchClansDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24310;
            }
        }

        /// <summary> 
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        public List<AllianceHeaderEntry> Alliances;
        public string Filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchClansDataMessage"/> class.
        /// </summary>
        /// <param name="Filter">The filter.</param>
        /// <param name="Alliances">The alliances.</param>
        public SearchClansDataMessage(string Filter, List<AllianceHeaderEntry> Alliances)
        {
            this.Filter     = Filter;
            this.Alliances  = Alliances;
        }

        /// <summary>
        /// Encodes this instance;
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Filter);
            this.Stream.WriteVInt(this.Alliances.Count);

            foreach (var Alliance in this.Alliances)
            {
                Alliance.Encode(this.Stream);
            }
        }
    }
}