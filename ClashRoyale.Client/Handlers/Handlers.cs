namespace ClashRoyale.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Handlers.Client;
    using ClashRoyale.Handlers.Server;

    using ClashRoyale.Logic;

    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Home;

    public static class Handlers
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MessageHandlers"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// The message handler, used to process the received and sent messages.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation token.</param>
        public delegate Task MessageHandler(Device Device, Message Message, CancellationToken Cancellation);


        /// <summary>
        /// The dictionnary of handlers, used to route packet ids and handle them.
        /// </summary>
        public static readonly Dictionary<short, MessageHandler> MessageHandlers = new Dictionary<short, MessageHandler>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (Handlers.Initialized)
            {
                return;
            }

            Handlers.MessageHandlers.Add(new ClientHelloMessage().Type,                ClientHelloHandler.Handle);
            Handlers.MessageHandlers.Add(new ServerHelloMessage().Type,                ServerHelloHandler.Handle);

            Handlers.MessageHandlers.Add(new LoginMessage().Type,                      LoginHandler.Handle);
            Handlers.MessageHandlers.Add(new LoginOkMessage().Type,                    LoginOkHandler.Handle);
            Handlers.MessageHandlers.Add(new LoginFailedMessage().Type,                LoginFailedHandler.Handle);

            Handlers.MessageHandlers.Add(new ClientCapabilitiesMessage().Type,         ClientCapabilitiesHandler.Handle);
            Handlers.MessageHandlers.Add(new EndClientTurnMessage().Type,              EndClientTurnHandler.Handle);

            Handlers.MessageHandlers.Add(new KeepAliveMessage().Type,                  KeepAliveHandler.Handle);
            Handlers.MessageHandlers.Add(new KeepAliveOkMessage().Type,                KeepAliveOkHandler.Handle);

            Handlers.MessageHandlers.Add(new GoHomeMessage().Type,                     GoHomeHandler.Handle);

            Handlers.MessageHandlers.Add(new ServerErrorMessage().Type,                ServerErrorHandler.Handle);
            Handlers.MessageHandlers.Add(new ServerShutdownMessage().Type,             ServerShutdownHandler.Handle);
            Handlers.MessageHandlers.Add(new DisconnectedMessage().Type,               DisconnectedHandler.Handle);

            Handlers.Initialized = true;
        }

        /// <summary>
        /// Handles the specified <see cref="Message"/> using the specified <see cref="Device"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        public static async Task MessageHandle(Device Device, Message Message)
        {
            using (var Cancellation = new CancellationTokenSource())
            {
                var Token = Cancellation.Token;

                if (Handlers.MessageHandlers.TryGetValue(Message.Type, out Handlers.MessageHandler Handler))
                {
                    Cancellation.CancelAfter(4000);

                    try
                    {
                        await Handler(Device, Message, Token);
                    }
                    catch (OperationCanceledException)
                    {
                        Logging.Warning(typeof(MessageFactory), "Operation has been cancelled after 4 seconds.");
                    }
                }
            }
        }
    }
}