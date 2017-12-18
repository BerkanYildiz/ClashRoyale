namespace ClashRoyale.Server.Logic.Scoring.Entries
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Maths;

    internal class RankingEntry
    {
        internal LogicLong Id;

        internal string Name;

        internal int Score;
        internal int Order;
        internal int PreviousOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingEntry"/> class.
        /// </summary>
        public RankingEntry()
        {
            // RankingEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RankingEntry"/> class.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Score">The score.</param>
        /// <param name="Order">The order.</param>
        /// <param name="PreviousOrder">The previous order.</param>
        public RankingEntry(LogicLong Id, string Name, int Score, int Order, int PreviousOrder)
        {
            this.Id             = Id;
            this.Name           = Name;
            this.Score          = Score;
            this.Order          = Order;
            this.PreviousOrder  = PreviousOrder;
        }

        /// <summary>
        /// Initializes the specified identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Score">The score.</param>
        /// <param name="Order">The order.</param>
        /// <param name="PreviousOrder">The previous order.</param>
        internal void Initialize(LogicLong Id, string Name, int Score, int Order, int PreviousOrder)
        {
            this.Id             = Id;
            this.Name           = Name;
            this.Score          = Score;
            this.Order          = Order;
            this.PreviousOrder  = PreviousOrder;
        }

        /// <summary>
        /// Decodes the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal virtual void Decode(ByteStream Stream)
        {
            // Decode.
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        internal virtual void Encode(ByteStream Stream)
        {
            Stream.EncodeLogicLong(this.Id);
            Stream.WriteString(this.Name);
            Stream.WriteVInt(this.Order);
            Stream.WriteVInt(this.Score);
            Stream.WriteVInt(this.PreviousOrder);
        }
    }
}