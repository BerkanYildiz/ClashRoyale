namespace ClashRoyale.Server.Handlers
{
    using ClashRoyale.Messages.Client;
    using ClashRoyale.Messages.Server.Account;

    using ClashRoyale.Server.Handlers.Client;
    using ClashRoyale.Server.Handlers.Server;

    internal static class Factory
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Factory"/> has been initialized.
        /// </summary>
        internal static bool Initialized
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

            Messages.Factory.Initialize();

            Messages.Factory.Handlers.Add(new ClientHelloMessage().Type,        ClientHelloHandler.Handle);
            Messages.Factory.Handlers.Add(new ServerHelloMessage().Type,        ServerHelloHandler.Handle);
            Messages.Factory.Handlers.Add(new LoginMessage().Type,              LoginHandler.Handle);
            Messages.Factory.Handlers.Add(new LoginOkMessage().Type,            LoginOkHandler.Handle);
            Messages.Factory.Handlers.Add(new LoginFailedMessage().Type,        LoginFailedHandler.Handle);
            Messages.Factory.Handlers.Add(new ClientCapabilitiesMessage().Type, ClientCapabilitiesHandler.Handle);
            Messages.Factory.Handlers.Add(new KeepAliveMessage().Type,          KeepAliveHandler.Handle);
            Messages.Factory.Handlers.Add(new KeepAliveOkMessage().Type,        KeepAliveOkHandler.Handle);

            Factory.Initialized = true;
        }
    }
}