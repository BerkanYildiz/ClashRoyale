namespace ClashRoyale.Messages.Server.Home
{
    using System.Text;

    using ClashRoyale.Enums;

    public class ServerErrorMessage : Message
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

        public string Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerErrorMessage"/> class.
        /// </summary>
        public ServerErrorMessage()
        {
            // ServerErrorMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerErrorMessage"/> class.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="Initial">if set to <c>true</c> [initial].</param>
        public ServerErrorMessage(string Message = "", bool Initial = false)
        {
            StringBuilder Builder = new StringBuilder();

            if (!Initial)
            {
                Builder.AppendLine("Your game threw an exception on our servers,");
                Builder.AppendLine("please contact one of the developers with these following informations :");

                /* if (this.Device.GameMode.Player != null)
                {
                    Builder.AppendLine();

                    if (this.Device.GameMode.Player.IsNameSet)
                    {
                        Builder.AppendLine("Your Player Name    : " + this.Device.GameMode.Player.Name + ".");
                    }

                    Builder.AppendLine("Your Player ID      : " + this.Device.GameMode.Player + ".");
                    Builder.AppendLine();
                } */
                Builder.AppendLine("Trace : ");
            }

            Builder.AppendLine(Message);
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.Reason = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.Reason);
        }
    }
}