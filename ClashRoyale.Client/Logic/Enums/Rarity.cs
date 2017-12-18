namespace ClashRoyale.Client.Logic.Enums
{
    public enum Rarity
    {
        COMMON = 0,
        RARE = 1,
        EPIC = 2,
        LENGENDARY = 3
    }

    public class String_To_Rarity_ID
    {
        public static Rarity GetArenaID(string _Rarity)
        {
            switch (_Rarity)
            {
                case "Common":
                    return Rarity.COMMON;

                case "Rare":
                    return Rarity.RARE;

                case "Epic":
                    return Rarity.EPIC;

                case "Legendary":
                    return Rarity.LENGENDARY;

                default:
                    return Rarity.COMMON;
            }
        }
    }
}