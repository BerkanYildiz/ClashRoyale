namespace ClashRoyale.Proxy.Logic.Collections
{
    using System;
    using System.Collections.Concurrent;

    internal static class Devices
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Devices"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        private static ConcurrentDictionary<IntPtr, Device> Entries;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (Devices.Initialized)
            {
                return;
            }

            Devices.Entries     = new ConcurrentDictionary<IntPtr, Device>();
            Devices.Initialized = true;
        }

        /// <summary>
        /// Add the specified device to the list.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal static void Add(Device Device)
        {
            if (Devices.Entries.ContainsKey(Device.Client.Handle))
            {
                Logging.Info(typeof(Devices), "Entries.ContainsKey(Device.Client.Handle) == true at Add(Device).");
            }
            else
            {
                if (!Devices.Entries.TryAdd(Device.Client.Handle, Device))
                {
                    Logging.Error(typeof(Devices), "Entries.TryAdd(IntPtr, Device) != true at Add(Device).");
                }
            }
        }

        /// <summary>
        /// Remove the specified device from the list.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal static void Remove(Device Device)
        {
            if (Devices.Entries.TryRemove(Device.Client.Handle, out Device TmpDevice))
            {
                if (TmpDevice.Client != Device.Client)
                {
                    Logging.Error(typeof(Devices), "TmpDevice.Client != Device.Client at Remove(Device).");
                }
            }
            else
            {
                Logging.Error(typeof(Devices), "Entries.TryRemove(IntPtr, out TmpDevice) != true at Remove(Device).");
            }
        }

        /// <summary>
        /// Gets the <see cref="Device"/> from the list using the specified handle.
        /// </summary>
        /// <param name="Handle">The handle.</param>
        internal static Device Get(IntPtr Handle)
        {
            if (Devices.Entries.TryGetValue(Handle, out Device Device))
            {
                return Device;
            }
            else
            {
                Logging.Error(typeof(Devices), "Entries.TryGetValue(Handle, out Device) != true at Get(IntPtr).");
            }

            return null;
        }
    }
}