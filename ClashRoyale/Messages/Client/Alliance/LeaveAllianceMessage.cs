namespace ClashRoyale.Messages.Client.Alliance
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Alliance.Stream;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Commands.Server;

    public class LeaveAllianceMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 14308;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaveAllianceMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public LeaveAllianceMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // LeaveAllianceMessage.
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        public override async void Process()
        {
            Logging.Info(this.GetType(), "Player is leaving a clan.");

            if (!this.Device.GameMode.CommandManager.WaitLeaveAllianceTurn)
            {
                if (this.Device.GameMode.Player.IsInAlliance)
                {
                    Clan Clan = await Clans.Get(this.Device.GameMode.Player.ClanHighId, this.Device.GameMode.Player.ClanLowId);

                    if (Clan != null)
                    {
                        if (this.Device.GameMode.Player.AllianceRole == 2)
                        {
                            // Promote.
                        }

                        if (await Clan.Members.TryRemove(this.Device.GameMode.Player))
                        {
                            this.Device.GameMode.CommandManager.WaitLeaveAllianceTurn = true;
                            this.Device.GameMode.CommandManager.AddAvailableServerCommand(new LeaveAllianceCommand(this.Device.GameMode.Player.AllianceId, false));
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "Player tried to leave a clan but TryRemove(Player) returned false.");
                        }

                        var Entry = new AllianceEventStreamEntry(this.Device.GameMode.Player, this.Device.GameMode.Player);
                        Entry.SetLeaveEvent();

                        Clan.Messages.AddEntry(Entry);
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Player tried to leave a clan but database returned a null value.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Player tried to leave a clan but is not in a clan.");
                }
            }
            else
            {
                Logging.Info(this.GetType(), "Player tried to leave a clan but is already leaving it.");
            }
        }
    }
}