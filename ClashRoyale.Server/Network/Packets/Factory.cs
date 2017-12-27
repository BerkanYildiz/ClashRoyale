namespace ClashRoyale.Server.Network.Packets
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Network.Packets.Client;
    using ClashRoyale.Server.Network.Packets.Server;

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
            Factory.Messages.Add(10100, typeof(ClientHelloMessage));
            Factory.Messages.Add(10101, typeof(LoginMessage));

            Factory.Messages.Add(10185, typeof(AskForTvContentMessage));
            Factory.Messages.Add(10517, typeof(InboxOpenedMessage));
            Factory.Messages.Add(10857, typeof(AskForJoinableAlliancesListMessage));
            Factory.Messages.Add(10949, typeof(SearchAllianceMessage));

            Factory.Messages.Add(11149, typeof(AskForAvatarRankingListMessage));
            Factory.Messages.Add(11639, typeof(AskForAvatarLocalRankingListMessage));
            Factory.Messages.Add(11688, typeof(ClientCapabilitiesMessage));
            Factory.Messages.Add(12269, typeof(CancelMatchmakeMessage));
            // Factory.Messages.Add(12393, typeof(StartTrainingBattleMessage));
            Factory.Messages.Add(14171, typeof(AskForAllianceRankingListMessage));
            Factory.Messages.Add(14560, typeof(GoHomeMessage));

            Factory.Messages.Add(15080, typeof(RequestApiMessage));

            Factory.Messages.Add(18688, typeof(EndClientTurnMessage));
            Factory.Messages.Add(19860, typeof(VisitHomeMessage));
            Factory.Messages.Add(19911, typeof(KeepAliveMessage));

            /* --------------------------------------------------- */

            Factory.Messages.Add(20103, typeof(AuthentificationFailedMessage));
            Factory.Messages.Add(20817, typeof(CancelMatchmakeDoneMessage));
            Factory.Messages.Add(21443, typeof(SectorHearbeatMessage));
            Factory.Messages.Add(21873, typeof(SectorStateMessage));
            Factory.Messages.Add(22280, typeof(AuthentificationOkMessage));
            Factory.Messages.Add(22726, typeof(RequestApiDataMessage));
            Factory.Messages.Add(24135, typeof(KeepAliveServerMessage));
            Factory.Messages.Add(24445, typeof(InboxListMessage));
            Factory.Messages.Add(25105, typeof(AllianceRankingListMessage));
            Factory.Messages.Add(25390, typeof(AvatarLocaleRankingListMessage));
            Factory.Messages.Add(25412, typeof(HomeBattleReplayDataMessage));
            Factory.Messages.Add(25880, typeof(VisitedHomeDataMessage));
            Factory.Messages.Add(26550, typeof(AllianceDataMessage));
            Factory.Messages.Add(26973, typeof(AllianceLocaleRankingListMessage));
            Factory.Messages.Add(28502, typeof(OwnHomeDataMessage));
            Factory.Messages.Add(29733, typeof(AvatarRankingListMessage));

            /* Factory.Messages.Add(10113, typeof(SetDeviceTokenMessage));
            Factory.Messages.Add(10119, typeof(ReportUserMessage));
            Factory.Messages.Add(10212, typeof(ChangeAvatarNameMessage));
            Factory.Messages.Add(10503, typeof(AskForFriendsInviteMessage));
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
            Factory.Messages.Add(14262, typeof(BindGoogleAccount));

            Factory.Messages.Add(14301, typeof(CreateAllianceMessage));
            Factory.Messages.Add(14302, typeof(AskForAllianceDataMessage));
            
            // Factory.Messages.Add(14304, typeof(Kick));
            Factory.Messages.Add(14305, typeof(JoinAllianceMessage));
            Factory.Messages.Add(14308, typeof(LeaveAllianceMessage));
            Factory.Messages.Add(14315, typeof(ChatToAllianceStreamMessage));
            Factory.Messages.Add(14316, typeof(EditAllianceMessage));
            Factory.Messages.Add(14321, typeof(ReplyJoinRequestMessage));

            Factory.Messages.Add(14405, typeof(AskForAvatarStreamMessage));
            Factory.Messages.Add(14406, typeof(AskForBattleReplayStreamMessage));

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
    }
}