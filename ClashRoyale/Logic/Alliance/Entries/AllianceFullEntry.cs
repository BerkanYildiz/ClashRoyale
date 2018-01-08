namespace ClashRoyale.Logic.Alliance.Entries
{
    using ClashRoyale.Extensions;

    public class AllianceFullEntry
    {
        public AllianceHeaderEntry Header;
        public AllianceMemberEntry[] Members;

        public string Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceFullEntry"/> class.
        /// </summary>
        public AllianceFullEntry()
        {
            // AllianceFullEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceFullEntry"/> class.
        /// </summary>
        /// <param name="Header">The header.</param>
        /// <param name="Members">The members.</param>
        /// <param name="Description">The description.</param>
        public AllianceFullEntry(AllianceHeaderEntry Header, AllianceMemberEntry[] Members, string Description)
        {
            this.Header         = Header;
            this.Members        = Members;
            this.Description    = Description;
        }

        /// <summary>
        /// Decodes from the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Decode(ByteStream Stream)
        {
            this.Header.Decode(Stream);
            this.Description = Stream.ReadString();

            int Length = Stream.ReadVInt();

            for (int i = 0; i < Length; i++)
            {
                AllianceMemberEntry Entry = new AllianceMemberEntry();
                Entry.Decode(Stream);
                this.Members[i] = Entry;
            }
        }

        /// <summary>
        /// Encodes in the specified stream.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public void Encode(ChecksumEncoder Stream)
        {
            this.Header.Encode(Stream);

            Stream.WriteString(this.Description);

            Stream.WriteVInt(this.Members.Length);

            foreach (AllianceMemberEntry Entry in this.Members)
            {
                Entry.Encode(Stream);
            }
        }
    }
}