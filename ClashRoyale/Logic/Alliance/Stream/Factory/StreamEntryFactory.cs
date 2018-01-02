namespace ClashRoyale.Logic.Alliance.Stream.Factory
{
    public static class StreamEntryFactory
    {
        /// <summary>
        /// Creates a stream entry instance by type.
        /// </summary>
        public static StreamEntry CreateStreamEntryByType(int Type)
        {
            switch (Type)
            {
                case 1:
                {
                    return new DonateStreamEntry();
                }

                case 2:
                {
                    return new ChatStreamEntry();
                }

                case 3:
                {
                    return new JoinRequestAllianceStreamEntry();
                }

                case 4:
                {
                    return new AllianceEventStreamEntry();
                }

                case 5:
                {
                    return null; // TODO : Implement ReplayStreamEntry::ReplayStreamEntry()
                }

                case 6:
                {
                    return null; // TODO : Implement CoOpenStreamEntry::CoOpenStreamEntry()
                }

                case 10:
                {
                    return new ChallengeStreamEntry();
                }

                case 11:
                {
                    return null; // TODO : Implement ChallengeDoneStreamEntry::ChallengeDoneStreamEntry()
                }
            }

            return null;
        }
    }
}