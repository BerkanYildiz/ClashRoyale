namespace ClashRoyale.Server
{
    internal class Constants
    {
        /// <summary>
        /// The unique server identifier, used to partition the database.
        /// </summary>
        internal const int ServerId     = 0;

        /// <summary>
        /// The length of the buffer used to send and receive packets.
        /// </summary>
        internal const int BufferSize   = 2048 * 1;

        /// <summary>
        /// The maximum of players the server can handle.
        /// </summary>
        internal const int MaxPlayers   = 10;
    }
}