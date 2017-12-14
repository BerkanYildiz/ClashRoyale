namespace ClashRoyale.Server.Network.Packets
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Network.Packets.Client;
    using ClashRoyale.Server.Network.Packets.Server;

    internal static class Factory
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Factory"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dictionnary of messages with the identifier and the type.
        /// </summary>
        private static Dictionary<short, Type> Messages
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (Factory.Initialized)
            {
                return;
            }

            Factory.Messages = new Dictionary<short, Type>();
            Factory.Messages.Add(10100, typeof(PreLoginMessage));
            Factory.Messages.Add(10101, typeof(LoginMessage));
            Factory.Messages.Add(19911, typeof(KeepAliveMessage));

            Factory.Messages.Add(20103, typeof(LoginFailedMessage));
            Factory.Messages.Add(22280, typeof(LoginOkMessage));
            Factory.Messages.Add(28502, typeof(OwnHomeDataMessage));
            Factory.Messages.Add(24135, typeof(KeepAliveOkMessage));
        }

        /// <summary>
        /// Creates a message with the specified type.
        /// </summary>
        internal static Message CreateMessage(short Type, Device Device, ByteStream Stream)
        {
            if (Factory.Messages.TryGetValue(Type, out Type Message))
            {
                return (Message) Activator.CreateInstance(Message, Device, Stream);
            }
            else
            {
                Logging.Warning(typeof(Factory), "Messages.TryGetValue(" + Type + ", out Message) != true at CreateMessage(" + Type + ", Device, Stream).");
            }

            return null;
        }
    }
}