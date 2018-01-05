namespace ClashRoyale.Messages.Client.Socials
{
    using System.Collections.Generic;

    using ClashRoyale.Database;
    using ClashRoyale.Database.Models;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Messages.Server.Socials;

    using MongoDB.Driver;

    public class AskForPlayingFacebookFriendsMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10513;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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
        public override void Decode()
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
        public override async void Process()
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