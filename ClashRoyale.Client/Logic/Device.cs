namespace ClashRoyale.Client.Logic
{
    using System;
    using System.Diagnostics;
    using System.Net.Sockets;

    using ClashRoyale.Client.Core.Network;
    using ClashRoyale.Client.Logic.Enums;
    using ClashRoyale.Client.Packets;
    using ClashRoyale.Client.Packets.Crypto;
    using ClashRoyale.Client.Packets.Crypto.Encrypter;
    using ClashRoyale.Client.Packets.Crypto.Init;

    internal class Device
    {
        internal Socket Socket;
        internal Token Token;
        internal Client Client;

        internal IEncrypter SendEncrypter;
        internal IEncrypter ReceiveEncrypter;

        internal RC4Init RC4Init;
        internal PepperInit PepperInit;
        
        internal State State = State.DISCONNECTED;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        internal Device()
        {
            this.Token      = new Token(new SocketAsyncEventArgs(), this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="Socket">The socket.</param>
        internal Device(Socket Socket, Client Client) : this()
        {
            this.Socket     = Socket;
            this.Client     = Client;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Device"/> is connected.
        /// </summary>
        /// <value>
        ///   True if connected, false if disconnected.
        /// </value>
        internal bool Connected
        {
            get
            {
                if (this.State == State.DISCONNECTED)
                {
                    return false;
                }

                if (this.Socket.Connected)
                {
                    return true;

                    try
                    {
                        if (!this.Socket.Poll(1000, SelectMode.SelectRead) || this.Socket.Available != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Processes the specified buffer.
        /// </summary>
        /// <param name="Buffer">The buffer.</param>
        internal void Process(byte[] Buffer)
        {
            if (Buffer.Length >= 7)
            {
                using (Reader Reader = new Reader(Buffer))
                {
                    Debug.WriteLine("[*] Encrypted : " + BitConverter.ToString(Buffer) + ".");

                    ushort Identifier   = Reader.ReadUInt16();
                    int Length          = Reader.ReadInt24();
                    ushort Version      = Reader.ReadUInt16();

                    byte[] Encrypted    = Reader.ReadBytes(Length);
                    byte[] Packet       = null;

                    if (Buffer.Length - 7 >= Length)
                    {
                        if (this.ReceiveEncrypter == null)
                        {
                            if (this.PepperInit.State == 1)
                            {
                                if (Identifier == 20100)
                                {
                                    Packet = PepperCrypto.HandlePepperAuthentificationResponse(ref this.PepperInit, Encrypted);
                                }
                                else
                                {
                                    Packet = Encrypted;
                                }
                            }
                            else
                            {
                                if (this.PepperInit.State == 3)
                                {
                                    if (Identifier == 20103 || Identifier == 22280)
                                    {
                                        Packet = PepperCrypto.HandlePepperLoginResponse(ref this.PepperInit, Encrypted, out this.SendEncrypter, out this.ReceiveEncrypter);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Packet = this.ReceiveEncrypter.Decrypt(Encrypted);
                        }

                        if (Packet != null)
                        {
                            Debug.WriteLine("[*] " + Identifier + " : " + BitConverter.ToString(Packet));

                            if (MessageFactory.Messages.ContainsKey(Identifier))
                            {
                                Message Message = (Message) Activator.CreateInstance(MessageFactory.Messages[Identifier], this, new Reader(Packet));

                                Debug.WriteLine("[*] We received the following message : " + Message.GetType().Name + ", Version " + Version + ".");

                                Message.Identifier  = Identifier;
                                Message.Length      = (uint) Length;
                                Message.Version     = Version;

                                try
                                {
                                    Message.Decode();
                                    Message.Process();
                                }
                                catch (Exception Exception)
                                {
                                    Debug.WriteLine("[*] " + Exception.GetType().Name + " when processing " + Message.GetType().Name + ".");
                                }
                            }
                            else
                            {
                                Debug.WriteLine("[*] We can't handle the following message : ID " + Identifier + ", Length " + Length + ", Version " + Version + ".");

                                byte[] AltBuffer = Reader.ReadBytes((int) Length);
                                this.Crypto.Decrypt(ref AltBuffer);
                                AltBuffer = null;
                            }
                        }
                        else
                        {
                            Debug.WriteLine("[*] Unable to decrypt message type " + Identifier + ".");
                        }

                        if (!this.Token.Aborting)
                        {
                            this.Token.Packet.RemoveRange(0, (int)(Length + 7));

                            if (Buffer.Length - 7 - Length >= 7)
                            {
                                this.Process(Reader.ReadBytes((int)(Buffer.Length - 7 - Length)));
                            }
                        }
                    }
                }
            }
        }
    }
}