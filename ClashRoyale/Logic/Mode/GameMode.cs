namespace ClashRoyale.Logic.Mode
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Listener;
    using ClashRoyale.Logic.Battle;
    using ClashRoyale.Logic.Commands.Manager;
    using ClashRoyale.Logic.Home;
    using ClashRoyale.Logic.Manager;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Logic.Replay;
    using ClashRoyale.Logic.Sector.Manager;
    using ClashRoyale.Logic.Time;

    using Random = ClashRoyale.Maths.Random;

    public class GameMode
    {
        public Time Time;
        public HomeState State;

        public Battle Battle;
        public Player Player;
        public Replay Replay;

        public GameListener Listener;
        public SectorManager SectorManager;
        public CommandManager CommandManager;
        public AchievementManager AchievementManager;

        private readonly Random Random;
        private readonly ChecksumEncoder ChecksumEncoder;

        /// <summary>
        /// Gets the home.
        /// </summary>
        public Home Home
        {
            get
            {
                return this.Player?.Home;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the device is connected.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (this.Listener != null)
                {
                    return this.Listener.IsConnected;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the checksum of this instance.
        /// </summary>
        public int Checksum
        {
            get
            {
                if (this.Home != null)
                {
                    return this.Home.Checksum;
                }
                
                this.ChecksumEncoder.ResetChecksum();
                this.Encode(this.ChecksumEncoder, false);

                return this.ChecksumEncoder.Checksum;
            }
        }

        /// <summary>
        /// Gets the random seed.
        /// </summary>
        public int RandomSeed
        {
            get
            {
                return this.Random.Seed;
            }
        }

        /// <summary>
        /// Gets if the message execution is imediate.
        /// </summary>
        public bool IsImmediateMessageExecution
        {
            get
            {
                if (this.Battle == null)
                {
                    return true;
                }

                /* if (this._Battle.Type <= 3)
                {
                    return true;
                } */

                return false;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        public GameMode()
        {
            this.Random             = new Random();
            this.SectorManager      = new SectorManager(this);
            this.CommandManager     = new CommandManager(this);
            this.AchievementManager = new AchievementManager(this);
            this.ChecksumEncoder    = new ChecksumEncoder(null);
        }

        /// <summary>
        /// Adds the player to battle.
        /// </summary>
        public void AddPlayer(Player Player, int Index)
        {
            this.Battle.AddPlayer(Player, Index);
        }

        /// <summary>
        /// Clears the client ticks.
        /// </summary>
        public void ClearClientTicks()
        {
            if (this.Time > 0)
            {
                this.FastForwardTime((this.Time + 19) / 20);
                this.Time = new Time();
            }
        }

        /// <summary>
        /// Puts to end the battle state.
        /// </summary>
        public void EndBattleState()
        {
            this.State          = HomeState.None;

            this.Replay         = null;
            this.Battle         = null;
            this.SectorManager  = null;
        }

        /// <summary>
        /// Puts to end the home state.
        /// </summary>
        public void EndHomeState()
        {
            this.State = HomeState.None;
        }

        /// <summary>
        /// Creates a fast forward of time.
        /// </summary>
        public void FastForwardTime(int Seconds)
        {
            if (Seconds > 0)
            {
                if (this.Home != null)
                {
                    this.Home.FastForward(Seconds);
                }
                else
                {
                    Logging.Warning(this.GetType(), "FastForwardTime(Seconds) called when Home == null.");
                }
            }
        }

        /// <summary>
        /// Gets a random int.
        /// </summary>
        public int GetRandomInt(int Max)
        {
            return this.Random.Rand(Max);
        }

        /// <summary>
        /// Loads home state.
        /// </summary>
        public void LoadHomeState(Player Player, int SecondsSinceLastSave, int RandomSeed)
        {
            this.State = HomeState.Home;

            this.ClearClientTicks();
            this.SetPlayer(Player);
            this.FastForwardTime(SecondsSinceLastSave);
            this.SetRandomSeed(RandomSeed);
            this.Home.LoadingFinished();
        }

        /// <summary>
        /// Loads home state.
        /// </summary>
        public void LoadBattleState()
        {
            this.State          = HomeState.Attack;

            this.ClearClientTicks();
            
            this.Battle         = new Battle(this);
            this.Replay         = new Replay(this);
            this.SectorManager  = new SectorManager(this);
        }

        /// <summary>
        /// Loads the replay.
        /// </summary>
        public void LoadReplay(string Json)
        {
            if (this.State == HomeState.None)
            {
                // TODO : Implement LogicGameMode::loadReplay().
            }
            else
            {
                Logging.Error(this.GetType(), "LoadReplay(Json) called twice.");
            }
        }

        /// <summary>
        /// Sets the player.
        /// </summary>
        public void SetPlayer(Player Player)
        {
            this.Player                = Player;
            this.Player.GameMode       = this;
            this.Player.Home.GameMode  = this;
        }

        /// <summary>
        /// Sets the random seed.
        /// </summary>
        public void SetRandomSeed(int Seed)
        {
            this.Random.Seed = Seed;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            this.CommandManager.Tick();

            if (this.Replay != null)
            {
                this.Replay.Tick();
            }

            if (this.Battle != null)
            {
                this.Battle.Tick();
            }
            else
            {
                this.Home.Tick();
            }
        }
        

        /// <summary>
        /// Updates the sector ticks.
        /// </summary>
        public void UpdateSectorTicks(int Tick)
        {
            while (this.Time < Tick)
            {
                this.Tick();
                this.Time.IncreaseTick();
            }
        }
        

        /// <summary>
        /// Updates one tick.
        /// </summary>
        public void UpdateOneTick()
        {
            if (this.Battle == null || !this.Battle.IsFinished)
            {
                this.Tick();
                this.Time.IncreaseTick();

                if (this.State == HomeState.Replay)
                {
                    // TODO : Implement LogicGameMode::updateReplayEventPlayback().
                }
            }
        }
        

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream, bool EncodeCommandManager)
        {
            Stream.ResetChecksum();
            
            /* Stream.WriteVInt(this.Time);
            Stream.WriteVInt(Stream.Checksum);
            Stream.WriteVInt(TimeUtil.Timestamp);

            Stream.WriteVInt(11); // Val != 11: Full update stream is corrupted!

            this.Time.Encode(Stream);
            this.Random.Encode(Stream);

            Stream.WriteVInt(this.Random.Seed);

            if (this.Battle != null)
            {
                this.Battle.Encode(Stream, this.Player);
            }
            else
            {
                this.Home.Encode(Stream.ByteStream);
            }

            Stream.WriteVInt(12); // Val != 12: Full update stream is corrupted #2!
            {
                // TUTORIAL MANAGER

                Stream.EncodeLogicData(null, 49);
                Stream.WriteVInt(0);

                if (this.Battle != null)
                {
                    Stream.EncodeData(null); // WriteGameObjectReference
                }
            }

            Stream.WriteVInt(Stream.Checksum);

            if (EncodeCommandManager)
            {
                this.CommandManager.Encode(Stream);
            } */

            Stream.AddRange("00-21-7F-0B-00-54-7E-9E-44-E8-A6-D2-A9-01-02-08-01-7F-7F-00  00-01  00-00-00-00-00-00-00-00-00-06-01-00-00-09-00-00-00-01-00-00-00-8E-02-F2-7D-00-00-06-7A-06-23-01-23-01-23-01-23-01-23-00-23-00-01-00-01-00-00-01-05-00-05-01-05-02-05-03-05-04-05-05-08-0D-A4-E2-01-9C-8E-03-00-00-7F-00-C0-7C-00-00-02-00-00-00-00-00-00-01-0D-AC-36-A4-65-00-00-7F-00-80-04-00-00-01-00-00-00-00-00-00-08-0D-AC-36-9C-8E-03-00-00-7F-00-C0-7C-00-00-01-00-00-00-00-00-00-01-0D-A4-E2-01-A4-65-00-00-7F-00-80-04-00-00-02-00-00-00-00-00-00-01-0D-A8-8C-01-B8-2E-00-00-7F-00-80-04-00-00-00-00-00-00-00-00-1A-04-03-01-7C-01-04-02-06-05-07-00-7F-7F-7F-00-7F-00-00-05-00-00-00-00-00-7F-7F-7F-7F-7F-7F-7F-7F-00-00-00-00-08-0D-A8-8C-01-88-C5-03-00-00-7F-00-C0-7C-00-00-00-00-00-00-00-00-1A-04-05-7F-7E-04-04-01-07-00-03-00-7F-7F-7F-00-7F-00-00-05-00-00-00-00-00-7F-7F-7F-7F-7F-7F-7F-7F-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-00-A6-27-00-A8-17-00-A6-27-00-A8-17-00-88-28-00-A8-3E-00-00-00-00-00-A4-01-00-00-00-00-A4-01-00-00-00-00-A4-01-00-00-00-00-A4-01-00-00-00-00-A4-01-00-00-00-00-A4-01-FF-01-01-01-02-01-03-01-14-01-04-00-81-01-00-8E-01-01-10-00-00-FF-01-20-06-2E-01-16-05-1A-01-96-01-08-9A-01-02-21-00-15-01-00-05-06-02-02-04-02-01-03-00-00-00-00-00-00-00-00-00-02-00-00-0C-00-00-00-9C-D5-AF-FC-01-00".HexaToBytes());
        }

        /// <summary>
        /// Encodes the first bytes of LogicGameMode.
        /// </summary>
        public void EncodeOnce(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(42); // Value != 42: decodeOnce stream is corrupted! #1

            Stream.AddRange("02-02-01-02-04-01-00  00-00-00-19  54-49-44-5F-43-41-53-54-5F-51-55-45-53-54-5F-4D-49-4E-5F-45-4C-49-58-49-52  00-00-00-00-1E-00-00-00-00-00-06-00-01-01-00-04-04-00  00-00-00-19  54-49-44-5F-43-41-53-54-5F-51-55-45-53-54-5F-4D-41-58-5F-45-4C-49-58-49-52  00-00-00-00-32-00-00-00-00-00-00-02-01-01-00  01 	7F-7F-7F-7F-00-00-00-00-00-00-01-32-00-00-00-00-00-00-00-00-00-00-00-00-00-08-00-00-00-00-00-00-00-00-02-00-00-00-00-00-00-00-00-00-00-00-01 02 	00-01  00-01  00-01  00-00-00-06-42-65-72-6B-61-6E-0A-9D-25-98-04-98-24-00-00-00-00-00-26-00-00-00-00-00-08-0E-05-01-83-B9-01-05-02-B2-04-05-03-00-05-04-00-05-0C-A3-0E-05-0D-00-05-0E-00-05-0F-8F-06-05-16-90-06-05-19-B0-DD-9E-F2-01-05-1A-09-05-1C-00-05-1D-B3-88-D5-44-05-23-01-00-00-00-05-05-06-BF-28-05-07-95-02-05-0B-26-05-14-08-05-1B-09-08-1A-17-8A-01-1A-1D-04-1A-23-26-1A-25-11-1A-2A-15-1A-2E-04-1A-36-05-1C-0B-3E-00-00-09-02-1E-97-8D-69-00-00-00-0B-6C-6F-73-20-70-69-74-75-64-6F-73-B3-01-99-0E-9C-02-00-A9-06-BA-05-01-B6-01-03-00-00-00-01".HexaToBytes());

            // TODO : Implement the Events.

            /* Stream.WriteBoolean(false);

            if (this.Battle != null)
            {
                for (int I = 0; I < 4; I++)
                {
                    if (this.Battle.Players[I] != null)
                    {
                        Stream.WriteBoolean(true);
                        this.Battle.Players[I].Encode(Stream, true);

                        continue;
                    }

                    Stream.WriteBoolean(false);
                }
            } */

            Stream.WriteVInt(43); // Value != 43: decodeOnce stream is corrupted! #2
        }
    }
}