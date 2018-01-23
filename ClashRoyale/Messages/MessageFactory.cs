namespace ClashRoyale.Messages
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Extensions;
    using ClashRoyale.Messages.Client.Account;
    using ClashRoyale.Messages.Client.Alliance;
    using ClashRoyale.Messages.Client.Attack;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Client.Home;
    using ClashRoyale.Messages.Client.Matchmaking;
    using ClashRoyale.Messages.Client.RoyalTv;
    using ClashRoyale.Messages.Client.Scoring;
    using ClashRoyale.Messages.Client.Socials;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Alliance;
    using ClashRoyale.Messages.Server.Avatar;
    using ClashRoyale.Messages.Server.Home;
    using ClashRoyale.Messages.Server.Matchmaking;
    using ClashRoyale.Messages.Server.RoyalTv;
    using ClashRoyale.Messages.Server.Scoring;
    using ClashRoyale.Messages.Server.Sector;
    using ClashRoyale.Messages.Server.Socials;

    public static class MessageFactory
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MessageFactory"/> has been initialized.
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
        /// The dictionnary of messages, used to route packet ids and decode them.
        /// </summary>
        public static readonly Dictionary<short, Type> Messages = new Dictionary<short, Type>();
        
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (MessageFactory.Initialized)
            {
                return;
            }

            MessageFactory.LoadMessages();

            MessageFactory.Initialized = true;
        }

        /// <summary>
        /// Loads the game messages.
        /// </summary>
        internal static void LoadMessages()
        {
            MessageFactory.Messages.Add(10100, typeof(ClientHelloMessage));
            MessageFactory.Messages.Add(10101, typeof(LoginMessage));
            MessageFactory.Messages.Add(10185, typeof(AskForTvContentMessage));
            MessageFactory.Messages.Add(10517, typeof(InboxOpenedMessage));
            MessageFactory.Messages.Add(10609, typeof(AskForAllianceDataMessage));
            MessageFactory.Messages.Add(10857, typeof(AskForJoinableAlliancesListMessage));
            MessageFactory.Messages.Add(10949, typeof(SearchAllianceMessage));

            MessageFactory.Messages.Add(11033, typeof(CreateAllianceMessage));
            MessageFactory.Messages.Add(11149, typeof(AskForAvatarRankingListMessage));
            MessageFactory.Messages.Add(11639, typeof(AskForAvatarLocalRankingListMessage));
            MessageFactory.Messages.Add(11688, typeof(ClientCapabilitiesMessage));

            MessageFactory.Messages.Add(12269, typeof(CancelMatchmakeMessage));
            MessageFactory.Messages.Add(12393, typeof(StartTrainingBattleMessage));

            MessageFactory.Messages.Add(14171, typeof(AskForAllianceRankingListMessage));
            MessageFactory.Messages.Add(14560, typeof(GoHomeMessage));
            MessageFactory.Messages.Add(14997, typeof(BindGoogleAccountMessage));

            MessageFactory.Messages.Add(15080, typeof(RequestApiMessage));
            MessageFactory.Messages.Add(15793, typeof(AskForFriendsInviteMessage));
            MessageFactory.Messages.Add(15827, typeof(AskForBattleReplayStreamMessage));

            MessageFactory.Messages.Add(17101, typeof(AskForAvatarStreamMessage));

            MessageFactory.Messages.Add(18688, typeof(EndClientTurnMessage));

            MessageFactory.Messages.Add(19860, typeof(VisitHomeMessage));
            MessageFactory.Messages.Add(19863, typeof(ChangeAvatarNameMessage));
            MessageFactory.Messages.Add(19911, typeof(KeepAliveMessage));

            /* --------------------------------------------------- */

            MessageFactory.Messages.Add(20032, typeof(BattleReportStreamMessage));
            MessageFactory.Messages.Add(20073, typeof(RoyalTvContentMessage));
            MessageFactory.Messages.Add(20103, typeof(LoginFailedMessage));
            MessageFactory.Messages.Add(20817, typeof(CancelMatchmakeDoneMessage));
            MessageFactory.Messages.Add(21443, typeof(SectorHearbeatMessage));
            MessageFactory.Messages.Add(21873, typeof(SectorStateMessage));
            MessageFactory.Messages.Add(22089, typeof(FriendsInviteDataMessage));
            MessageFactory.Messages.Add(22280, typeof(LoginOkMessage));
            MessageFactory.Messages.Add(22726, typeof(RequestApiDataMessage));

            MessageFactory.Messages.Add(24135, typeof(KeepAliveOkMessage));
            MessageFactory.Messages.Add(24445, typeof(InboxListMessage));
            MessageFactory.Messages.Add(24457, typeof(AllianceOnlineStatusUpdatedMessage));
            MessageFactory.Messages.Add(24719, typeof(AllianceStreamMessage));

            MessageFactory.Messages.Add(25105, typeof(AllianceRankingListMessage));
            MessageFactory.Messages.Add(25390, typeof(AvatarLocalRankingListMessage));
            MessageFactory.Messages.Add(25412, typeof(HomeBattleReplayDataMessage));
            MessageFactory.Messages.Add(25880, typeof(VisitedHomeDataMessage));

            MessageFactory.Messages.Add(26068, typeof(InboxCountMessage));
            MessageFactory.Messages.Add(26550, typeof(AllianceDataMessage));
            MessageFactory.Messages.Add(26973, typeof(AllianceLocalRankingListMessage));

            MessageFactory.Messages.Add(28502, typeof(OwnHomeDataMessage));
            MessageFactory.Messages.Add(29567, typeof(AvatarStreamMessage));
            MessageFactory.Messages.Add(29733, typeof(AvatarRankingListMessage));

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
        public static Message CreateMessage(short Type, ByteStream Stream)
        {
            if (MessageFactory.Messages.TryGetValue(Type, out Type Message))
            {
                return (Message) Activator.CreateInstance(Message, Stream);
            }

            Logging.Warning(typeof(MessageFactory), "Messages.TryGetValue(" + Type + ", out Message) != true at CreateMessage(" + Type + ", Device, Stream).");

            return null;
        }
    }
}