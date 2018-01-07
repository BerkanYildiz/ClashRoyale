namespace ClashRoyale.Handlers.Server
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;

    public static class DefaultHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var DefaultMessage = (Message) Message;

            if (DefaultMessage == null)
            {
                throw new LogicException(typeof(DefaultHandler), nameof(DefaultMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}
