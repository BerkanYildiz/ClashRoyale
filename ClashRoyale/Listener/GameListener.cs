namespace ClashRoyale.Listener
{
    using System;

    using ClashRoyale.Exceptions;
    using ClashRoyale.Messages;

    public class GameListener
    {
        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        public virtual bool IsConnected
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an android device.
        /// </summary>
        public virtual bool IsAndroid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Adds this instance to the matchmaking system.
        /// </summary>
        public virtual void Matchmaking()
        {
            throw new LogicException(this.GetType(), new NotImplementedException("GameListener::Matchmaking() is not implemented."));
        }

        /// <summary>
        /// Sends the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="Message">The message.</param>
        public virtual void SendMessage(Message Message)
        {
            throw new LogicException(this.GetType(), new NotImplementedException("GameListener::SendMessage(Message) is not implemented."));
        }
    }
}