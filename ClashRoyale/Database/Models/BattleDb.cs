namespace ClashRoyale.Database.Models
{
    using System.Threading.Tasks;

    using ClashRoyale.Logic.Battle;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    public class BattleDb
    {
        /// <summary>
        /// The settings for the <see cref="JsonConvert" /> class.
        /// </summary>
        [BsonIgnore]
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.None,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            Formatting                  = Formatting.None
        };

        [BsonId]                    public BsonObjectId _id;

        [BsonElement("highId")]     public int HighId;
        [BsonElement("lowId")]      public int LowId;

        [BsonElement("profile")]    public BsonDocument Profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleDb"/> class.
        /// </summary>
        public BattleDb()
        {
            // BattleDb.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleDb"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Profile">The profile.</param>
        public BattleDb(int HighId, int LowId, string Json)
        {
            this.HighId     = HighId;
            this.LowId      = LowId;
            this.Profile    = BsonDocument.Parse(Json);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleDb"/> class.
        /// </summary>
        /// <param name="BattleLog">The battle log.</param>
        public BattleDb(BattleLog BattleLog)
        {
            this.HighId     = BattleLog.HighId;
            this.LowId      = BattleLog.LowId;
            this.Profile    = BsonDocument.Parse(JsonConvert.SerializeObject(BattleLog, BattleDb.JsonSettings));
        }

        /// <summary>
        /// Creates the specified battle log.
        /// </summary>
        /// <param name="BattleLog">The battle log.</param>
        public static async Task Create(BattleLog BattleLog)
        {
            await GameDb.Battles.InsertOneAsync(new BattleDb(BattleLog));
        }

        /// <summary>
        /// Creates the battle log in the database.
        /// </summary>
        public static async Task<BattleDb> Save(BattleLog BattleLog)
        {
            var UpdatedEntity = await GameDb.Battles.FindOneAndUpdateAsync(BattleDb =>

                BattleDb.HighId == BattleLog.HighId &&
                BattleDb.LowId  == BattleLog.LowId,
                
                Builders<BattleDb>.Update.Set(BattleDb => BattleDb.Profile, BsonDocument.Parse(JsonConvert.SerializeObject(BattleLog, BattleDb.JsonSettings)))
            );

            if (UpdatedEntity != null)
            {
                if (UpdatedEntity.HighId == BattleLog.HighId && UpdatedEntity.LowId == BattleLog.LowId)
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
        public static async Task<BattleDb> Load(int HighId, int LowId)
        {
            if (LowId > 0)
            {
                var Entities = await GameDb.Battles.FindAsync(BattleDb => BattleDb.HighId == HighId && BattleDb.LowId == LowId);

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
                var Result = await GameDb.Battles.DeleteOneAsync(BattleDb => BattleDb.HighId == HighId && BattleDb.LowId == LowId);

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
        public bool Deserialize(out BattleLog BattleLog)
        {
            if (this.Profile != null)
            {
                BattleLog = JsonConvert.DeserializeObject<BattleLog>(this.Profile.ToJson(), BattleDb.JsonSettings);

                if (BattleLog != null)
                {
                    return true;
                }
            }
            else
            {
                BattleLog = null;
            }

            return false;
        }
    }
}