namespace ClashRoyale.Proxy
{
    using System.Collections.Generic;

    internal class PacketType
    {
        private static Dictionary<int, string> Packets;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            PacketType.Packets = new Dictionary<int, string>();

            PacketType.Packets.Add(10100, "ClientHello");
            PacketType.Packets.Add(10101, "Login");
            PacketType.Packets.Add(10107, "ClientCapabilities");
            PacketType.Packets.Add(10108, "KeepAlive");
            PacketType.Packets.Add(10112, "AuthenticationCheck");
            PacketType.Packets.Add(10113, "SetDeviceToken");
            PacketType.Packets.Add(10116, "ResetAccount");
            PacketType.Packets.Add(10117, "ReportUser");
            PacketType.Packets.Add(10118, "AccountSwitched");
            PacketType.Packets.Add(10121, "UnlockAccount");
            PacketType.Packets.Add(10150, "AppleBillingRequest");
            PacketType.Packets.Add(10151, "GoogleBillingRequest");
            PacketType.Packets.Add(10159, "KunlunBillingRequest");
            PacketType.Packets.Add(10212, "ChangeAvatarName");
            PacketType.Packets.Add(10512, "AskForPlayingGamecenterFriends");
            PacketType.Packets.Add(10513, "AskForPlayingFacebookFriends");
            PacketType.Packets.Add(10905, "InboxOpened");
            PacketType.Packets.Add(12211, "UnbindFacebookAccount");
            PacketType.Packets.Add(12903, "RequestSectorState");
            PacketType.Packets.Add(12904, "SectorCommand");
            PacketType.Packets.Add(12905, "GetCurrentBattleReplayData");
            PacketType.Packets.Add(12951, "SendBattleEvent");
            PacketType.Packets.Add(14101, "GoHome");
            PacketType.Packets.Add(14102, "EndClientTurn");
            PacketType.Packets.Add(14104, "StartMission");
            PacketType.Packets.Add(14105, "HomeLogicStopped");
            PacketType.Packets.Add(14107, "CancelMatchmake");
            PacketType.Packets.Add(14108, "ChangeHomeName");
            PacketType.Packets.Add(14113, "VisitHome");
            PacketType.Packets.Add(14114, "HomeBattleReplay");
            PacketType.Packets.Add(14117, "HomeBattleReplayViewed");
            PacketType.Packets.Add(14120, "AcceptChallenge");
            PacketType.Packets.Add(14123, "CancelChallengeMessage");
            PacketType.Packets.Add(14201, "BindFacebookAccount");
            PacketType.Packets.Add(14212, "BindGamecenterAccount");
            PacketType.Packets.Add(14262, "BindGoogleServiceAccount");
            PacketType.Packets.Add(14301, "CreateAlliance");
            PacketType.Packets.Add(14302, "AskForAllianceData");
            PacketType.Packets.Add(14303, "AskForJoinableAlliancesList");
            PacketType.Packets.Add(14304, "AskForAllianceStream");
            PacketType.Packets.Add(14305, "JoinAlliance");
            PacketType.Packets.Add(14306, "ChangeAllianceMemberRole");
            PacketType.Packets.Add(14307, "KickAllianceMember");
            PacketType.Packets.Add(14308, "LeaveAlliance");
            PacketType.Packets.Add(14310, "DonateAllianceUnit");
            PacketType.Packets.Add(14315, "ChatToAllianceStream");
            PacketType.Packets.Add(14316, "ChangeAllianceSettings");
            PacketType.Packets.Add(14317, "RequestJoinAlliance");
            PacketType.Packets.Add(14318, "SelectSpellsFromCoOpen");
            PacketType.Packets.Add(14319, "OfferChestForCoOpen");
            PacketType.Packets.Add(14321, "RespondToAllianceJoinRequest");
            PacketType.Packets.Add(14322, "SendAllianceInvitation");
            PacketType.Packets.Add(14323, "JoinAllianceUsingInvitation");
            PacketType.Packets.Add(14324, "SearchAlliances");
            PacketType.Packets.Add(14330, "SendAllianceMail");
            PacketType.Packets.Add(14401, "AskForAllianceRankingList");
            PacketType.Packets.Add(14402, "AskForTVContent");
            PacketType.Packets.Add(14403, "AskForAvatarRankingList");
            PacketType.Packets.Add(14404, "AskForAvatarLocalRanking");
            PacketType.Packets.Add(14405, "AskForAvatarStream");
            PacketType.Packets.Add(14406, "AskForBattleReplayStream");
            PacketType.Packets.Add(14408, "AskForLastAvatarTournamentResults");
            PacketType.Packets.Add(14418, "RemoveAvatarStreamEntry");
            PacketType.Packets.Add(14600, "AvatarNameCheckRequest");
            PacketType.Packets.Add(16000, "LogicDeviceLinkCodeStatus");
            PacketType.Packets.Add(16103, "JoinableTournaments");

            /* --------------------------------------- */

            PacketType.Packets.Add(20100, "ServerHello");
            PacketType.Packets.Add(20103, "LoginFailed");
            PacketType.Packets.Add(20104, "LoginOk");
            PacketType.Packets.Add(20105, "FriendList");
            PacketType.Packets.Add(20108, "KeepAliveOk");
            PacketType.Packets.Add(20118, "ChatAccountBanStatus");
            PacketType.Packets.Add(20121, "BillingRequestFailed");
            PacketType.Packets.Add(20132, "UnlockAccountOk");
            PacketType.Packets.Add(20133, "UnlockAccountFailed");
            PacketType.Packets.Add(20151, "AppleBillingProcessedByServer");
            PacketType.Packets.Add(20152, "GoogleBillingProcessedByServer");
            PacketType.Packets.Add(20156, "KunlunBillingProcessedByServer");
            PacketType.Packets.Add(20161, "ShutdownStarted");
            PacketType.Packets.Add(20205, "AvatarNameChangeFailed");
            PacketType.Packets.Add(20206, "AvatarOnlineStatusUpdated");
            PacketType.Packets.Add(20207, "AllianceOnlineStatusUpdated");
            PacketType.Packets.Add(20225, "BattleResult");
            PacketType.Packets.Add(20300, "AvatarNameCheckResponse");
            PacketType.Packets.Add(20801, "OpponentLeftMatchNotification");
            PacketType.Packets.Add(20802, "OpponentRejoinsMatchNotification");
            PacketType.Packets.Add(21902, "SectorHearbeat");
            PacketType.Packets.Add(21903, "SectorState");
            PacketType.Packets.Add(22952, "BattleEvent");
            PacketType.Packets.Add(22957, "PvpMatchmakeNotification");
            PacketType.Packets.Add(24101, "OwnHomeData");
            PacketType.Packets.Add(24102, "OwnAvatarData");
            PacketType.Packets.Add(24104, "OutOfSync");
            PacketType.Packets.Add(24106, "StopHomeLogic");
            PacketType.Packets.Add(24107, "MatchmakeInfo");
            PacketType.Packets.Add(24108, "MatchmakeFailed");
            PacketType.Packets.Add(24111, "AvailableServerCommand");
            PacketType.Packets.Add(24112, "UdpConnectionInfo");
            PacketType.Packets.Add(24113, "VisitedHomeData");
            PacketType.Packets.Add(24114, "HomeBattleReplay");
            PacketType.Packets.Add(24115, "ServerError");
            PacketType.Packets.Add(24116, "HomeBattleReplayFailed");
            PacketType.Packets.Add(24121, "ChallengeFailed");
            PacketType.Packets.Add(24124, "CancelChallengeDone");
            PacketType.Packets.Add(24125, "CancelMatchmakeDone");
            PacketType.Packets.Add(24201, "FacebookAccountBound");
            PacketType.Packets.Add(24202, "FacebookAccountAlreadyBound");
            PacketType.Packets.Add(24212, "GamecenterAccountAlreadyBound");
            PacketType.Packets.Add(24213, "FacebookAccountUnbound");
            PacketType.Packets.Add(24261, "GoogleServiceAccountBound");
            PacketType.Packets.Add(24262, "GoogleServiceAccountAlreadyBound");
            PacketType.Packets.Add(24301, "AllianceData");
            PacketType.Packets.Add(24302, "AllianceJoinFailed");
            PacketType.Packets.Add(24303, "AllianceJoinOk");
            PacketType.Packets.Add(24304, "JoinableAllianceList");
            PacketType.Packets.Add(24305, "AllianceLeaveOk");
            PacketType.Packets.Add(24306, "ChangeAllianceMemberRoleOk");
            PacketType.Packets.Add(24307, "KickAllianceMemberOk");
            PacketType.Packets.Add(24308, "AllianceMember");
            PacketType.Packets.Add(24309, "AllianceMemberRemoved");
            PacketType.Packets.Add(24310, "AllianceList");
            PacketType.Packets.Add(24311, "AllianceStream");
            PacketType.Packets.Add(24312, "AllianceStreamEntry");
            PacketType.Packets.Add(24318, "AllianceStreamEntryRemoved");
            PacketType.Packets.Add(24319, "AllianceJoinRequestOk");
            PacketType.Packets.Add(24320, "AllianceJoinRequestFailed");
            PacketType.Packets.Add(24321, "AllianceInvitationSendFailed");
            PacketType.Packets.Add(24322, "AllianceInvitationSentOk");
            PacketType.Packets.Add(24324, "AllianceFullEntryUpdate");
            PacketType.Packets.Add(24332, "AllianceCreateFailed");
            PacketType.Packets.Add(24333, "AllianceChangeFailed");
            PacketType.Packets.Add(24401, "AllianceRankingList");
            PacketType.Packets.Add(24402, "AllianceLocalRankingList");
            PacketType.Packets.Add(24403, "AvatarRankingList");
            PacketType.Packets.Add(24404, "AvatarLocalRankingList");
            PacketType.Packets.Add(24405, "RoyalTVContent");
            PacketType.Packets.Add(24407, "LastAvatarTournamentResults");
            PacketType.Packets.Add(24411, "AvatarStream");
            PacketType.Packets.Add(24412, "AvatarStreamEntry");
            PacketType.Packets.Add(24413, "BattleReportStream");
            PacketType.Packets.Add(24418, "AvatarStreamEntryRemoved");
            PacketType.Packets.Add(24445, "InboxList");
            PacketType.Packets.Add(24447, "InboxCount");
            PacketType.Packets.Add(25892, "Disconnected");
            PacketType.Packets.Add(26002, "LogicDeviceLinkCodeResponse");
            PacketType.Packets.Add(26003, "LogicDeviceLinkNewDeviceLinked");
            PacketType.Packets.Add(26004, "LogicDeviceLinkCodeDeactivated");
            PacketType.Packets.Add(26005, "LogicDeviceLinkResponse");
            PacketType.Packets.Add(26007, "LogicDeviceLinkDone");
            PacketType.Packets.Add(26008, "LogicDeviceLinkError");

            PacketType.Packets.Add(26108, "JoinableTournamentsData");

            // NEW

            PacketType.Packets.Add(10609, "AskClan");
            PacketType.Packets.Add(10949, "SearchClans");
            PacketType.Packets.Add(11688, "ClientCapabilities");
            PacketType.Packets.Add(18688, "ExecuteCommands");
            PacketType.Packets.Add(19860, "AskProfile");
            PacketType.Packets.Add(19911, "KeepAlive");

            PacketType.Packets.Add(22280, "LoginOk");
            PacketType.Packets.Add(22534, "SearchClansData");
            PacketType.Packets.Add(24135, "KeepAliveOk");
            PacketType.Packets.Add(26550, "ClanData");
            
        }

        /// <summary>
        /// Gets the packet name according to the ID
        /// </summary>
        public static string GetName(int MessageID)
        {
            string PacketName = "Unknown";

            if (PacketType.Packets.ContainsKey(MessageID))
            {
                PacketType.Packets.TryGetValue(MessageID, out PacketName);
            }

            return PacketName;
        }
    }
}