namespace ClashRoyale.Proxy.Network
{
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using ClashRoyale.Enums;
    using ClashRoyale.Proxy.Network.States;

    using State = ClashRoyale.Proxy.Network.States.State;

    public class Processor : IDisposable
    {
        public Socket ClientSocket;
        public Socket ServerSocket;

        /// <summary>
        /// Async send/receive thread constructor
        /// </summary>
        public Processor(Socket cs, Socket ss)
        {
            this.ClientSocket   = cs;
            this.ServerSocket   = ss;

            Task.Factory.StartNew(() =>
            {
                ClientState CState = new ClientState(this.ClientSocket);
                ServerState SState = new ServerState(this.ServerSocket);

                this.ServerSocket.BeginReceive(SState.Buffer, 0, State.BufferSize, 0, this.DataReceived, SState);
                this.ClientSocket.BeginReceive(CState.Buffer, 0, State.BufferSize, 0, this.DataReceived, CState);
            });
        }

        /// <summary>
        /// Memory-friendly dispose method
        /// </summary>
        public virtual void Dispose()
        {
            this.ClientSocket.Disconnect(false);
            this.ServerSocket.Disconnect(false);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// DataReceive callback
        /// </summary>
        private void DataReceived(IAsyncResult AsyncResult)
        {
            try
            {
                State State     = (State) AsyncResult.AsyncState;
                int Received    = State.Socket.EndReceive(AsyncResult);

                State.Offset   += Received;

                Logging.Info(this.GetType(), "Received " + Received + " bytes from " + AsyncResult.AsyncState.GetType().Name + ", for a total of " + State.Offset + " bytes !");

                if (Received > 0)
                {
                    byte[] Data = new byte[State.Offset];
                    Array.Copy(State.Buffer, 0, Data, 0, State.Offset);
                    State.Offset = 0;

                    State.Packet.AddRange(Data);

                    if (State.Socket.Available == 0)
                    {
                        this.Handle(State);
                    }
                    else
                    {
                        // Logging.Info(this.GetType(), "We have nothing to receive again, aborting.");
                    }

                    State.Socket.BeginReceive(State.Buffer, State.Offset, State.BufferSize - State.Offset, 0, this.DataReceived, State);
                }
                else
                {
                    Logging.Info(this.GetType(), "We got disconnected by either the server or the client, aborting.");
                }
            }
            catch (Exception Exception)
            {
                Logging.Error(this.GetType(), Exception.GetType().Name + ", " + Exception.Message);
            }
        }

        /// <summary>
        /// Handles the received data
        /// </summary>
        private void Handle(State State)
        {
            if (State.Packet.Count == 0)
            {
                Logging.Info(this.GetType(), "We got disconnected by either the server or the client.");
            }
            else
            {
                if (State.Packet.Count >= 7)
                {
                    int MessageLength = BitConverter.ToInt32(new byte[1].Concat(State.Packet.Skip(2).Take(3)).Reverse().ToArray(), 0);

                    // Logging.Info(this.GetType(), "Received a message with a length of " + MessageLength + " bytes.");

                    if (State.Packet.Count >= MessageLength)
                    {
                        if      (State.GetType() == typeof(ClientState))    this.ServerSocket.Send(new Packet(State.Packet.Take(MessageLength + 7).ToArray(), Destination.FromClient, this.ClientSocket).RebuiltEncrypted);
                        else if (State.GetType() == typeof(ServerState))    this.ClientSocket.Send(new Packet(State.Packet.Take(MessageLength + 7).ToArray(), Destination.FromServer, this.ClientSocket).RebuiltEncrypted);

                        State.Packet.RemoveRange(0, MessageLength + 7);

                        if (State.Packet.Count > 0)
                        {
                            // Logging.Info(this.GetType(), "We can continue to process the packet, we have some bytes left in the buffer.");

                            if (State.Packet.Count >= 7)
                            {
                                this.Handle(State);
                            }
                            else
                            {
                                // Logging.Info(this.GetType(), "We don't have enough bytes to continue to process the packet.");
                            }
                        }
                        else
                        {
                            // Logging.Info(this.GetType(), "We successfully processed the whole buffer.");
                        }
                    }
                    else
                    {
                        // Logging.Info(this.GetType(), "We received a packet, but the buffer length is inferior to the message length.");
                    }
                }
                else
                {
                    // Logging.Info(this.GetType(), "We received a packet, but length is inferior to 7.");
                }
            }
        }
    }
}