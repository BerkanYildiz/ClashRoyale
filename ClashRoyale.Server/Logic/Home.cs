namespace ClashRoyale.Server.Logic
{
    internal class Home
    {
        internal Player Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        internal Home()
        {
            // Home.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="Player">The player.</param>
        internal Home(Player Player) : this()
        {
            this.Player = Player;
        }
    }
}
