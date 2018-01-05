namespace ClashRoyale.Server.Handlers.Client
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Files;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client;
    using ClashRoyale.Messages.Server.Account;
    using ClashRoyale.Messages.Server.Home;

    public static class LoginHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            Logging.Info(typeof(LoginHandler), "Currently handling LoginMessage.");

            var LoginMessage = (LoginMessage) Message;

            if (LoginMessage == null)
            {
                throw new LogicException(typeof(LoginHandler), "LoginMessage == null at Handle(Device, Message, CancellationToken).");
            }

            Logging.Info(typeof(LoginHandler), "Account Id    : " + LoginMessage.HighId + "-" + LoginMessage.LowId + ".");
            Logging.Info(typeof(LoginHandler), "Account Token : " + LoginMessage.Token + ".");

            if (Trusted(Device, LoginMessage) == false)
            {
                return;
            }

            if (LoginMessage.HighId == 0 && LoginMessage.LowId == 0 && string.IsNullOrEmpty(LoginMessage.Token))
            {
                Player Player = await Players.Create();

                if (Player != null)
                {
                    await Login(Device, LoginMessage, Player);
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Maintenance));
                }
            }
            else
            {
                Player Player = await Players.Get(LoginMessage.HighId, LoginMessage.LowId);

                if (Player != null)
                {
                    if (string.Equals(LoginMessage.Token, Player.Token))
                    {
                        if (!Player.IsBanned)
                        {
                            if (Player.IsConnected)
                            {
                                Player.GameMode.Device.NetworkManager.SendMessage(new DisconnectedMessage(Player.GameMode.Device));
                            }

                            await Login(Device, LoginMessage, Player);
                        }
                        else
                        {
                            Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Banned));
                        }
                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Reset));
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Reset));
                }
            }
        }

        /// <summary>
        /// Returns whether we should handle this device or nah.
        /// </summary>
        private static bool Trusted(Device Device, LoginMessage Message)
        {
            if (Message.HighId < 0 || Message.LowId < 0)
            {
                throw new LogicException(typeof(LoginMessage), "HighId or LowId is inferior to zero.");
            }
            else
            {
                if (Message.MajorVersion == Config.ClientMajorVersion && Message.MinorVersion == 0 && Message.BuildVersion == Config.ClientBuildVersion)
                {
                    if (Program.Initialized)
                    {
                        if (string.Equals(Message.MasterHash, Fingerprint.Masterhash))
                        {
                            return true;
                        }

                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Patch));
                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Maintenance));
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Update));
                }
            }

            return false;
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        private static async Task Login(Device Device, LoginMessage Message, Player Player)
        {
            if (Player.AccountLocation == null)
            {
                Player.AccountLocation = Message.Locale;
            }

            Device.GameMode = new GameMode(Device);
            Device.GameMode.SetPlayer(Player);
            Device.NetworkManager.AccountId = new LogicLong(Player.HighId, Player.LowId);

            Device.NetworkManager.SendMessage(new LoginOkMessage(Device, Player));
            Device.NetworkManager.SendMessage(new OwnHomeDataMessage(Device, Player));
            Device.NetworkManager.SendMessage(new InboxCountMessage(Device));
        }
    }
}
