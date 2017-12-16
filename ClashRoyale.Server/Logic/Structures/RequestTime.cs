namespace ClashRoyale.Server.Logic.Structures
{
    using System;

    using ClashRoyale.Server.Network.Packets;

    internal struct RequestTime
    {
        internal int InvalidRequestCount;

        internal DateTime AskForGoHome;
        internal DateTime AskForVisitHome;
        internal DateTime AskForAllianceData;
        internal DateTime AskForChatToAlliance;
        internal DateTime AskForJoinableAlliancesList;

        /// <summary>
        /// Gets if the specified message can be handled.
        /// </summary>
        internal bool CanHandleMessage(Message Message)
        {
            switch (Message.Type)
            {
                case 14101:
                {
                    return this.ValidTime(ref this.AskForGoHome);
                }

                case 14302:
                {
                    return this.ValidTime(ref this.AskForAllianceData);
                }

                case 14303:
                {
                    return this.ValidTime(ref this.AskForJoinableAlliancesList);
                }

                case 14308:
                {
                    return this.ValidTime(ref this.AskForChatToAlliance);
                }

                case 14113:
                {
                    return this.ValidTime(ref this.AskForVisitHome);
                }

                default:
                {
                    return true;
                }
            }
        }

        private bool ValidTime(ref DateTime LastRequest)
        {
            DateTime Utc = DateTime.UtcNow;

            if (DateTime.UtcNow.Subtract(LastRequest).TotalMilliseconds >= 500)
            {
                LastRequest = Utc;
                return true;
            }

            ++this.InvalidRequestCount;

            return false;
        }
    }
}