namespace ClashRoyale.Handlers.Client.Avatar
{
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.RoyalTv;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Server.Avatar;

    public static class HomeBattleReplayHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var HomeBattleReplayMessage = (HomeBattleReplayMessage) Message;

            if (HomeBattleReplayMessage == null)
            {
                throw new LogicException(typeof(HomeBattleReplayHandler), nameof(HomeBattleReplayMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (!HomeBattleReplayMessage.ReplayId.IsZero)
            {
                BattleLog BattleLog = await Battles.Get(HomeBattleReplayMessage.ReplayId.HigherInt, HomeBattleReplayMessage.ReplayId.LowerInt);

                if (BattleLog != null)
                {
                    int ChannelIdx = RoyalTvManager.GetChannelArenaData(HomeBattleReplayMessage.ArenaData);

                    if (ChannelIdx != -1)
                    {
                        RoyalTvEntry TvEntry = RoyalTvManager.GetEntryByIdx(ChannelIdx, HomeBattleReplayMessage.ReplayShardId);

                        if (TvEntry != null)
                        {
                            Interlocked.Increment(ref TvEntry.ViewCount);
                        }
                    }

                    byte[] Decompressed = Encoding.UTF8.GetBytes(BattleLog.ReplayJson);
                    byte[] Compressed   = ZLibHelper.CompressCompressableByteArray(Decompressed);

                    Device.NetworkManager.SendMessage(new HomeBattleReplayDataMessage(Compressed));
                }
                else
                {
                    Logging.Info(typeof(HomeBattleReplayHandler), "BattleLog == null at Handle(Device, Message, CancellationToken).");
                }
            }
            else
            {
                Logging.Info(typeof(HomeBattleReplayHandler), "ReplayId.IsZero == true at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}