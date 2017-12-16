namespace ClashRoyale.Server.Logic.Structures
{
    internal struct Stats
    {
        internal int TotalNewConnections;
        internal int TotalConnections;
        internal int TotalMessages;

        public override string ToString()
        {
            return "Total Connections: " + this.TotalConnections
                   + "\r\nTotal New Connections : " + this.TotalNewConnections
                   + "\r\nTotal Messages : " + this.TotalMessages;
        }
    }
}