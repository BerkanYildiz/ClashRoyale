namespace ClashRoyale.Server.Logic
{
    using MongoDB.Bson.Serialization.Attributes;

    internal class Player
    {
        [BsonElement("highId")]     internal int HighId;
        [BsonElement("lowId")]      internal int LowId;

        [BsonElement("token")]      internal string Token;

        [BsonElement("home")]       internal Home Home;

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        internal long PlayerId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has been already initialized.
        /// </summary>
        internal bool IsInitialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        internal Player()
        {
            this.Home = new Home(this);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            
        }
    }
}
