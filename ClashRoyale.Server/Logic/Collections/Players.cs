namespace ClashRoyale.Server.Logic.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Database.Models;
    using ClashRoyale.Server.Files;

    using Newtonsoft.Json;

    internal static class Players
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Players"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        private static ConcurrentDictionary<long, Player> Entities;

        private static int HighId;
        private static int LowId;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (Players.Initialized)
            {
                return;
            }

            Players.Entities    = new ConcurrentDictionary<long, Player>();
            Players.HighId      = Constants.ServerId;
            Players.LowId       = GameDb.GetPlayersSeed();

            Players.Initialized = true;
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        internal static async Task Remove(Player Entity)
        {
            if (Entity == null)
            {
                throw new ArgumentNullException(nameof(Entity), "Entity was null at Remove(Entity).");
            }

            await Players.Save(Entity);
        }

        /// <summary>
        /// Creates a new <see cref="Player"/> and store it into the database.
        /// </summary>
        internal static async Task<Player> Create(Player Entity = null, bool Store = true)
        {
            if (Entity == null)
            {
                Entity = new Player();
                Entity.Initialize();
            }

            JsonConvert.PopulateObject(Home.Json.ToString(), Entity.Home);

            Entity.HighId   = Players.HighId;
            Entity.LowId    = Interlocked.Increment(ref Players.LowId);
            Entity.Token    = Program.Random.NextToken();

            await PlayerDb.Create(Entity);

            if (Store)
            {
                if (Players.Entities.ContainsKey(Entity.PlayerId))
                {
                    if (!Players.Entities.TryUpdate(Entity.PlayerId, Entity, Entity))
                    {
                        Logging.Error(typeof(Players), "TryUpdate(EntityId, Entity, Entity) != true at Add(Entity).");
                    }
                }
                else
                {
                    if (!Players.Entities.TryAdd(Entity.PlayerId, Entity))
                    {
                        Logging.Error(typeof(Players), "TryAdd(EntityId, Entity) != true at Add(Entity).");
                    }
                }
            }

            return Entity;
        }

        /// <summary>
        /// Gets a <see cref="Player"/> from the database using the specified identifiers.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        internal static async Task<Player> Get(int HighId, int LowId, bool Store = true)
        {
            Logging.Warning(typeof(Players), "Get(" + HighId + ", " + LowId + ") has been called.");

            long PlayerId       = (long) HighId << 32 | (uint) LowId;

            PlayerDb PlayerDb   = await PlayerDb.Load(HighId, LowId);
            Player Player       = null;

            if (Players.Entities.TryGetValue(PlayerId, out Player))
            {
                return Player;
            }
            else
            {
                if (PlayerDb != null)
                {
                    if (PlayerDb.Deserialize(out Player))
                    {
                        if (Store)
                        {
                            if (Players.Entities.ContainsKey(Player.PlayerId))
                            {
                                if (!Players.Entities.TryUpdate(Player.PlayerId, Player, Player))
                                {
                                    Logging.Error(typeof(Players), "TryUpdate(EntityId, Player, Player) != true at Get(" + HighId + ", " + LowId + ").");
                                }
                            }
                            else
                            {
                                if (!Players.Entities.TryAdd(Player.PlayerId, Player))
                                {
                                    Logging.Error(typeof(Players), "TryAdd(EntityId, Player) != true at Get(" + HighId + ", " + LowId + ").");
                                }
                            }
                        }

                        return Player;
                    }
                    else
                    {
                        Logging.Error(typeof(Players), "PlayerDb.Deserialize(out Player) != true at Get(" + HighId + ", " + LowId + ").");
                    }
                }
                else
                {
                    Logging.Warning(typeof(Players), "PlayerDb == null at Get(HighId, LowId).");
                }
            }

            return null;
        }

        /// <summary>
        /// Saves the specified <see cref="Player"/> in the database.
        /// </summary>
        /// <param name="Player">The player.</param>
        internal static async Task Save(Player Player)
        {
            var Result = await PlayerDb.Save(Player);

            if (Result == null)
            {
                Logging.Error(typeof(Players), "Result == null");
            }
        }
    }
}
