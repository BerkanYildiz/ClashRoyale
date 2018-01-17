namespace ClashRoyale.Logic.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    public static class Devices
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Devices"/> has been already initialized.
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
                return Devices.Entities.Count;
            }
        }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        private static long Seed;

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        private static ConcurrentDictionary<long, Device> Entities;

        /// <summary>
        /// Initializes the slot.
        /// </summary>
        public static void Initialize()
        {
            if (Devices.Initialized)
            {
                return;
            }

            Devices.Entities    = new ConcurrentDictionary<long, Device>();
            Devices.Initialized = true;
        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static void Add(Device Entity)
        {
            Entity.DeviceId = Interlocked.Increment(ref Devices.Seed);

            if (Devices.Entities.ContainsKey(Entity.DeviceId))
            {
                Logging.Error(typeof(Devices), "ContainsKey(EntityId) != false at Add(Entity).");
            }
            else
            {
                if (!Devices.Entities.TryAdd(Entity.DeviceId, Entity))
                {
                    Logging.Error(typeof(Devices), "TryAdd(EntityId, Entity) != true at Add(Entity).");
                }
            }
        }

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="Entity">The entity.</param>
        public static void Remove(Device Entity)
        {
            if (Devices.Entities.TryRemove(Entity.DeviceId, out Device TmpDevice))
            {
                if (TmpDevice.DeviceId == Entity.DeviceId)
                {
                    Entity.DeviceId = 0;
                }
                else
                {
                    Logging.Error(typeof(Devices), "TmpDevice.DeviceId != Entity.DeviceId at Remove(Entity).");
                }
            }
            else
            {
                Logging.Error(typeof(Devices), "TryRemove(EntityId, out TmpDevice) != true at Remove(Entity).");
            }
        }

        /// <summary>
        /// Gets the entity using the specified identifier.
        /// </summary>
        /// <param name="Identifier">The identifier.</param>
        public static Device Get(long Identifier)
        {
            if (Devices.Entities.TryGetValue(Identifier, out Device Device))
            {
                return Device; 
            }

            return null;
        }

        /// <summary>
        /// Executes an action on every players in the collection.
        /// </summary>
        /// <param name="Action">The action to execute on the players.</param>
        /// <param name="Connected">if set to true, only execute the action on connected players.</param>
        public static void ForEach(Action<Device> Action, bool Connected = true)
        {
            var Entities = Devices.Entities.Values;

            if (Connected)
            {
                foreach (var Entity in Entities)
                {
                    if (Entity.Token.IsConnected)
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