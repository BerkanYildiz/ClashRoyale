namespace ClashRoyale.Handlers.Client.Account
{
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Extensions.Utils;
    using ClashRoyale.Files;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Inbox;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Maths;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;
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
            var LoginMessage = (LoginMessage) Message;

            if (LoginMessage == null)
            {
                throw new LogicException(typeof(LoginHandler), nameof(LoginMessage) + " == null at Handle(Device, Message, CancellationToken).");
            }

            Device.State = State.Login;

            if (LoginHandler.Trusted(Device, LoginMessage) == false)
            {
                return;
            }

            Logging.Info(typeof(LoginHandler), "Login(" + LoginMessage.HighId + "-" + LoginMessage.LowId + ").");

            if (LoginMessage.HighId == 0 && LoginMessage.LowId == 0 && string.IsNullOrEmpty(LoginMessage.Token))
            {
                Player Player = await Players.Create();

                if (Player != null)
                {
                    await LoginHandler.Login(Device, LoginMessage, Player);
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Maintenance));
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
                            await LoginHandler.Login(Device, LoginMessage, Player);
                        }
                        else
                        {
                            Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Banned));
                        }
                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Reset));
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Reset));
                }
            }
        }

        /// <summary>
        /// Returns whether we should handle this device or nah.
        /// </summary>
        private static bool Trusted(Device Device, LoginMessage Message)
        {
            Logging.Info(typeof(LoginHandler), "Trusted().");

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

                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Patch));
                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Maintenance));
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Update));
                }
            }

            return false;
        }

        /// <summary>
        /// Logins this instance.
        /// </summary>
        private static async Task Login(Device Device, LoginMessage Message, Player Player)
        {
            if (Player.IsConnected)
            {
                Player.GameMode.Listener.SendMessage(new DisconnectedMessage());
                Player.GameMode.Listener.Disconnect();
            }

            if (Player.AccountLocation == null)
            {
                Player.AccountLocation = Message.Locale;
            }

            Device.GameMode = new GameMode
            {
                Listener = Device.GameListener
            };

            Device.GameMode.SetPlayer(Player);

            Device.NetworkManager.AccountId = new LogicLong(Player.HighId, Player.LowId);
            
            Device.Defines.Region       = Message.Region;
            Device.Defines.OpenUdid     = Message.OpenUdid;
            Device.Defines.MacAddress   = Message.MacAddress;
            Device.Defines.Model        = Message.Model;
            Device.Defines.AdvertiseId  = Message.AdvertiseId;
            Device.Defines.AndroidId    = Message.AndroidId;
            Device.Defines.OsVersion    = Message.OsVersion;
            Device.Defines.Android      = Message.IsAndroid;

            Device.NetworkManager.SendMessage(new LoginOkMessage(Player));
            Device.NetworkManager.SendMessage(new OwnHomeDataMessage(Player, Player.Home, TimeUtil.Timestamp));

            if (InboxManager.Entries.Count > 0)
            {
                Device.NetworkManager.SendMessage(new InboxCountMessage
                {
                    InboxNewMessageCnt = InboxManager.Entries.Count
                });
            }
        }
    }
}
