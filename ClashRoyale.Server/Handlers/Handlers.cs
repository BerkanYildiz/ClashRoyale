namespace ClashRoyale.Handlers
{
    using ClashRoyale.Handlers.Client;
    using ClashRoyale.Handlers.Server;

    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client;
    using ClashRoyale.Messages.Client.Alliance;
    using ClashRoyale.Messages.Client.Attack;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Client.Matchmaking;
    using ClashRoyale.Messages.Client.Scoring;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Home;

    internal static class Handlers
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Handlers"/> has been initialized.
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
            if (Handlers.Initialized)
            {
                return;
            }

            Factory.Handlers.Add(new ClientHelloMessage().Type,                ClientHelloHandler.Handle);
            Factory.Handlers.Add(new ServerHelloMessage().Type,                ServerHelloHandler.Handle);

            Factory.Handlers.Add(new LoginMessage().Type,                      LoginHandler.Handle);
            Factory.Handlers.Add(new LoginOkMessage().Type,                    LoginOkHandler.Handle);
            Factory.Handlers.Add(new LoginFailedMessage().Type,                LoginFailedHandler.Handle);

            Factory.Handlers.Add(new ClientCapabilitiesMessage().Type,         ClientCapabilitiesHandler.Handle);
            Factory.Handlers.Add(new EndClientTurnMessage().Type,              EndClientTurnHandler.Handle);
            Factory.Handlers.Add(new ChangeAvatarNameMessage().Type,           ChangeAvatarNameHandler.Handle);

            Factory.Handlers.Add(new KeepAliveMessage().Type,                  KeepAliveHandler.Handle);
            Factory.Handlers.Add(new KeepAliveOkMessage().Type,                KeepAliveOkHandler.Handle);

            Factory.Handlers.Add(new AskForAvatarStreamMessage().Type,         AskForAvatarStreamHandler.Handle);
            Factory.Handlers.Add(new AskForBattleReplayStreamMessage().Type,   AskForBattleReplayStreamHandler.Handle);

            Factory.Handlers.Add(new GoHomeMessage().Type,                     GoHomeHandler.Handle);
            Factory.Handlers.Add(new VisitHomeMessage().Type,                  VisitHomeHandler.Handle);
            Factory.Handlers.Add(new HomeBattleReplayMessage().Type,           HomeBattleReplayHandler.Handle);
            Factory.Handlers.Add(new AskForAllianceDataMessage().Type,         AskForAllianceDataHandler.Handle);

            Factory.Handlers.Add(new CancelMatchmakeMessage().Type,            CancelMatchmakeHandler.Handle);
            Factory.Handlers.Add(new StartTrainingBattleMessage().Type,        StartTrainingBattleHandler.Handle);
            Factory.Handlers.Add(new SectorCommandMessage().Type,              SectorCommandHandler.Handle);
            Factory.Handlers.Add(new SendBattleEventMessage().Type,            SendBattleEventHandler.Handle);

            Factory.Handlers.Add(new ServerErrorMessage().Type,                ServerErrorHandler.Handle);
            Factory.Handlers.Add(new ServerShutdownMessage().Type,             ServerShutdownHandler.Handle);
            Factory.Handlers.Add(new DisconnectedMessage().Type,               DisconnectedHandler.Handle);

            Factory.Handlers.Add(new AskForAvatarRankingListMessage().Type,    AskForAvatarRankingListHandler.Handle);
            Factory.Handlers.Add(new AskForAvatarLocalRankingListMessage().Type, AskForAvatarRankingListHandler.Handle);

            Handlers.Initialized = true;
        }
    }
}