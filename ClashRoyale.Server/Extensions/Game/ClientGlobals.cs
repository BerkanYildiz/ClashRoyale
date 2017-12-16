namespace ClashRoyale.Server.Extensions.Game
{
    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Enums;

    internal class ClientGlobals
    {
        internal static ArenaData[] TvArenas;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
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