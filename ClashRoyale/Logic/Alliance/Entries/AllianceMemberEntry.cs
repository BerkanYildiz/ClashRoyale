namespace ClashRoyale.Logic.Alliance.Entries
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json;

    public class AllianceMemberEntry
    {
        private Clan Clan;

        [JsonProperty("highId")]    public int HighId;
        [JsonProperty("lowId")]     public int LowId;

        [JsonProperty("donations")] public int Donations;
        [JsonProperty("expLevel")]  public int Level;
        [JsonProperty("score")]     public int Score;
        [JsonProperty("crowns")]    public int Crowns;
        [JsonProperty("role")]      public int Role;

        [JsonProperty("name")]      public string Name;

        [JsonProperty("arena")]     public ArenaData Arena;

        /// <summary>
        /// Gets the player id.
        /// </summary>
        public long PlayerId
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
        public AllianceMemberEntry Clone()
        {
            ByteStream Stream = new ByteStream();
            AllianceMemberEntry MemberEntry = new AllianceMemberEntry(this.Clan);

            this.Encode(Stream);
            Stream.SetOffset(0);
            MemberEntry.Decode(Stream);

            return MemberEntry;
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
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
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
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
        public bool HasLowerRole(int Comparer)
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
        public void SetAllianceRole(int Role)
        {
            this.Role = Role;
        }
        
        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update(Player Player)
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