namespace ClashRoyale.Client.Packets.Messages.Server
{
    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Logic.Enums;

    using Newtonsoft.Json.Linq;

    internal class Authentification_Failed : Message
    {
        internal Reason Reason;

        internal string Fingerprint;
        internal string PatchingHost;
        internal string RedirectHost;
        internal string UpdateHost;
        internal string Message;

        internal int TimeLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="Authentification_Failed"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        public Authentification_Failed(Device Device, Reader Reader) : base(Device, Reader)
        {
            this.Device.State = State.DISCONNECTED;
        }

        /// <summary>
        /// Decodes the <see cref="Message" />, using the <see cref="Reader" /> instance.
        /// </summary>
        internal override void Decode()
        {
            this.Reason         = (Reason) this.Reader.ReadVInt();

            this.Fingerprint    = this.Reader.ReadString();

            this.RedirectHost   = this.Reader.ReadString();
            this.PatchingHost   = this.Reader.ReadString();
            this.UpdateHost     = this.Reader.ReadString();
            this.Message        = this.Reader.ReadString();
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
        }
    }
}