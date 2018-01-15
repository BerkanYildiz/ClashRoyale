namespace ClashRoyale.Logic.Collections
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Logic.Battle;

    using BattleDb  = ClashRoyale.Database.Models.BattleDb;
    using GameDb    = ClashRoyale.Database.GameDb;

    public static class Battles
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Battles"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        public static int Count
        {
            get
            {
                return Battles.Entities.Count;
            }
        }

        /// <summary>
        /// Gets or sets the highest seed.
        /// </summary>
        private static int HighSeed;

        /// <summary>
        /// Gets or sets the lowest seed.
        /// </summary>
        private static int LowSeed;

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        private static ConcurrentDictionary<long, BattleLog> Entities;

        /// <summary>
        /// Initializes the slot.
        /// </summary>
        public static void Initialize()
        {
            if (Battles.Initialized)
            {
                return;
            }

            Battles.Entities    = new ConcurrentDictionary<long, BattleLog>();
            Battles.HighSeed    = Config.ServerId;
            Battles.LowSeed     = GameDb.GetBattlesSeed();

            /* for (int i = 1; i <= this.LowSeed; i++)
            {
                BattleLog BattleLog = await this.Get(Constants.ServerID, i);
                this.RemainingSecs -= Resources.Database.TimeToLoadSave;

                if (BattleLog != null)
                {
                    int ChannelIdx = Resources.ServerManager.RoyalTvManager.GetChannelArenaData(BattleLog.ArenaData);

                    if (ChannelIdx != -1)
                    {
                        Resources.ServerManager.RoyalTvManager.AddEntry(ChannelIdx, new RoyalTvEntry(BattleLog));
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "ChannelIdx == -1 at StartLoading().");
                    }
                }
            } */
            Battles.Initialized   = true;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static void Add(BattleLog Entity)
        {
            if (Battles.Entities.ContainsKey(Entity.BattleId))
            {
                if (!Battles.Entities.TryUpdate(Entity.BattleId, Entity, Entity))
                {
                    Logging.Error(typeof(Battles), "TryUpdate(EntityId, Entity, Entity) != true at Add(Entity).");
                }
            }
            else
            {
                if (!Battles.Entities.TryAdd(Entity.BattleId, Entity))
                {
                    Logging.Error(typeof(Battles), "TryAdd(EntityId, Entity) != true at Add(Entity).");
                }
            }
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Remove(BattleLog Entity)
        {
            await Battles.Save(Entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<BattleLog> Create(BattleLog Entity = null, bool Store = true)
        {
            if (Entity == null)
            {
                Entity    = new BattleLog();
            }

            Entity.HighId = Battles.HighSeed;
            Entity.LowId  = Interlocked.Increment(ref Battles.LowSeed);

            await BattleDb.Create(Entity);
            
            if (Store)
            {
                Battles.Add(Entity);
            }

            return Entity;
        }

        /// <summary>
        /// Gets the entity using the specified identifiers.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<BattleLog> Get(int HighId, int LowId, bool Store = true)
        {
            Logging.Warning(typeof(Battles), "Get(" + HighId + ", " + LowId + ") has been called.");

            long BattleId           = (long) HighId << 32 | (uint) LowId;

            BattleDb BattleDb       = await BattleDb.Load(HighId, LowId);
            BattleLog BattleLog     = null;

            if (Battles.Entities.TryGetValue(BattleId, out BattleLog))
            {
                return BattleLog; 
            }
            else
            {
                if (BattleDb != null)
                {
                    if (BattleDb.Deserialize(out BattleLog))
                    {
                        if (Store)
                        {
                            Battles.Add(BattleLog);
                        }

                        return BattleLog;
                    }
                    else
                    {
                        Logging.Error(typeof(Battles), "BattleDb.Deserialize(out BattleLog) != true at Get(" + HighId + ", " + LowId + ").");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Battles), "BattleDb == null at Get(HighId, LowId).");
                }
            }

            return BattleLog;
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Save(BattleLog Entity)
        {
            var Result = await BattleDb.Save(Entity);

            if (Result == null)
            {
                Logging.Error(typeof(Battles), "Result == null at Save(Entity).");
            }
        }

        /// <summary>
        /// Saves every entities in this slot.
        /// </summary>
        public static async Task SaveAll()
        {
            var Battles       = Collections.Battles.Entities.Values.ToArray();
            var RequestsTasks = new Task[Battles.Length];

            for (var I = 0; I < Battles.Length; I++)
            {
                RequestsTasks[I] = Collections.Battles.Save(Battles[I]);
            }

            await Task.WhenAll(RequestsTasks);
        }
    }
}