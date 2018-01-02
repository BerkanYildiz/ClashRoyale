namespace ClashRoyale.Logic.GameObject.Factory
{
    using ClashRoyale.Files.Csv;

    public static class GameObjectFactory
    {
        /// <summary>
        /// Creates a gameobject by data.
        /// </summary>
        public static GameObject CreateGameObjectByData(CsvData CsvData)
        {
            switch (CsvData.Type)
            {
                case 10:
                {
                    return null; // LogicProjectile
                }

                case 42:
                {
                    return null; // LogicDeco
                }

                case 44:
                {
                    return null; // LogicSpawnPoint
                }

                default:
                {
                    if (CsvData == CsvFiles.SummonerData)
                    {
                        return new Summoner(CsvData);
                    }

                    return new Character(CsvData);
                }
            }
        }
    }
}