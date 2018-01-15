namespace ClashRoyale.Database.Models
{
    using System.Threading.Tasks;

    using ClashRoyale.Logic.Alliance;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    public class ClanDb
    {
        /// <summary>
        /// The settings for the <see cref="JsonConvert" /> class.
        /// </summary>
        [BsonIgnore]
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.None,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            ReferenceLoopHandling       = ReferenceLoopHandling.Ignore,     Formatting              = Formatting.None
        };

        [BsonId]                    public BsonObjectId _id;

        [BsonElement("highId")]     public int HighId;
        [BsonElement("lowId")]      public int LowId;

        [BsonElement("profile")]    public BsonDocument Profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanDb"/> class.
        /// </summary>
        public ClanDb()
        {
            // ClanDb.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanDb"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Profile">The profile.</param>
        public ClanDb(int HighId, int LowId, string Json)
        {
            this.HighId     = HighId;
            this.LowId      = LowId;
            this.Profile    = BsonDocument.Parse(Json);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanDb"/> class.
        /// </summary>
        /// <param name="Clan">The clan.</param>
        public ClanDb(Clan Clan)
        {
            this.HighId     = Clan.HighId;
            this.LowId      = Clan.LowId;
            this.Profile    = BsonDocument.Parse(JsonConvert.SerializeObject(Clan, ClanDb.JsonSettings));
        }

        /// <summary>
        /// Creates the specified clan.
        /// </summary>
        /// <param name="Clan">The clan.</param>
        public static async Task Create(Clan Clan)
        {
            await GameDb.Clans.InsertOneAsync(new ClanDb(Clan));
        }

        /// <summary>
        /// Creates the clan in the database.
        /// </summary>
        public static async Task<ClanDb> Save(Clan Clan)
        {
            var UpdatedEntity = await GameDb.Clans.FindOneAndUpdateAsync(ClanDb =>

                ClanDb.HighId   == Clan.HighId &&
                ClanDb.LowId    == Clan.LowId,
                
                Builders<ClanDb>.Update.Set(ClanDb => ClanDb.Profile, BsonDocument.Parse(JsonConvert.SerializeObject(Clan, ClanDb.JsonSettings)))
            );

            if (UpdatedEntity != null)
            {
                if (UpdatedEntity.HighId == Clan.HighId && UpdatedEntity.LowId == Clan.LowId)
                {
                    return UpdatedEntity;
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "UpdatedEntity.Ids != this.Ids at Save().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "UpdatedEntity == null at Save().");
            }

            return null;
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        public static async Task<ClanDb> Load(int HighId, int LowId)
        {
            if (LowId > 0)
            {
                var Entities = await GameDb.Clans.FindAsync(ClanDb => ClanDb.HighId == HighId && ClanDb.LowId == LowId);

                if (Entities != null)
                {
                    var Entity = Entities.FirstOrDefault();

                    if (Entity != null)
                    {
                        return Entity;
                    }
                    else
                    {
                        Logging.Error(typeof(PlayerDb), "Entity == null at Load().");
                    }
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "Entities == null at Load().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "this.LowId < 0 at Load().");
            }

            return null;
        }

        /// <summary>
        /// Deletes this instance from the database.
        /// </summary>
        public static async Task<bool> Delete(int HighId, int LowId)
        {
            if (LowId > 0)
            {
                var Result = await GameDb.Clans.DeleteOneAsync(ClanDb => ClanDb.HighId == HighId && ClanDb.LowId == LowId);

                if (Result.IsAcknowledged)
                {
                    if (Result.DeletedCount > 0)
                    {
                        if (Result.DeletedCount == 1)
                        {
                            return true;
                        }
                        else
                        {
                            Logging.Error(typeof(PlayerDb), "Result.DeletedCount > 1 at Delete().");
                        }
                    }
                    else
                    {
                        Logging.Warning(typeof(PlayerDb), "Result.DeletedCount == 0 at Delete().");
                    }
                }
                else
                {
                    Logging.Error(typeof(PlayerDb), "Result.IsAcknowledged != true at Delete().");
                }
            }
            else
            {
                Logging.Error(typeof(PlayerDb), "LowId <= 0 at Delete(HighId, LowId).");
            }

            return false;
        }

        /// <summary>
        /// Deserializes the specified entity.
        /// </summary>
        public bool Deserialize(out Clan Clan)
        {
            if (this.Profile != null)
            {
                Clan = JsonConvert.DeserializeObject<Clan>(this.Profile.ToJson(), ClanDb.JsonSettings);

                if (Clan != null)
                {
                    return true;
                }
            }
            else
            {
                Clan = null;
            }

            return false;
        }
    }
}