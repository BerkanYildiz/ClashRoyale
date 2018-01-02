namespace ClashRoyale.Logic.Player
{
    public class PlayerBase
    {
        /// <summary>
        /// Gets the checksum of this instance.
        /// </summary>
        public virtual int Checksum
        {
            get
            {
                return 0;
            }
        }

        public virtual bool IsNpcPlayer
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerBase"/> class.
        /// </summary>
        public PlayerBase()
        {
            // PlayerBase.
        }
    }
}