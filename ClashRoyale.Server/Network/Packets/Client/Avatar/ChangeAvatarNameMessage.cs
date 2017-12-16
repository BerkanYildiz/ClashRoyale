namespace ClashRoyale.Server.Network.Packets.Client.Avatar
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Commands.Server;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server.Avatar;

    internal class ChangeAvatarNameMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10212;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        private string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public ChangeAvatarNameMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // ChangeAvatarNameMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Name = this.Stream.ReadString();
            this.Stream.ReadVInt();
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override void Process()
        {
            if (!this.Device.GameMode.CommandManager.WaitChangeAvatarNameTurn)
            {
                if (!string.IsNullOrEmpty(this.Name))
                {
                    this.Name = this.Name.Trim();

                    if (this.Name.Length >= 2 && this.Name.Length <= 16)
                    {
                        if (this.Device.GameMode.Player.IsNameSet)
                        {
                            if (this.Device.GameMode.Player.NameChangeState > 1)
                            {
                                return;
                            }

                            this.Device.GameMode.CommandManager.WaitChangeAvatarNameTurn = true;
                            this.Device.GameMode.CommandManager.AddAvailableServerCommand(new ChangeAvatarNameCommand(this.Name, true, 1));
                        }
                        else
                        {
                            this.Device.GameMode.CommandManager.WaitChangeAvatarNameTurn = true;
                            this.Device.GameMode.CommandManager.AddAvailableServerCommand(new ChangeAvatarNameCommand(this.Name, true, 0));
                        }

                    }
                    else
                    {
                        this.Device.NetworkManager.SendMessage(new AvatarNameChangeFailedMessage(this.Device));
                    }
                }
                else
                {
                    this.Device.NetworkManager.SendMessage(new AvatarNameChangeFailedMessage(this.Device));
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "Player is already waiting for a change name.");
            }
        }
    }
}