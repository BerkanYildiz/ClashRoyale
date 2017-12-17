namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Database.Models;
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Network.Packets.Server;

    using MongoDB.Driver;

    internal class AskForPlayingGamecenterFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10512;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Friend;
            }
        }

        private string[] FriendsIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForPlayingGamecenterFriendsMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForPlayingGamecenterFriendsMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForPlayingGamecenterFriendsMessage.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal override void Decode()
        {
            int Count       = this.Stream.ReadVInt();

            this.FriendsIds = new string[Count];

            for (int I = 0; I < Count; I++)
            {
                this.FriendsIds[I] = this.Stream.ReadString();
            }
        }

        /// <summary>
        /// Processes this message.
        /// </summary>
        internal override async void Process()
        {
            List<Player> Friends = new List<Player>(this.FriendsIds.Length);

            foreach (string GamecenterId in this.FriendsIds)
            {
                Logging.Info(this.GetType(), "GamcenterFriend/" + GamecenterId + "/");

                var DbRequest = await GameDb.Players.FindAsync(new JsonFilterDefinition<PlayerDb>("{'Data.api.gamecenter.gcId' : \"" + GamecenterId + "\"}"));
                var PlayerDb  = DbRequest.SingleOrDefault();

                if (PlayerDb != null)
                {
                    /* Player Player = await Players.Get(PlayerDb.HighId, PlayerDb.LowId, false);

                    if (Player == null)
                    {
                        Player = JsonConvert.DeserializeObject<Player>(PlayerDb.Profile.ToString(), Resources.Players.Settings);
                    }

                    Friends.Add(Player); */
                }
            }

            this.Device.NetworkManager.SendMessage(new FriendsListMessage(this.Device, Friends));
        }
    }
}