namespace ClashRoyale.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;

    using ClashRoyale.Handlers.Client.Account;
    using ClashRoyale.Handlers.Client.Alliance;
    using ClashRoyale.Handlers.Client.Attack;
    using ClashRoyale.Handlers.Client.Avatar;
    using ClashRoyale.Handlers.Client.Home;
    using ClashRoyale.Handlers.Client.Matchmaking;
    using ClashRoyale.Handlers.Client.RoyalTv;
    using ClashRoyale.Handlers.Client.Scoring;
    using ClashRoyale.Handlers.Client.Sector;
    using ClashRoyale.Handlers.Server.Account;
    using ClashRoyale.Handlers.Server.Home;

    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;
    using ClashRoyale.Messages.Client.Alliance;
    using ClashRoyale.Messages.Client.Attack;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Client.Matchmaking;
    using ClashRoyale.Messages.Client.RoyalTv;
    using ClashRoyale.Messages.Client.Scoring;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Home;

    internal static class HandlerFactory
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
        internal static void Initialize()
        {
            if (HandlerFactory.Initialized)
            {
                return;
            }

            HandlerFactory.MessageHandlers.Add(new ClientHelloMessage().Type,                   ClientHelloHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new ServerHelloMessage().Type,                   ServerHelloHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new LoginMessage().Type,                         LoginHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new LoginOkMessage().Type,                       LoginOkHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new LoginFailedMessage().Type,                   LoginFailedHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new ClientCapabilitiesMessage().Type,            ClientCapabilitiesHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new EndClientTurnMessage().Type,                 EndClientTurnHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new ChangeAvatarNameMessage().Type,              ChangeAvatarNameHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new KeepAliveMessage().Type,                     KeepAliveHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new KeepAliveOkMessage().Type,                   KeepAliveOkHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new AskForAvatarStreamMessage().Type,            AskForAvatarStreamHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new AskForBattleReplayStreamMessage().Type,      AskForBattleReplayStreamHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new OwnHomeDataMessage().Type,                   OwnHomeDataHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new GoHomeMessage().Type,                        GoHomeHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new VisitHomeMessage().Type,                     VisitHomeHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new HomeBattleReplayMessage().Type,              HomeBattleReplayHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new AskForAllianceDataMessage().Type,            AskForAllianceDataHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new CancelMatchmakeMessage().Type,               CancelMatchmakeHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new StartTrainingBattleMessage().Type,           StartTrainingBattleHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new SectorCommandMessage().Type,                 SectorCommandHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new SendBattleEventMessage().Type,               SendBattleEventHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new AskForTvContentMessage().Type,               AskForTvContentHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new ServerErrorMessage().Type,                   ServerErrorHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new ServerShutdownMessage().Type,                ServerShutdownHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new DisconnectedMessage().Type,                  DisconnectedHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new AskForAvatarRankingListMessage().Type,       AskForAvatarRankingListHandler.Handle);
            HandlerFactory.MessageHandlers.Add(new AskForAvatarLocalRankingListMessage().Type,  AskForAvatarLocalRankingListHandler.Handle);

            HandlerFactory.MessageHandlers.Add(new CreateAllianceMessage().Type,                CreateAllianceHandler.Handle);

            HandlerFactory.Initialized = true;
        }

        /// <summary>
        /// Handles the specified <see cref="Message"/> using the specified <see cref="Device"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        public static async Task<bool> MessageHandle(Device Device, Message Message)
        {
            using (var Cancellation = new CancellationTokenSource())
            {
                var Token = Cancellation.Token;

                if (HandlerFactory.MessageHandlers.TryGetValue(Message.Type, out MessageHandler Handler))
                {
                    Cancellation.CancelAfter(4000);

                    try
                    {
                        await Handler(Device, Message, Token);
                    }
                    catch (LogicException)
                    {
                        // Handled.
                    }
                    catch (OperationCanceledException)
                    {
                        Logging.Warning(typeof(MessageHandler), "Operation has been cancelled after 4 seconds, while processing " + Message.GetType().Name + ".");
                    }
                    catch (Exception Exception)
                    {
                        Logging.Error(typeof(MessageHandler), "Operation has been aborted because of a " + Exception.GetType().Name + ", while processing " + Message.GetType().Name + ".");
                    }

                    if (Cancellation.IsCancellationRequested == false)
                    {
                        /* if (Device.GameMode != null)
                        {
                            if (Device.GameMode.Player != null)
                            {
                                await Players.Save(Device.GameMode.Player);
                            }
                        } */

                        return true;
                    }
                    else
                    {
                        Logging.Warning(typeof(MessageHandler), "Operation has been cancelled after processing " + Message.GetType().Name + ".");
                    }
                }
            }

            return false;
        }
    }
}