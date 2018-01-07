namespace ClashRoyale.Logic.Collections
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Logic.Alliance;

    using ClanDb = ClashRoyale.Database.Models.ClanDb;
    using GameDb = ClashRoyale.Database.GameDb;

    public static class Clans
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Clans"/> has been already initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the count of <see cref="Clan"/> instances.
        /// </summary>
        public static int Count
        {
            get
            {
                return Clans.Entities.Count;
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
        private static ConcurrentDictionary<long, Clan> Entities;

        /// <summary>
        /// Initializes the slot.
        /// </summary>
        public static void Initialize()
        {
            if (Clans.Initialized)
            {
                return;
            }

            Clans.Entities      = new ConcurrentDictionary<long, Clan>();
            Clans.HighSeed      = Config.ServerId;
            Clans.LowSeed       = GameDb.GetClansSeed();

            Clans.Initialized   = true;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static void Add(Clan Entity)
        {
            if (Clans.Entities.ContainsKey(Entity.AllianceId))
            {
                if (!Clans.Entities.TryUpdate(Entity.AllianceId, Entity, Entity))
                {
                    Logging.Error(typeof(Clans), "TryUpdate(EntityId, Entity, Entity) != true at Add(Entity).");
                }
            }
            else
            {
                if (!Clans.Entities.TryAdd(Entity.AllianceId, Entity))
                {
                    Logging.Error(typeof(Clans), "TryAdd(EntityId, Entity) != true at Add(Entity).");
                }
            }
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Remove(Clan Entity)
        {
            await Clans.Save(Entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<Clan> Create(Clan Entity = null, bool Store = true)
        {
            if (Entity == null)
            {
                Entity    = new Clan();
            }

            Entity.HighId = Clans.HighSeed;
            Entity.LowId  = Interlocked.Increment(ref Clans.LowSeed);

            await ClanDb.Create(Entity);
            
            if (Store)
            {
                Clans.Add(Entity);
            }

            return Entity;
        }

        /// <summary>
        /// Gets the entity using the specified identifiers.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<Clan> Get(int HighId, int LowId, bool Store = true)
        {
            Logging.Warning(typeof(Clans), "Get(" + HighId + ", " + LowId + ") has been called.");

            long ClanId     = (long) HighId << 32 | (uint) LowId;

            ClanDb ClanDb   = await ClanDb.Load(HighId, LowId);
            Clan Clan       = null;

            if (Clans.Entities.TryGetValue(ClanId, out Clan))
            {
                return Clan; 
            }
            else
            {
                if (ClanDb != null)
                {
                    if (ClanDb.Deserialize(out Clan))
                    {
                        Clan.LoadingFinished();

                        if (Store)
                        {
                            Clans.Add(Clan);
                        }

                        return Clan;
                    }
                    else
                    {
                        Logging.Error(typeof(Clans), "ClanDb.Deserialize(out Clan) != true at Get(" + HighId + ", " + LowId + ").");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Clans), "ClanDb == null at Get(HighId, LowId).");
                }
            }

            return Clan;
        }

        /// <summary>
        /// Gets all the clans available in this instane.
        /// </summary>
        /// <returns></returns>
        public static List<Clan> GetAll()
        {
            List<Clan> Clans = new List<Clan>();
            Clans.AddRange(Entities.Values);
            return Clans;
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Save(Clan Entity)
        {
            var Result = await ClanDb.Save(Entity);

            if (Result == null)
            {
                Logging.Error(typeof(Clans), "Result == null at Save(Entity).");
            }
        }

        /// <summary>
        /// Saves every entities in this slot.
        /// </summary>
        public static async Task SaveAll()
        {
            var Clans         = Collections.Clans.Entities.Values.ToArray();
            var RequestsTasks = new Task[Clans.Length];

            for (var I = 0; I < Clans.Length; I++)
            {
                RequestsTasks[I] = Collections.Clans.Save(Clans[I]);
            }

            await Task.WhenAll(RequestsTasks);
        }
    }
}