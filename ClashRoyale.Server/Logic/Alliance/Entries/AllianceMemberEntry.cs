namespace ClashRoyale.Server.Logic.Entries
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Files.Csv.Logic;

    using Newtonsoft.Json;

    internal class AllianceMemberEntry
    {
        private Clan Clan;

        [JsonProperty("highId")]    internal int HighId;
        [JsonProperty("lowId")]     internal int LowId;

        [JsonProperty("donations")] internal int Donations;
        [JsonProperty("expLevel")]  internal int Level;
        [JsonProperty("score")]     internal int Score;
        [JsonProperty("crowns")]    internal int Crowns;
        [JsonProperty("role")]      internal int Role;

        [JsonProperty("name")]      internal string Name;

        [JsonProperty("arena")]     internal ArenaData Arena;

        /// <summary>
        /// Gets the player id.
        /// </summary>
        internal long PlayerId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMemberEntry"/> class.
        /// </summary>
        public AllianceMemberEntry()
        {
            // AllianceMemberEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMemberEntry"/> class.
        /// </summary>
        public AllianceMemberEntry(Clan Clan)
        {
            this.Clan = Clan;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMemberEntry"/> class.
        /// </summary>
        public AllianceMemberEntry(Clan Clan, Player Player, int Role) : this(Clan)
        {
            this.Role = Role;
            this.Update(Player);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        internal AllianceMemberEntry Clone()
        {
            ByteStream Stream = new ByteStream();
            AllianceMemberEntry MemberEntry = new AllianceMemberEntry(this.Clan);

            this.Encode(Stream);
            Stream.SetOffset(0);
            MemberEntry.Decode(Stream);

            return MemberEntry;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream Stream)
        {
            this.HighId     = Stream.ReadVInt();
            this.LowId      = Stream.ReadVInt();
            this.Name       = Stream.ReadStringReference();
            this.Arena      = Stream.DecodeData<ArenaData>();
            this.Role       = Stream.ReadVInt();
            this.Level      = Stream.ReadVInt();
            this.Score      = Stream.ReadVInt();
            this.Donations  = Stream.ReadVInt();

            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();
            Stream.ReadVInt();

            Stream.ReadBoolean();
            Stream.ReadBoolean();

            if (Stream.ReadBoolean())
            {
                Stream.ReadVInt(); // HomeID
                Stream.ReadVInt();
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream Stream)
        {
            Stream.WriteLong(this.PlayerId);
            Stream.WriteString(this.Name);

            Stream.EncodeData(this.Arena);

            Stream.WriteVInt(this.Role);
            Stream.WriteVInt(this.Level);
            Stream.WriteVInt(this.Score);
            Stream.WriteVInt(this.Donations);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);

            if (true)
            {
                Stream.WriteBoolean(true);
                Stream.WriteLong(this.PlayerId);
            }
            else
            {
                Stream.WriteBoolean(false);
            }
        }

        /// <summary>
        /// Gets if the specified role has lower role.
        /// </summary>
        internal bool HasLowerRole(int Comparer)
        {
            switch (Comparer)
            {
                case 4:
                {
                    return this.Role != 2 && this.Role != 4;
                }

                case 3:
                {
                    return this.Role == 1;
                }

                case 2:
                {
                    return this.Role != 2;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets the alliance role.
        /// </summary>
        internal void SetAllianceRole(int Role)
        {
            this.Role = Role;
        }
        
        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update(Player Player)
        {
            this.HighId = Player.HighId;
            this.LowId  = Player.LowId;
            this.Level  = Player.ExpLevel;
            this.Arena  = Player.Arena;
            this.Role   = Player.AllianceRole;
            this.Name   = Player.Name;
            this.Score  = Player.Score;

            Player.SetAllianceRole(this.Role);
        }
    }
}