namespace ClashRoyale.Server.Logic.Time
{
    using ClashRoyale.Extensions;

    internal struct Time
    {
        private int ClientTick;
        private int ServerTick;
        
        /// <summary>
        /// Gets if the client is off sync.
        /// </summary>
        internal bool IsClientOffSync
        {
            get
            {
                return this.ClientTick > this.ServerTick + 60;
            }
        }

        /// <summary>
        /// Increases the tick.
        /// </summary>
        internal void IncreaseTick()
        {
            ++this.ClientTick;
            ++this.ServerTick;
        }

        /// <summary>
        /// Sets the server tick.
        /// </summary>
        internal void Update(float Time)
        {
            this.ClientTick += (int) (20 * Time);
            this.ServerTick += (int) (20 * Time);
        }

        /// <summary>
        /// Sets the server tick.
        /// </summary>
        internal void SetServerTick(int Tick)
        {
            this.ClientTick = Tick;
            this.ServerTick = Tick;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(this.ClientTick);
        }

        /// <summary>
        /// Returns the seconds converted in ticks.
        /// </summary>
        internal static int GetSecondsInTicks(int Seconds)
        {
            return 20 * Seconds;
        }

        public static implicit operator int(Time Time)
        {
            return Time.ClientTick;
        }
    }
}