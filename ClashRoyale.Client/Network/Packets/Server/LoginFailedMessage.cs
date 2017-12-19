namespace ClashRoyale.Client.Network.Packets.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    using Newtonsoft.Json.Linq;

    internal class LoginFailedMessage : Message
    {
        /// <summary>
        /// The type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 20103;
            }
        }

        /// <summary>
        /// The service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        private Reason Reason;

        private string Fingerprint;
        private string PatchingHost;
        private string RedirectHost;
        private string UpdateHost;
        private string Message;

        private int TimeLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Stream">The stream.</param>
        public LoginFailedMessage(Device Device, ByteStream Stream) : base(Device, Stream)
        {
            // LoginFailedMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            this.Reason         = (Reason) this.Stream.ReadVInt();

            this.Fingerprint    = this.Stream.ReadString();
            this.RedirectHost   = this.Stream.ReadString();
            this.PatchingHost   = this.Stream.ReadString();
            this.UpdateHost     = this.Stream.ReadString();
            this.Message        = this.Stream.ReadString();
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            int ErrorCode;

            if (!int.TryParse(this.Reason.ToString(), out ErrorCode))
            {
                System.Diagnostics.Debug.WriteLine("[*] We've been disconnected because : " + this.Reason + ".");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[*] We've been disconnected because of error code " + ErrorCode + ".");
            }

            if (this.Reason == Reason.Patch)
            {
                System.Diagnostics.Debug.WriteLine("[*] " + this.GetType().Name + " : " + JObject.Parse(this.Fingerprint)["sha"]);
            }

            this.Device.State = State.Disconnected;
        }
    }
}