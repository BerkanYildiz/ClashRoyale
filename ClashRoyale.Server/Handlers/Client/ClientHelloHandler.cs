namespace ClashRoyale.Server.Handlers.Client
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
    using ClashRoyale.Messages.Client;
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
            var ClientHello = (ClientHelloMessage) Message;

            if (ClientHello == null)
            {
                throw new LogicException(typeof(ClientHelloHandler), "ClientHello == null at Handle(Device, Message, CancellationToken).");
            }

            if (ClientHello.Protocol == 1)
            {
                if (ClientHello.MajorVersion == Config.ClientMajorVersion && ClientHello.MinorVersion == 0 && ClientHello.BuildVersion == Config.ClientBuildVersion)
                {
                    if (string.Equals(ClientHello.MasterHash, Fingerprint.Masterhash))
                    {
                        if (PepperFactory.SecretKeys.TryGetValue(ClientHello.KeyVersion, out byte[] SecretKey))
                        {
                            if (ClientHello.DeviceType == 3)
                            {
                                if (!Config.IsDevelopment)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Redirection)); // Dev Host

                                    return;
                                }
                                else
                                {
                                    if (ClientHello.KeyVersion != PepperFactory.SecretKeys.Keys.Last())
                                    {
                                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Update));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (Config.IsDevelopment)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Redirection)); // Prod Host

                                    return;
                                }
                            }

                            if (ClientHello.DeviceType == 30)
                            {
                                if (!Config.IsKunlunServer)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Redirection)); // Kunlun Host

                                    return;
                                }
                            }
                            else
                            {
                                if (Config.IsKunlunServer)
                                {
                                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Redirection)); // Prod Host
                                    return;
                                }
                            }

                            if (Program.Initialized)
                            {
                                Device.NetworkManager.PepperInit.KeyVersion = ClientHello.KeyVersion;
                                Device.NetworkManager.PepperInit.ServerPublicKey = new byte[32];

                                Curve25519Xsalsa20Poly1305.CryptoBoxGetpublickey(Device.NetworkManager.PepperInit.ServerPublicKey, SecretKey);

                                Device.NetworkManager.SendMessage(new ServerHelloMessage(Device));
                            }
                            else
                            {
                                Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Maintenance));
                            }
                        }
                        else
                        {
                            Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.UpdateInProgress));
                        }
                    }
                    else
                    {
                        Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Patch));
                    }
                }
                else
                {
                    Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.Update));
                }
            }
            else
            {
                Device.NetworkManager.SendMessage(new LoginFailedMessage(Device, Reason.UpdateInProgress));
            }
        }
    }
}
