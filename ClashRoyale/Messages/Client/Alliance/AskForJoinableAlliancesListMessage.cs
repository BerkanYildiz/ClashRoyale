namespace ClashRoyale.Messages.Client.Alliance
{
    using System.Collections.Generic;

    using ClashRoyale.Crypto.Randomizers;
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic;
    using ClashRoyale.Logic.Alliance;
    using ClashRoyale.Logic.Alliance.Entries;
    using ClashRoyale.Logic.Collections;
    using ClashRoyale.Messages.Server.Alliance;

    public class AskForJoinableAlliancesListMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 10857;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
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
        public override void Process()
        {
            List<AllianceHeaderEntry> Alliances = new List<AllianceHeaderEntry>(50);

            if (Clans.Count > 50)
            {
                Clan[] Availables = Clans.GetAll().FindAll(T => T.HeaderEntry.Type == 0 /* && _T.HeaderEntry.NumberOfMembers < 50 */ && T.HeaderEntry.NumberOfMembers > 0).ToArray();

                int Skiped  = 0;

                for (int I = 0; I < 50; I++)
                {
                    if (Availables.Length - Skiped > 50 - I)
                    {
                        if (!XorShift.NextBool())
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

                for (int I = 0; I < Availables.Length; I++)
                {
                    Alliances.Add(Availables[I].HeaderEntry);
                }
            }

            this.Device.NetworkManager.SendMessage(new JoinableAllianceListMessage(this.Device, Alliances));
        }
    }
}