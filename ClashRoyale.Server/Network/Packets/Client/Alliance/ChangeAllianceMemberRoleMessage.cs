namespace ClashRoyale.Server.Network.Packets.Client
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Alliance.Entries;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands.Server;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Player;

    internal class ChangeAllianceMemberRoleMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14306;
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

        private long MemberId;
        private int NewRole;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAllianceMemberRoleMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ChangeAllianceMemberRoleMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // ChangeAllianceMemberRoleMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.MemberId   = this.Stream.ReadLong();
            this.NewRole    = this.Stream.ReadVInt();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override async void Process()
        {
            Player Player = this.Device.GameMode.Player;

            Logging.Info(this.GetType(), "Player is promoting or demoting a clan member.");

            if (Player.IsInAlliance)
            {
                Clan Clan = await Clans.Get(Player.ClanHighId, Player.ClanLowId);

                if (Clan != null)
                {
                    Logging.Info(this.GetType(), "Checking if the member is part of the clan.");

                    if (Clan.Members.TryGetValue(this.MemberId, out AllianceMemberEntry Member))
                    {
                        Logging.Info(this.GetType(), "Checking if the member has rights to promote or demote the targetted member.");

                        if (this.HasRights(Member, Player.AllianceRole))
                        {
                            Player MemberPlayer = await Players.Get((int) (this.MemberId >> 32), (int) this.MemberId);

                            if (this.NewRole == 2)
                            {
                                Logging.Warning(this.GetType(), "Member is a leader, this case has not been implemented !"); // TODO : Implement what happen if the leader demotes himself.
                            }
                            
                            Member.SetAllianceRole(this.NewRole);

                            if (MemberPlayer.IsConnected)
                            {
                                MemberPlayer.GameMode.CommandManager.AddAvailableServerCommand(new ChangeAllianceRoleCommand(Player.AllianceId, this.NewRole));
                            }
                            else
                            {
                                MemberPlayer.SetAllianceRole(this.NewRole);
                                await Players.Remove(MemberPlayer);
                            }
                        }
                        else
                        {
                            Logging.Warning(this.GetType(), "Member of the clan had no rights promoting or demoting another member.");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Tried to retrieve the member instance from the clan members list, null value returned.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Tried to retrieve the player's clan from the database, null value returned.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Player is not in a clan, therefore this message shouldn't have been sent.");
            }
        }

        /// <summary>
        /// Gets if the specified member has a lower role than specified role.
        /// </summary>
        internal bool HasRights(AllianceMemberEntry Entry, int PromoterRole)
        {
            if (Entry.HasLowerRole(PromoterRole))
            {
                return this.NewRole != Entry.Role;
            }

            return false;
        }
    }
}