namespace ClashRoyale.Handlers.Client.Scoring
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Scoring;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Scoring;
    using ClashRoyale.Messages.Server.Scoring;

    public static class AskForAvatarRankingListHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var AskForAvatarRankingListMessage = (AskForAvatarRankingListMessage) Message;

            if (AskForAvatarRankingListMessage == null)
            {
                throw new LogicException(typeof(AskForAvatarRankingListHandler), nameof(AskForAvatarRankingListMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            LeaderboardPlayers Leaderboard = Leaderboards.GlobalPlayers;

            if (Leaderboard != null)
            {
                Device.NetworkManager.SendMessage(new AvatarRankingListMessage()
                {
                    Entries = Leaderboard.Players.ToArray(),
                    LastSeasonEntries = Leaderboard.LastSeason.ToArray()
                });
            }
            else
            {
                Logging.Error(typeof(AskForAvatarRankingListHandler), "Leaderboard == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}
