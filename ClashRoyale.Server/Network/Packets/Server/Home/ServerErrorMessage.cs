namespace ClashRoyale.Server.Network.Packets.Server
{
    using System.Text;

    using ClashRoyale.Enums;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    internal class ServerErrorMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 24115;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        private readonly StringBuilder Reason;
        private readonly string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerErrorMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Initial">if set to <c>true</c> [initial].</param>
        public ServerErrorMessage(Device Device, string Message = "", bool Initial = false) : base(Device)
        {
            this.Message    = Message;
            this.Reason     = new StringBuilder();

            if (!Initial)
            {
                this.Reason.AppendLine("Your game threw an exception on our servers,\nplease contact one of the developers with these following informations :");

                if (this.Device.GameMode.Player != null)
                {
                    if (this.Device.GameMode.Player.IsNameSet)
                    {
                        this.Reason.AppendLine("Your Player Name    : " + this.Device.GameMode.Player.Name + ".");
                    }

                    this.Reason.AppendLine("Your Player ID      : " + this.Device.GameMode.Player + ".");
                }

                this.Reason.AppendLine();
                this.Reason.AppendLine("Trace : ");
            }

            this.Reason.AppendLine(this.Message);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Reason.ToString());
        }
    }
}