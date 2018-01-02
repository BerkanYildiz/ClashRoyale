namespace ClashRoyale.Interfaces
{
    using ClashRoyale.Enums;

    public interface IDevice
    {
        Defines Defines
        {
            get;
            set;
        }

        State State
        {
            get;
            set;
        }
    }
}
