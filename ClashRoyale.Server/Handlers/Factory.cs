namespace ClashRoyale.Handlers
{
    using ClashRoyale.Handlers.Client;
    using ClashRoyale.Handlers.Server;
    using ClashRoyale.Messages.Client;
    using ClashRoyale.Messages.Client.Alliance;
    using ClashRoyale.Messages.Client.Attack;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Client.Matchmaking;
    using ClashRoyale.Messages.Client.Scoring;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Home;

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

            Messages.Factory.Handlers.Add(new ClientHelloMessage().Type,                ClientHelloHandler.Handle);
            Messages.Factory.Handlers.Add(new ServerHelloMessage().Type,                ServerHelloHandler.Handle);

            Messages.Factory.Handlers.Add(new LoginMessage().Type,                      LoginHandler.Handle);
            Messages.Factory.Handlers.Add(new LoginOkMessage().Type,                    LoginOkHandler.Handle);
            Messages.Factory.Handlers.Add(new LoginFailedMessage().Type,                LoginFailedHandler.Handle);

            Messages.Factory.Handlers.Add(new ClientCapabilitiesMessage().Type,         ClientCapabilitiesHandler.Handle);
            Messages.Factory.Handlers.Add(new EndClientTurnMessage().Type,              EndClientTurnHandler.Handle);
            Messages.Factory.Handlers.Add(new ChangeAvatarNameMessage().Type,           ChangeAvatarNameHandler.Handle);

            Messages.Factory.Handlers.Add(new KeepAliveMessage().Type,                  KeepAliveHandler.Handle);
            Messages.Factory.Handlers.Add(new KeepAliveOkMessage().Type,                KeepAliveOkHandler.Handle);

            Messages.Factory.Handlers.Add(new AskForAvatarStreamMessage().Type,         AskForAvatarStreamHandler.Handle);
            Messages.Factory.Handlers.Add(new AskForBattleReplayStreamMessage().Type,   AskForBattleReplayStreamHandler.Handle);

            Messages.Factory.Handlers.Add(new GoHomeMessage().Type,                     GoHomeHandler.Handle);
            Messages.Factory.Handlers.Add(new VisitHomeMessage().Type,                  VisitHomeHandler.Handle);
            Messages.Factory.Handlers.Add(new HomeBattleReplayMessage().Type,           HomeBattleReplayHandler.Handle);
            Messages.Factory.Handlers.Add(new AskForAllianceDataMessage().Type,         AskForAllianceDataHandler.Handle);

            Messages.Factory.Handlers.Add(new CancelMatchmakeMessage().Type,            CancelMatchmakeHandler.Handle);
            Messages.Factory.Handlers.Add(new StartTrainingBattleMessage().Type,        StartTrainingBattleHandler.Handle);
            Messages.Factory.Handlers.Add(new SectorCommandMessage().Type,              SectorCommandHandler.Handle);
            Messages.Factory.Handlers.Add(new SendBattleEventMessage().Type,            SendBattleEventHandler.Handle);

            Messages.Factory.Handlers.Add(new ServerErrorMessage().Type,                ServerErrorHandler.Handle);
            Messages.Factory.Handlers.Add(new ServerShutdownMessage().Type,             ServerShutdownHandler.Handle);
            Messages.Factory.Handlers.Add(new DisconnectedMessage().Type,               DisconnectedHandler.Handle);

            Messages.Factory.Handlers.Add(new AskForAvatarRankingListMessage().Type,    AskForAvatarRankingListHandler.Handle);
            Messages.Factory.Handlers.Add(new AskForAvatarLocalRankingListMessage().Type, AskForAvatarRankingListHandler.Handle);

            Factory.Initialized = true;
        }
    }
}