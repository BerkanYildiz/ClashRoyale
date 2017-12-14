namespace ClashRoyale.Server.Database.Models
{
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    internal class ClanDb
    {
        [BsonId]                    internal BsonObjectId _id;

        [BsonElement("highId")]     internal int HighId;
        [BsonElement("lowId")]      internal int LowId;

        [BsonElement("profile")]    internal BsonDocument Profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanDb"/> class.
        /// </summary>
        internal ClanDb()
        {
            // ClanDb.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanDb"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Profile">The profile.</param>
        internal ClanDb(int HighId, int LowId, BsonDocument Profile)
        {
            this.HighId     = HighId;
            this.LowId      = LowId;
            this.Profile    = Profile;
        }

        /// <summary>
        /// Creates the clan in the database.
        /// </summary>
        internal async Task Save()
        {
            if (this._id == null)
            {
                var Entities = await GameDb.Clans.FindAsync(Clan => Clan.HighId == this.LowId && Clan.LowId == this.LowId);

                if (Entities != null)
                {
                    var Entity = Entities.First();

                    if (Entity != null)
                    {
                        this._id = Entity._id;
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Entity == null at Save().");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Entities == null at Save().");
                }
            }

            if (this._id == null)
            {
                await GameDb.Clans.InsertOneAsync(this);
            }
            else
            {
                var UpdatedEntity = await GameDb.Clans.FindOneAndUpdateAsync(Player => Player._id == this._id, Builders<ClanDb>.Update.Set(Clan => Clan.Profile, this.Profile));

                if (UpdatedEntity != null)
                {
                    if (UpdatedEntity.HighId == this.HighId && UpdatedEntity.LowId == this.LowId)
                    {
                        // TODO : Remove this check.
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "UpdatedEntity.Ids != this.Ids at Save().");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "UpdatedEntity == null at Save().");
                }
            }
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        internal async Task Load()
        {
            if (this._id == null)
            {
                if (this.LowId > 0)
                {
                    var Entities = await GameDb.Clans.FindAsync(Clan => Clan.HighId == this.LowId && Clan.LowId == this.LowId);

                    if (Entities != null)
                    {
                        var Entity = Entities.First();

                        if (Entity != null)
                        {
                            this._id = Entity._id;
                        }
                        else
                        {
                            Logging.Error(this.GetType(), "Entity == null at Load().");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Entities == null at Load().");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "this.LowId < 0 at Load().");
                }
            }
            else
            {
                Logging.Warning(this.GetType(), "BsonId != null at Load().");
            }
        }

        /// <summary>
        /// Deletes this instance from the database.
        /// </summary>
        internal async Task Delete()
        {
            if (this._id != null)
            {
                var Result = await GameDb.Clans.DeleteOneAsync(Clan => Clan._id == this._id);

                if (Result.IsAcknowledged)
                {
                    if (Result.DeletedCount > 0)
                    {
                        if (Result.DeletedCount > 1)
                        {
                            Logging.Error(this.GetType(), "Result.DeletedCount > 1 at Delete().");
                        }
                    }
                    else
                    {
                        Logging.Warning(this.GetType(), "Result.DeletedCount == 0 at Delete().");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "Result.IsAcknowledged != true at Delete().");
                }
            }
            else
            {
                if (this.LowId > 0)
                {
                    var Result = await GameDb.Clans.DeleteOneAsync(Clan => Clan.HighId == this.HighId && Clan.LowId == this.LowId);

                    if (Result.IsAcknowledged)
                    {
                        if (Result.DeletedCount > 0)
                        {
                            if (Result.DeletedCount > 1)
                            {
                                Logging.Error(this.GetType(), "Result.DeletedCount > 1 at Delete().");
                            }
                        }
                        else
                        {
                            Logging.Warning(this.GetType(), "Result.DeletedCount == 0 at Delete().");
                        }
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Result.IsAcknowledged != true at Delete().");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "this._id == null && this.LowId == 0 at Delete().");
                }
            }
        }
    }
}