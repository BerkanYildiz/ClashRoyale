namespace ClashRoyale.Handlers.Client.Attack
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Attack;

    public static class SectorCommandHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var SectorCommandMessage = (SectorCommandMessage) Message;

            if (SectorCommandMessage == null)
            {
                throw new LogicException(typeof(SectorCommandHandler), nameof(SectorCommandMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (Device.GameMode.State == HomeState.Attack)
            {
                Device.GameMode.SectorManager.ReceiveSectorCommand(SectorCommandMessage.Tick, SectorCommandMessage.Checksum, SectorCommandMessage.Command);
            }
            else
            {
                Logging.Info(typeof(SendBattleEventHandler), "State != HomeState.Attack at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}