namespace ClashRoyale.Handlers.Client.Avatar
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Commands.Server;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Avatar;
    using ClashRoyale.Messages.Server.Avatar;

    public static class ChangeAvatarNameHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var ChangeAvatarNameMessage = (ChangeAvatarNameMessage) Message;

            if (ChangeAvatarNameMessage == null)
            {
                throw new LogicException(typeof(ChangeAvatarNameHandler), nameof(ChangeAvatarNameMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            if (!Device.GameMode.CommandManager.WaitChangeAvatarNameTurn)
            {
                if (!string.IsNullOrEmpty(ChangeAvatarNameMessage.Username))
                {
                    ChangeAvatarNameMessage.Username = ChangeAvatarNameMessage.Username.Trim();

                    if (ChangeAvatarNameMessage.Username.Length >= 2 && ChangeAvatarNameMessage.Username.Length <= 16)
                    {
                        if (Device.GameMode.Player.IsNameSet)
                        {
                            if (Device.GameMode.Player.NameChangeState > 1)
                            {
                                return;
                            }

                            Device.GameMode.CommandManager.WaitChangeAvatarNameTurn = true;
                            Device.GameMode.CommandManager.AddAvailableServerCommand(new ChangeAvatarNameCommand(ChangeAvatarNameMessage.Username, true, 1));
                        }
                        else
                        {
                            Device.GameMode.CommandManager.WaitChangeAvatarNameTurn = true;
                            Device.GameMode.CommandManager.AddAvailableServerCommand(new ChangeAvatarNameCommand(ChangeAvatarNameMessage.Username, true, 0));
                        }

                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new AvatarNameChangeFailedMessage());
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new AvatarNameChangeFailedMessage());
                }
            }
            else
            {
                Logging.Warning(typeof(ChangeAvatarNameHandler), "WaitChangeAvatarNameTurn == true at Handle(Device, Message, CancellationToken).");
            }
        }
    }
}