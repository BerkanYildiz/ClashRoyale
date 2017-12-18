namespace ClashRoyale.Client.Core
{
    using ClashRoyale.Client.Logic.Slots;

    internal class Resources
    {
        internal Bots Bots;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resources"/> class.
        /// </summary>
        public Resources()
        {
            this.Bots = new Bots();
        }
    }
}