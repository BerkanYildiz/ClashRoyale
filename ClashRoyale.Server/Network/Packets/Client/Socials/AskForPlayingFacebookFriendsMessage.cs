namespace ClashRoyale.Server.Network.Packets.Client.Socials
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Database;
    using ClashRoyale.Server.Database.Models;
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server.Socials;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    internal class AskForPlayingFacebookFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 10513;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Home;
            }
        }

        private string[] FriendsIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForPlayingFacebookFriendsMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForPlayingFacebookFriendsMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForPlayingFacebookFriendsMessage.
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
            int OnlineFriends = 0;

            List<Player> Friends = new List<Player>(this.FriendsIds.Length);

            foreach (string FacebookId in this.FriendsIds)
            {
                Logging.Info(this.GetType(), "https://graph.facebook.com/v2.2/" + FacebookId + "/picture");

                var DbRequest   = await GameDb.Players.FindAsync(new JsonFilterDefinition<PlayerDb>("{'Data.api.facebook.fbId' : \"" + FacebookId + "\"}"));
                var PlayerDb    = DbRequest.SingleOrDefault();

                if (PlayerDb != null)
                {
                    Player Player = await Players.Get(PlayerDb.HighId, PlayerDb.LowId, false);

                    if (Player == null)
                    {
                        // Player = JsonConvert.DeserializeObject<Player>(PlayerDb.Profile.ToString(), Resources.Players.Settings);
                        Logging.Error(this.GetType(), "Player == null.");
                    }
                    else
                    {
                        if (Player.IsConnected)
                        {
                            OnlineFriends++;
                        }
                    }

                    Friends.Add(Player);
                }
            }

            this.Device.NetworkManager.SendMessage(new OnlineFacebookFriendsMessage(this.Device, Friends));
            this.Device.NetworkManager.SendMessage(new FriendsListMessage(this.Device, Friends));
        }
    }
}