namespace ClashRoyale.Logic.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json;

    using GameDb    = ClashRoyale.Database.GameDb;
    using PlayerDb  = ClashRoyale.Database.Models.PlayerDb;

    public static class Players
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Players"/> has been already initialized.
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
                return Players.Entities.Count;
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
        private static ConcurrentDictionary<long, Player> Entities;

        /// <summary>
        /// Initializes the slot.
        /// </summary>
        public static void Initialize()
        {
            if (Players.Initialized)
            {
                return;
            }

            Players.Entities    = new ConcurrentDictionary<long, Player>();
            Players.HighSeed    = Config.ServerId;
            Players.LowSeed     = GameDb.GetPlayersSeed();

            Players.Initialized = true;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static void Add(Player Entity)
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

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Remove(Player Entity)
        {
            Player TmpPlayer;

            if (Entity.GameMode != null)
            {
                if (Entity.GameMode.CommandManager.AvailableServerCommands.Count > 0)
                {
                    foreach (Command Command in Entity.GameMode.CommandManager.AvailableServerCommands.Values.ToArray())
                    {
                        if (Command.IsServerCommand)
                        {
                            Command.Execute(Entity.GameMode);
                        }
                    }
                }

                if (Entity.IsInAlliance)
                {
                    Clan Clan = Clans.Get(Entity.ClanHighId, Entity.ClanLowId).Result;

                    if (Clan != null)
                    {
                        await Clan.Members.TryRemoveOnlineMember(Entity);
                    }
                }

                if (Entity.GameMode.Battle != null)
                {
                    foreach (Player Player2 in Entity.GameMode.Battle.Players)
                    {
                        if (Player2 != null)
                        {
                            Player2.GameMode.SectorManager.OpponentLeftMatch();
                        }
                    }
                }
            }

            await Players.Save(Entity);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<Player> Create(Player Entity = null, bool Store = true)
        {
            if (Entity == null)
            {
                Entity = new Player();
                Entity.Initialize();
            }

            if (Entity.HighId == 0)
            {
                Entity.HighId   = Players.HighSeed;
            }

            if (Entity.LowId == 0)
            {
                Entity.LowId    = Interlocked.Increment(ref Players.LowSeed);
            }

            JsonConvert.PopulateObject(Files.Home.Json.ToString(), Entity.Home);

            Entity.Home.HighId  = Entity.HighId;
            Entity.Home.LowId   = Entity.LowId;

            if (string.IsNullOrEmpty(Entity.Token))
            {
                Entity.Token    = XorShift.NextToken();
            }

            await PlayerDb.Create(Entity);

            if (Store)
            {
                Players.Add(Entity);
            }

            return Entity;
        }

        /// <summary>
        /// Gets the entity using the specified identifiers.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        /// <param name="Store">Whether it has to be stored.</param>
        public static async Task<Player> Get(int HighId, int LowId, bool Store = true)
        {
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
                            Players.Add(Player);
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

            return Player;
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static async Task Save(Player Entity)
        {
            var Result = await PlayerDb.Save(Entity);

            if (Result == null)
            {
                Logging.Error(typeof(Players), "Result == null at Save(Entity).");
            }
        }

        /// <summary>
        /// Saves every entities in this slot.
        /// </summary>
        public static async Task SaveAll()
        {
            var Players       = Collections.Players.Entities.Values.ToArray();
            var RequestsTasks = new Task[Players.Length];

            for (var I = 0; I < Players.Length; I++)
            {
                RequestsTasks[I] = Collections.Players.Save(Players[I]);
            }

            await Task.WhenAll(RequestsTasks);
        }

        /// <summary>
        /// Executes an action on every players in the collection.
        /// </summary>
        /// <param name="Action">The action to execute on the players.</param>
        /// <param name="Connected">if set to true, only execute the action on connected players.</param>
        public static void ForEach(Action<Player> Action, bool Connected = true)
        {
            var Entities = Players.Entities.Values;

            if (Connected)
            {
                foreach (var Entity in Entities)
                {
                    if (Entity.GameMode.IsConnected)
                    {
                        Action.Invoke(Entity);
                    }
                }
            }
            else
            {
                foreach (var Entity in Entities)
                {
                    Action.Invoke(Entity);
                }
            }
        }
    }
}