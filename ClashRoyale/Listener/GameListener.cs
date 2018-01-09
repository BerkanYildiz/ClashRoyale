namespace ClashRoyale.Listener
{
    using ClashRoyale.Messages;

    public class GameListener
    {
        public virtual void Matchmaking()
        {

        }

        public virtual bool IsConnected()
        {
            return false;
        }

        public virtual bool IsAndroid()
        {
            return false;
        }

        public virtual void SendMessage(Message message)
        {

        }
    }
}