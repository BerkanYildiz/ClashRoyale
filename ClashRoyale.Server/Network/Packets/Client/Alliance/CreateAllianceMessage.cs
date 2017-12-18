namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands.Server;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class CreateAllianceMessage : Message
    {
        internal string AllianceName;
        internal string AllianceDescription;

        internal int AllianceType;
        internal int RequiredScore;

        internal RegionData RegionData;
        internal AllianceBadgeData AllianceBadgeData;

        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14301;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAllianceMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public CreateAllianceMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // CreateAllianceMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.AllianceName       = this.Stream.ReadString();
            this.AllianceDescription= this.Stream.ReadString();
            this.AllianceBadgeData  = this.Stream.DecodeData<AllianceBadgeData>();
            this.AllianceType       = this.Stream.ReadVInt();
            this.RequiredScore      = this.Stream.ReadVInt();
            this.RegionData         = this.Stream.DecodeData<RegionData>();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override async void Process()
        {
            Logging.Info(this.GetType(), "Player is creating a clan.");

            if (this.Device.GameMode.CommandManager.WaitJoinAllianceTurn)
            {
                Logging.Info(this.GetType(), "Player is already joining an alliance.");
            }
            else if (this.Device.GameMode.Player.IsInAlliance)
            {
                Logging.Error(this.GetType(), "Player is already in an alliance.");
            }
            else if (this.Device.GameMode.Player.Gold < Globals.AllianceCreateCost)
            {
                Logging.Error(this.GetType(), "Player don't have enough gold to create a clan.");
            }
            else if (this.CheckValues())
            {
                Logging.Info(this.GetType(), "Player is creating an alliance.");

                Clan Clan = new Clan
                {
                    Description = this.AllianceDescription,
                    HeaderEntry =
                    {
                        Name    = this.AllianceName,
                        Type    = this.AllianceType,
                        Badge   = this.AllianceBadgeData,
                        Region  = this.RegionData,

                        RequiredScore = this.RequiredScore
                    }
                };

                await Clans.Create(Clan);

                if (await Clan.Members.TryAdd(this.Device.GameMode.Player, true))
                {
                    this.Device.NetworkManager.SendMessage(new AllianceDataMessage(this.Device, Clan));
                    this.Device.NetworkManager.SendMessage(new AllianceStreamMessage(this.Device, Clan.Messages.ToArray()));

                    this.Device.GameMode.CommandManager.WaitJoinAllianceTurn = true;
                    this.Device.GameMode.CommandManager.AddAvailableServerCommand(new JoinAllianceCommand(Clan.HighId, Clan.LowId, this.AllianceName, this.AllianceBadgeData, true));
                }
                else
                {
                    Logging.Error(this.GetType(), "Player failed to join a newly created clan, server error !");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Player failed to create an alliance, the values were incorrect.");
            }
        }

        /// <summary>
        /// Checks this instance values.
        /// </summary>
        /// <returns>Returns a boolean indicating whether the values are correct.</returns>
        private bool CheckValues()
        {
            Logging.Info(this.GetType(), "Checking the values..");

            if (!string.IsNullOrEmpty(this.AllianceName))
            {
                this.AllianceName = this.AllianceName.Trim();

                if (this.AllianceName.Length >= 2 && this.AllianceName.Length <= 16)
                {
                    if (!string.IsNullOrEmpty(this.AllianceDescription))
                    {
                        this.AllianceDescription = this.AllianceDescription.Trim();

                        if (this.AllianceDescription.Length > 128)
                        {
                            Logging.Error(this.GetType(), "The clan description length was longer than the limit.");
                            return false;
                        }
                    }

                    if (this.AllianceBadgeData == null)
                    {
                        Logging.Error(this.GetType(), "The clan badge data was null.");
                        return false;
                    }

                    if (this.RegionData == null)
                    {
                        Logging.Error(this.GetType(), "The clan region data was null.");
                        return false;
                    }

                    if (this.AllianceType < 0 && this.AllianceType > 3)
                    {
                        Logging.Error(this.GetType(), "The clan alliance type was out of range.");
                        return false;
                    }

                    return true;
                }
                else
                {
                    Logging.Error(this.GetType(), "The clan name length was outside the limits.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "The clan name was either null or empty at CheckValues().");
            }

            return false;
        }
    }
}