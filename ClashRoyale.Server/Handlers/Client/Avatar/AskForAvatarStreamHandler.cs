namespace ClashRoyale.Handlers.Client.Avatar
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Server.Avatar;

    public static class AskForAvatarStreamHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var AskForAvatarStreamMessage = (AskForAvatarStreamMessage) Message;

            if (AskForAvatarStreamMessage == null)
            {
                throw new LogicException(typeof(AskForAvatarStreamHandler), nameof(AskForAvatarStreamMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.NetworkManager.SendMessage(new AvatarStreamMessage());
        }
    }
}