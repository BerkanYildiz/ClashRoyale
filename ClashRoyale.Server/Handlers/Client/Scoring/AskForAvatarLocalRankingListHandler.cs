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

    public static class AskForAvatarLocalRankingListHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var AskForAvatarLocalRankingListMessage = (AskForAvatarLocalRankingListMessage) Message;

            if (AskForAvatarLocalRankingListMessage == null)
            {
                throw new LogicException(typeof(AskForAvatarLocalRankingListHandler), nameof(AskForAvatarLocalRankingListMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            LeaderboardPlayers Leaderboard = Leaderboards.GetRegionalPlayers(Device.Defines.Region);

            if (Leaderboard != null)
            {
                Device.NetworkManager.SendMessage(new AvatarLocalRankingListMessage()
                {
                    Entries = Leaderboard.Players.ToArray(),
                    LastSeasonEntries = Leaderboard.LastSeason.ToArray()
                });
            }
            else
            {
                Logging.Error(typeof(AskForAvatarLocalRankingListHandler), "Leaderboard == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}
