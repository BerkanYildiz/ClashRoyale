namespace ClashRoyale.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;

    using ClashRoyale.Messages.Client;
    using ClashRoyale.Messages.Client.Alliance;
    using ClashRoyale.Messages.Client.Attack;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Client.Matchmaking;
    using ClashRoyale.Messages.Client.RoyalTv;
    using ClashRoyale.Messages.Client.Scoring;
    using ClashRoyale.Messages.Client.Socials;
    using ClashRoyale.Messages.Client.Socials.Bind;

    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Alliance;
    using ClashRoyale.Messages.Server.Avatar;
    using ClashRoyale.Messages.Server.Home;
    using ClashRoyale.Messages.Server.Matchmaking;
    using ClashRoyale.Messages.Server.RoyalTv;
    using ClashRoyale.Messages.Server.Scoring;
    using ClashRoyale.Messages.Server.Sector;
    using ClashRoyale.Messages.Server.Socials;

    public static class Factory
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Factory"/> has been initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// The delimiter used to detect if x string is a call-command.
        /// </summary>
        public const char Delimiter = '/';

        /// <summary>
        /// The message handler, used to process the received and sent messages.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation token.</param>
        public delegate Task MessageHandler(Device Device, Message Message, CancellationToken Cancellation);

        /// <summary>
        /// The dictionnary of messages, used to route packet ids and decode them.
        /// </summary>
        public static readonly Dictionary<short, Type> Messages = new Dictionary<short, Type>();

        /// <summary>
        /// The dictionnary of handlers, used to route packet ids and handle them.
        /// </summary>
        public static readonly Dictionary<short, MessageHandler> Handlers = new Dictionary<short, MessageHandler>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (Factory.Initialized)
            {
                return;
            }

            Factory.LoadMessages();

            Factory.Initialized = true;
        }

        /// <summary>
        /// Loads the game messages.
        /// </summary>
        internal static void LoadMessages()
        {
            Factory.Messages.Add(10100, typeof(ClientHelloMessage));
            Factory.Messages.Add(10101, typeof(LoginMessage));
            Factory.Messages.Add(10185, typeof(AskForTvContentMessage));
            Factory.Messages.Add(10517, typeof(InboxOpenedMessage));
            Factory.Messages.Add(10609, typeof(AskForAllianceDataMessage));
            Factory.Messages.Add(10857, typeof(AskForJoinableAlliancesListMessage));
            Factory.Messages.Add(10949, typeof(SearchAllianceMessage));

            Factory.Messages.Add(11149, typeof(AskForAvatarRankingListMessage));
            Factory.Messages.Add(11639, typeof(AskForAvatarLocalRankingListMessage));
            Factory.Messages.Add(11688, typeof(ClientCapabilitiesMessage));

            Factory.Messages.Add(12269, typeof(CancelMatchmakeMessage));
            Factory.Messages.Add(12393, typeof(StartTrainingBattleMessage));

            Factory.Messages.Add(14171, typeof(AskForAllianceRankingListMessage));
            Factory.Messages.Add(14560, typeof(GoHomeMessage));
            Factory.Messages.Add(14997, typeof(BindGoogleAccountMessage));

            Factory.Messages.Add(15080, typeof(RequestApiMessage));
            Factory.Messages.Add(15793, typeof(AskForFriendsInviteMessage));
            Factory.Messages.Add(15827, typeof(AskForBattleReplayStreamMessage));

            Factory.Messages.Add(17101, typeof(AskForAvatarStreamMessage));

            Factory.Messages.Add(18688, typeof(EndClientTurnMessage));

            Factory.Messages.Add(19860, typeof(VisitHomeMessage));
            Factory.Messages.Add(19863, typeof(ChangeAvatarNameMessage));
            Factory.Messages.Add(19911, typeof(KeepAliveMessage));

            /* --------------------------------------------------- */

            Factory.Messages.Add(20032, typeof(BattleReportStreamMessage));
            Factory.Messages.Add(20073, typeof(RoyalTvContentMessage));
            Factory.Messages.Add(20103, typeof(LoginFailedMessage));
            Factory.Messages.Add(20817, typeof(CancelMatchmakeDoneMessage));
            Factory.Messages.Add(21443, typeof(SectorHearbeatMessage));
            Factory.Messages.Add(21873, typeof(SectorStateMessage));
            Factory.Messages.Add(22089, typeof(FriendsInviteDataMessage));
            Factory.Messages.Add(22280, typeof(LoginOkMessage));
            Factory.Messages.Add(22726, typeof(RequestApiDataMessage));

            Factory.Messages.Add(24135, typeof(KeepAliveOkMessage));
            Factory.Messages.Add(24445, typeof(InboxListMessage));
            Factory.Messages.Add(24457, typeof(AllianceOnlineStatusUpdatedMessage));
            Factory.Messages.Add(24719, typeof(AllianceStreamMessage));

            Factory.Messages.Add(25105, typeof(AllianceRankingListMessage));
            Factory.Messages.Add(25390, typeof(AvatarLocalRankingListMessage));
            Factory.Messages.Add(25412, typeof(HomeBattleReplayDataMessage));
            Factory.Messages.Add(25880, typeof(VisitedHomeDataMessage));

            Factory.Messages.Add(26068, typeof(InboxCountMessage));
            Factory.Messages.Add(26550, typeof(AllianceDataMessage));
            Factory.Messages.Add(26973, typeof(AllianceLocalRankingListMessage));

            Factory.Messages.Add(28502, typeof(OwnHomeDataMessage));
            Factory.Messages.Add(29567, typeof(AvatarStreamMessage));
            Factory.Messages.Add(29733, typeof(AvatarRankingListMessage));

            /* Factory.Messages.Add(10113, typeof(SetDeviceTokenMessage));
            Factory.Messages.Add(10119, typeof(ReportUserMessage));
            Factory.Messages.Add(10212, typeof(ChangeAvatarNameMessage));
            
            Factory.Messages.Add(10504, typeof(AskForPlayingInvitedFriendsListMessage));
            Factory.Messages.Add(10512, typeof(AskForPlayingGamecenterFriendsMessage));
            Factory.Messages.Add(10513, typeof(AskForPlayingFacebookFriendsMessage));

            Factory.Messages.Add(12903, typeof(RequestSectorStateMessage));
            Factory.Messages.Add(12904, typeof(SectorCommandMessage));
            Factory.Messages.Add(12951, typeof(SendBattleEventMessage));

            Factory.Messages.Add(14101, typeof(GoHomeMessage));
            
            Factory.Messages.Add(14107, typeof(CancelMatchmakeMessage));
            
            Factory.Messages.Add(14114, typeof(HomeBattleReplayMessage));

            Factory.Messages.Add(14201, typeof(BindFacebookAccount));
            Factory.Messages.Add(14212, typeof(BindGamecenterAccount));
            

            Factory.Messages.Add(14301, typeof(CreateAllianceMessage));
            
            
            // Factory.Messages.Add(14304, typeof(Kick));
            Factory.Messages.Add(14305, typeof(JoinAllianceMessage));
            Factory.Messages.Add(14308, typeof(LeaveAllianceMessage));
            Factory.Messages.Add(14315, typeof(ChatToAllianceStreamMessage));
            Factory.Messages.Add(14316, typeof(EditAllianceMessage));
            Factory.Messages.Add(14321, typeof(ReplyJoinRequestMessage));

            Factory.Messages.Add(14600, typeof(AvatarNameCheckRequestMessage));

            Factory.Messages.Add(16103, typeof(AskForJoinableTournamentMessage)); */
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

            Logging.Warning(typeof(Factory), "Messages.TryGetValue(" + Type + ", out Message) != true at CreateMessage(" + Type + ", Device, Stream).");

            return null;
        }

        /// <summary>
        /// Handles the specified <see cref="Message"/> using the specified <see cref="Device"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        internal static async Task MessageHandle(Device Device, Message Message)
        {
            using (var Cancellation = new CancellationTokenSource())
            {
                var Token = Cancellation.Token;

                if (Factory.Handlers.TryGetValue(Message.Type, out MessageHandler Handler))
                {
                    Cancellation.CancelAfter(4000);

                    try
                    {
                        await Handler(Device, Message, Token);
                    }
                    catch (OperationCanceledException)
                    {
                        Logging.Warning(typeof(Factory), "Operation has been cancelled after 4 seconds.");
                    }
                }
            }
        }
    }
}