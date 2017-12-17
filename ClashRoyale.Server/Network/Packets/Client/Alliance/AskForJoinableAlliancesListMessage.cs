namespace ClashRoyale.Server.Network.Packets.Client
{
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic;
    using ClashRoyale.Server.Logic.Alliance;
    using ClashRoyale.Server.Logic.Alliance.Entries;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Network.Packets.Server;

    internal class AskForJoinableAlliancesListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        internal override short Type
        {
            get
            {
                return 14303;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        internal override Node ServiceNode
        {
            get
            {
                return Node.Alliance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForJoinableAlliancesListMessage"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        /// <param name="ByteStream">The byte stream.</param>
        public AskForJoinableAlliancesListMessage(Device Device, ByteStream ByteStream) : base(Device, ByteStream)
        {
            // AskForJoinableAlliancesListMessage.
        }
        
        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal override void Process()
        {
            List<AllianceHeaderEntry> Alliances = new List<AllianceHeaderEntry>(50);

            if (Clans.Count > 50)
            {
                Clan[] Availables = Clans.GetAll().FindAll(T => T.HeaderEntry.Type == 0 /* && _T.HeaderEntry.NumberOfMembers < 50 */ && T.HeaderEntry.NumberOfMembers > 0).ToArray();

                Logging.Info(this.GetType(), "Generated a list of joinables clans, then shuffle them.");
                
                int Skiped  = 0;

                for (int I = 0; I < 50; I++)
                {
                    if (Availables.Length - Skiped > 50 - I)
                    {
                        if (!Program.Random.NextBool())
                        {
                            ++Skiped;
                        }
                        else
                        {
                            Alliances.Add(Availables[I].HeaderEntry);
                        }
                    }
                    else
                    {
                        Alliances.Add(Availables[I].HeaderEntry);
                    }
                }

                Logging.Info(this.GetType(), "The joinables clans list has been shuffled.");
            }
            else
            {
                Clan[] Availables = Clans.GetAll().FindAll(T => T.HeaderEntry.NumberOfMembers < 50 && T.HeaderEntry.NumberOfMembers > 0).ToArray();

                Logging.Info(this.GetType(), "Generated a list of joinables clans without shuffling them.");

                for (int I = 0; I < Availables.Length; I++)
                {
                    Alliances.Add(Availables[I].HeaderEntry);
                }
            }

            this.Device.NetworkManager.SendMessage(new JoinableAllianceListMessage(this.Device, Alliances));
        }
    }
}