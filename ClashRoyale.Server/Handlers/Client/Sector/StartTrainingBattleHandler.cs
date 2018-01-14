namespace ClashRoyale.Handlers.Client.Sector
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Attack;

    public static class StartTrainingBattleHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var StartTrainingBattleMessage = (StartTrainingBattleMessage) Message;

            if (StartTrainingBattleMessage == null)
            {
                throw new LogicException(typeof(StartTrainingBattleHandler), nameof(StartTrainingBattleMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (Device.GameMode.State <= HomeState.Home)
            {
                Device.GameMode.SectorManager.SendSectorState();
            }
            else
            {
                Logging.Info(typeof(StartTrainingBattleHandler), "State > HomeState.Home at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}