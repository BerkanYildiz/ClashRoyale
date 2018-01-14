namespace ClashRoyale.Handlers.Client.Attack
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Attack;

    public static class SendBattleEventHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var SendBattleEventMessage = (SendBattleEventMessage) Message;

            if (SendBattleEventMessage == null)
            {
                throw new LogicException(typeof(SendBattleEventHandler), nameof(SendBattleEventMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (Device.GameMode.State == HomeState.Attack)
            {
                Device.GameMode.SectorManager.ReceiveBattleEvent(SendBattleEventMessage.BattleEvent);
            }
            else
            {
                Logging.Info(typeof(SendBattleEventHandler), "State != HomeState.Attack at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}