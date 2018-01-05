namespace ClashRoyale.Messages.Client.Alliance
{
    using System.Collections.Generic;

    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Alliance.Entries;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages.Server.Alliance;

    public class SearchAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10949;
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

        private string Name;

        private int MinimumPlayers;
        private int MaximumPlayers;
        private int MinimumScore;

        private bool OpenOnly;

        private RegionData Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAllianceMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public SearchAllianceMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // SearchAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Name               = this.Stream.ReadString();
            this.Location           = this.Stream.DecodeData<RegionData>();

            this.MinimumPlayers    = this.Stream.ReadInt();
            this.MaximumPlayers    = this.Stream.ReadInt();
            this.MinimumScore      = this.Stream.ReadInt();

            this.OpenOnly          = this.Stream.ReadBoolean();

            this.Stream.ReadInt();
            this.Stream.ReadInt();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override void Process()
        {
            Logging.Info(this.GetType(), "Player is searching clans.");

            bool FilterPlayers          = (this.MaximumPlayers < 50 || this.MinimumPlayers > 0);
            bool FilterScore            = this.MinimumScore > 0;
            bool FilterName             = !string.IsNullOrEmpty(this.Name);
            bool FilterLocation         = this.Location != null;
            bool FilterOpenOnly         = this.OpenOnly;

            List<Clan> Availables       = Clans.GetAll();

            if (FilterName)
            {
                Availables = Availables.FindAll(Alliance => Alliance.HeaderEntry.Name.StartsWith(this.Name, System.StringComparison.CurrentCultureIgnoreCase));
            }

            if (FilterPlayers)
            {
                Availables = Availables.FindAll(Alliance => Alliance.HeaderEntry.NumberOfMembers >= this.MinimumPlayers && Alliance.HeaderEntry.NumberOfMembers <= this.MaximumPlayers);
            }

            if (FilterScore)
            {
                Availables = Availables.FindAll(Alliance => Alliance.HeaderEntry.Score >= this.MinimumScore);
            }

            if (FilterLocation)
            {
                Availables = Availables.FindAll(Alliance => Alliance.HeaderEntry.Region == this.Location);
            }

            if (FilterOpenOnly)
            {
                Availables = Availables.FindAll(Alliance => Alliance.HeaderEntry.Type == 1);
            }

            List<AllianceHeaderEntry> Alliances = new List<AllianceHeaderEntry>(Availables.Count > 50 ? 50 : Availables.Count);

            foreach (var Alliance in Availables)
            {
                if (XorShift.NextBool())
                {
                    Alliances.Add(Alliance.HeaderEntry);
                }
                else
                {
                    Alliances.Insert(Availables.Count - 1, Alliance.HeaderEntry);
                }
            }

            Logging.Info(this.GetType(), "The searched clans list has been shuffled.");

            this.Device.NetworkManager.SendMessage(new SearchClansDataMessage(this.Device, this.Name, Alliances));
        }
    }
}