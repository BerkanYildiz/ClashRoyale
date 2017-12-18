namespace ClashRoyale.Extensions.Game
{
    using ClashRoyale.Enums;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;

    public class ClientGlobals
    {
        public static ArenaData[] TvArenas;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            string[] TvArenas = CsvFiles.Get(Gamefile.ClientGlobal).GetData<GlobalData>("TV_ARENAS").StringArray;

            ClientGlobals.TvArenas = new ArenaData[TvArenas.Length];

            for (int I = 0; I < TvArenas.Length; I++)
            {
                ClientGlobals.TvArenas[I] = CsvFiles.Get(Gamefile.Arena).GetData<ArenaData>(TvArenas[I]);
            }
        }
    }
}