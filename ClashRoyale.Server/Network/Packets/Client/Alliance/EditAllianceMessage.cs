namespace ClashRoyale.Server.Network.Packets.Client.Alliance
{
    using System.Threading.Tasks;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Entries;
    using ClashRoyale.Server.Logic.Enums;

    internal class EditAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14316;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        private string Description;

        private int HiringType;
        private int RequiredScore;

        private AllianceBadgeData Badge;
        private RegionData Location;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditAllianceMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public EditAllianceMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // EditAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Description    = this.Stream.ReadString();
            this.Badge          = this.Stream.DecodeData<AllianceBadgeData>();
            this.HiringType     = this.Stream.ReadVInt();
            this.RequiredScore  = this.Stream.ReadVInt();
            this.Location       = this.Stream.DecodeData<RegionData>();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override async void Process()
        {
            Logging.Info(this.GetType(), "Player is editing an alliance.");

            if (this.Device.GameMode.Player.IsInAlliance)
            {
                Task<Clan> GetAlliance = Clans.Get(this.Device.GameMode.Player.AllianceHighId, this.Device.GameMode.Player.AllianceLowId);

                // Check the edited values here.

                Clan Clan = await GetAlliance;

                if (Clan != null)
                {
                    AllianceMemberEntry Member;

                    if (Clan.Members.TryGetValue(this.Device.GameMode.Player.PlayerId, out Member))
                    {
                        if (Member.Role == 2 || Member.Role == 4)
                        {
                            Clan.Description        = this.Description;
                            Clan.HeaderEntry.Badge  = this.Badge;
                            Clan.HeaderEntry.Region = this.Location;
                            Clan.HeaderEntry.Type   = this.HiringType;

                            Clan.HeaderEntry.RequiredScore = this.RequiredScore;

                            Logging.Info(this.GetType(), "Player successfully edited the clan desc.");
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "Player tried to edit an alliance but insufficient permissions.");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Player tried to edit an alliance but the member instance was null.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Player tried to edit an alliance but the alliance was null.");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Player tried to edit an alliance but is not in an alliance.");
            }
        }
    }
}