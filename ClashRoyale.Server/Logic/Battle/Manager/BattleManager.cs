namespace ClashRoyale.Server.Logic.Battle.Manager
{
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Timers;

    using ClashRoyale.Server.Files.Csv;
    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands.Storage;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Math;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Logic.RoyalTV;
    using ClashRoyale.Server.Logic.RoyalTV.Entry;
    using ClashRoyale.Server.Logic.Time;
    using ClashRoyale.Server.Network.Packets.Server;

    using Timer = System.Timers.Timer;

    internal static class BattleManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BattleManager"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        internal const int BATTLE_UPDATE_TICKS = 10; // DON'T TOUCH !!!

        internal static Timer Timer;
        internal static ConcurrentDictionary<long, GameMode> Waitings;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            if (BattleManager.Initialized)
            {
                return;
            }

            BattleManager.Waitings       = new ConcurrentDictionary<long, GameMode>();

            BattleManager.Timer          = new Timer();
            BattleManager.Timer.Interval = 1000;
            BattleManager.Timer.Elapsed += BattleManager.Matchmake;
            BattleManager.Timer.Start();

            BattleManager.Initialized    = true;
        }

        /// <summary>
        /// Matchmakes the specified sender.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        /// <param name="ElapsedEventArgs">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private static void Matchmake(object Sender, ElapsedEventArgs ElapsedEventArgs)
        {
            GameMode[] GameModes = BattleManager.Waitings.Values.ToArray();

            for (int I = 0; I < GameModes.Length; I++)
            {
                GameMode GameMode = GameModes[I];

                if (GameMode.IsConnected)
                {
                    if (GameMode.State == HomeState.Home)
                    {
                        int MatchmakePoints = BattleManager.CalculateMatchmakePoints(GameMode);
                        int BestMatchmakePoints = -1;

                        Search:

                        for (int J = Math.Min(GameModes.Length - 1, 100); J >= 0; J--)
                        {
                            if (I != J)
                            {
                                if (GameModes[J].IsConnected)
                                {
                                    int Points = BattleManager.CalculateMatchmakePoints(GameModes[J]);

                                    if (BestMatchmakePoints == -1 || Points + 200 >= MatchmakePoints && Points - 200 <= MatchmakePoints)
                                    {
                                        if (BattleManager.Waitings.TryRemove(GameMode.Player.PlayerId, out _))
                                        {
                                            if (BattleManager.Waitings.TryRemove(GameModes[J].Player.PlayerId, out _))
                                            {
                                                BattleManager.InitBattle(new []
                                                {
                                                    GameMode.Player,
                                                    GameModes[J].Player
                                                }, CsvFiles.GameModeLadderData);

                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    BattleManager.Waitings.TryRemove(GameModes[J].Player.PlayerId, out _);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates total matchmake points.
        /// </summary>
        internal static int CalculateMatchmakePoints(GameMode GameMode)
        {
            Player Player = GameMode.Player;

            if (Player != null)
            {
                return Player.Score;
            }

            return 0;
        }

        /// <summary>
        /// Adds a player in queue.
        /// </summary>
        internal static void AddPlayer(GameMode GameMode)
        {
            if (GameMode.Player != null)
            {
                if (BattleManager.Waitings.TryAdd(GameMode.Player.PlayerId, GameMode))
                {
                    int Estimed;
                    int Count = BattleManager.Waitings.Count;

                    if (Count > 0)
                    {
                        if (Count > 5)
                        {
                            if (Count > 25)
                            {
                                if (Count > 100)
                                {
                                    Estimed = 5;
                                }
                                else
                                    Estimed = 15;
                            }
                            else
                                Estimed = 60;
                        }
                        else
                            Estimed = 600;
                    }
                    else
                        Estimed = 900;

                    GameMode.Device.NetworkManager.SendMessage(new MatchmakeInfoMessage(GameMode.Device, Estimed));
                }
            }
        }

        /// <summary>
        /// Initializes a new battle.
        /// </summary>
        internal static void InitBattle(Player[] Players, GameModeData GameMode)
        {
            if (Players.Length <= 0)
            {
                return;
            }

            ArenaData ArenaData = null;

            CommandStorage Storage = new CommandStorage();
            CommandStorage Queue = new CommandStorage();

            object Locker = new object();

            for (int I = 0; I < Players.Length; I++)
            {
                if (ArenaData == null || Players[I].Arena.DemoteTrophyLimit > ArenaData.DemoteTrophyLimit)
                {
                    ArenaData = Players[I].Arena;
                }
            }

            for (int I = 0; I < Players.Length; I++)
            {
                Player Player = Players[I];

                Player.GameMode.LoadBattleState();

                for (int J = 0; J < Players.Length; J++)
                {
                    Player.GameMode.AddPlayer(Players[J], J);
                }

                Player.GameMode.Battle.SetArena(ArenaData);
                Player.GameMode.Battle.SetLocation(ArenaData.PvPLocationData);
                Player.GameMode.Battle.SetGameMode(GameMode);

                Player.GameMode.SectorManager.SetCommandStorage(Storage);
                Player.GameMode.SectorManager.SetCommandQueue(Queue);
                Player.GameMode.SectorManager.SetLocker(Locker);
            }

            BattleManager.InitBattleTimer(Players, Locker).Start();
        }

        /// <summary>
        /// Starts the battle between the two players.
        /// </summary>
        internal static Timer InitBattleTimer(Player[] Players, object Locker)
        {
            Timer Timer = new Timer(50 * BattleManager.BATTLE_UPDATE_TICKS);

            Timer.Elapsed += (Ass, Dick) =>
            {
                lock (Locker)
                {
                    for (int I = 0; I < Players.Length; I++)
                    {
                        for (int J = 0; J < BattleManager.BATTLE_UPDATE_TICKS; J++)
                        {
                            Players[I].GameMode.SectorManager.IncreaseTick();
                        }
                    }

                    Players[0].GameMode.SectorManager.Queue.RemoveCommands();

                    int LastTurn = 0;

                    for (int I = 0; I < Players.Length; I++)
                    {
                        LastTurn = Math.Max(LastTurn, Players[I].GameMode.SectorManager.LastClientTurn);
                    }

                    if (Players[0].GameMode.Time > LastTurn + Time.GetSecondsInTicks(5))
                    {
                        if (Players[0].GameMode.Replay != null)
                        {
                            BattleLog BattleLog = new BattleLog(Players[0].GameMode.Battle, Players[0].GameMode.Replay);
                            Battles.Create(BattleLog).Wait();

                            BattleManager.AddReplayToRoyalTv(BattleLog);
                        }

                        for (int I = 0; I < Players.Length; I++)
                        {
                            Players[I].GameMode.SectorManager.EndBattle();
                        }

                        Timer.Dispose();
                    }
                }
            };


            return Timer;
        }

        /// <summary>
        /// Adds the specified replay to royal tv.
        /// </summary>
        internal static void AddReplayToRoyalTv(BattleLog BattleLog)
        {
            int ChannelIdx = RoyalTvManager.GetChannelArenaData(BattleLog.ArenaData);

            if (ChannelIdx != -1)
            {
                RoyalTvManager.AddEntry(ChannelIdx, new RoyalTvEntry(BattleLog));
            }
        }
    }
}