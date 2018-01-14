namespace ClashRoyale.Handlers.Client.Account
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using ClashRoyale.Crypto;
    using ClashRoyale.Crypto.Nacl;
    using ClashRoyale.Enums;
    using ClashRoyale.Exceptions;
    using ClashRoyale.Files;
    using ClashRoyale.Logic;
    using ClashRoyale.Messages;
    using ClashRoyale.Messages.Client.Account;
    using ClashRoyale.Messages.Server.Account;

    public static class ClientHelloHandler
    {
        /// <summary>
        /// Handles the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Cancellation">The cancellation.</param>
        public static async Task Handle(Device Device, Message Message, CancellationToken Cancellation)
        {
            var ClientHelloMessage = (ClientHelloMessage) Message;

            if (ClientHelloMessage == null)
            {
                throw new LogicException(typeof(ClientHelloHandler), "ClientHelloMessage == null at Handle(Device, Message, CancellationToken).");
            }

            Device.State = State.Session;

            if (ClientHelloMessage.Protocol == 1)
            {
                if (ClientHelloMessage.MajorVersion == Config.ClientMajorVersion && ClientHelloMessage.MinorVersion == 0 && ClientHelloMessage.BuildVersion == Config.ClientBuildVersion)
                {
                    if (string.Equals(ClientHelloMessage.MasterHash, Fingerprint.Masterhash))
                    {
                        if (PepperFactory.SecretKeys.TryGetValue(ClientHelloMessage.KeyVersion, out byte[] SecretKey))
                        {
                            if (ClientHelloMessage.DeviceType == 3)
                            {
                                if (!Config.IsDevelopment)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Redirection)); // Dev Host

                                    return;
                                }
                                else
                                {
                                    if (ClientHelloMessage.KeyVersion != PepperFactory.SecretKeys.Keys.Last())
                                    {
                                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Update));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (Config.IsDevelopment)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Redirection)); // Prod Host

                                    return;
                                }
                            }

                            if (ClientHelloMessage.DeviceType == 30)
                            {
                                if (!Config.IsKunlunServer)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Redirection)); // Kunlun Host

                                    return;
                                }
                            }
                            else
                            {
                                if (Config.IsKunlunServer)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Redirection)); // Prod Host
                                    return;
                                }
                            }

                            if (Program.Initialized)
                            {
                                Device.NetworkManager.PepperInit.KeyVersion = ClientHelloMessage.KeyVersion;
                                Device.NetworkManager.PepperInit.ServerPublicKey = new byte[32];

                                Curve25519Xsalsa20Poly1305.CryptoBoxGetpublickey(Device.NetworkManager.PepperInit.ServerPublicKey, SecretKey);

                                Device.NetworkManager.SendMessage(new ServerHelloMessage());
                            }
                            else
                            {
                                Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Maintenance));
                            }
                        }
                        else
                        {
                            Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.UpdateInProgress));
                        }
                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Patch));
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.Update));
                }
            }
            else
            {
                Device.NetworkManager.SendMessage(new LoginFailedMessage(Reason.UpdateInProgress));
            }
        }
    }
}
