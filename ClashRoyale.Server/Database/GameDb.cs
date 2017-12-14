namespace ClashRoyale.Server.Database
{
    using System;

    using ClashRoyale.Server.Database.Models;
    using ClashRoyale.Server.Logic.Collections;

    using MongoDB.Driver;

    internal static class GameDb
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GameDb"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        internal static IMongoCollection<PlayerDb> Players;
        internal static IMongoCollection<ClanDb> Clans;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (GameDb.Initialized)
            {
                return;
            }

            var MongoClient     = new MongoClient("mongodb://GL.Servers.CR:YXPZvFNuryLwWyD4fDckxEV5qFH1f3jBWck8scvt@ns1.gobelinland.fr:27017");
            var MongoDb         = MongoClient.GetDatabase("ClashRoyale");

            Logging.Info(typeof(GameDb), "GameDb is connected to " + MongoClient.Settings.Server.Host + ".");

            if (MongoDb == null)
            {
                throw new Exception("MongoDb == null at Initialize().");
            }

            if (MongoDb.GetCollection<PlayerDb>("Players") == null)
            {
                MongoDb.CreateCollection("Players");
            }

            if (MongoDb.GetCollection<ClanDb>("Clans") == null)
            {
                MongoDb.CreateCollection("Clans");
            }

            GameDb.Players      = MongoDb.GetCollection<PlayerDb>("Players");
            GameDb.Clans        = MongoDb.GetCollection<ClanDb>("Clans");

            GameDb.Players.Indexes.CreateOne(Builders<PlayerDb>.IndexKeys.Combine(
                Builders<PlayerDb>.IndexKeys.Ascending(T => T.HighId),
                Builders<PlayerDb>.IndexKeys.Descending(T => T.LowId)),

                new CreateIndexOptions()
                {
                    Name = "entityIds",
                    Background = true
                    
                }
            );

            GameDb.Clans.Indexes.CreateOne(Builders<ClanDb>.IndexKeys.Combine(
                Builders<ClanDb>.IndexKeys.Ascending(T => T.HighId),
                Builders<ClanDb>.IndexKeys.Descending(T => T.LowId)),

                new CreateIndexOptions()
                {
                    Name = "entityIds",
                    Background = true
                    
                }
            );

            GameDb.Initialized  = true;
        }


        /// <summary>
        /// Gets the seed for the specified collection.
        /// </summary>
        internal static int GetPlayersSeed()
        { 
            return GameDb.Players.Find(T => T.HighId == Constants.ServerId)
                .Sort(Builders<PlayerDb>.Sort.Descending(T => T.LowId))
                .Limit(1)
                .SingleOrDefault()?.LowId ?? 0;
        }

        /// <summary>
        /// Gets the seed for the specified collection.
        /// </summary>
        internal static int GetClansSeed()
        {
            return GameDb.Clans.Find(T => T.HighId == Constants.ServerId)
                .Sort(Builders<ClanDb>.Sort.Descending(T => T.LowId))
                .Limit(1)
                .SingleOrDefault()?.LowId ?? 0;
        }
    }
}
