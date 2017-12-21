namespace ClashRoyale.Client.Network.Packets
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Client.Logic;
    using ClashRoyale.Client.Network.Packets.Client;
    using ClashRoyale.Client.Network.Packets.Server;

    using ClashRoyale.Extensions;

    internal static class Factory
    {
        /// <summary>
        /// The delimiter used to detect if x string is a call-command.
        /// </summary>
        internal const char Delimiter = '/';

        internal static readonly Dictionary<short, Type> Messages    = new Dictionary<short, Type>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            Factory.LoadMessages();
        }

        /// <summary>
        /// Loads the game messages.
        /// </summary>
        private static void LoadMessages()
        {
            Factory.Messages.Add(10100, typeof(PreLoginMessage));
            Factory.Messages.Add(10101, typeof(LoginMessage));
            Factory.Messages.Add(15080, typeof(RequestApiKeyMessage));

            // Factory.Messages.Add(18688, typeof(EndClientTurnMessage));
            Factory.Messages.Add(19911, typeof(KeepAliveMessage));

            Factory.Messages.Add(20100, typeof(PreLoginOkMessage));
            Factory.Messages.Add(20103, typeof(LoginFailedMessage));
            Factory.Messages.Add(22280, typeof(LoginOkMessage));
            // Factory.Messages.Add(22726, typeof(ApiKeyDataMessage));
            Factory.Messages.Add(24135, typeof(KeepAliveOkMessage));
            // Factory.Messages.Add(25880, typeof(VisitedHomeDataMessage));
            Factory.Messages.Add(28502, typeof(OwnHomeDataMessage));
        }

        /// <summary>
        /// Creates a message with the specified type.
        /// </summary>
        internal static Message CreateMessage(short Type, Bot Bot, ByteStream Stream)
        {
            if (Factory.Messages.TryGetValue(Type, out Type Message))
            {
                return (Message) Activator.CreateInstance(Message, Bot, Stream);
            }

            Logging.Warning(typeof(Factory), "Messages.TryGetValue(" + Type + ", out Message) != true at CreateMessage(Type, Bot, Stream).");

            return null;
        }
    }
}