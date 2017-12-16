namespace ClashRoyale.Server.Logic.Mode
{
    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Extensions.Helper;
    using ClashRoyale.Server.Extensions.Utils;
    using ClashRoyale.Server.Logic.Collections;
    using ClashRoyale.Server.Logic.Commands.Manager;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Manager;

    using Random = ClashRoyale.Server.Logic.Random;

    internal class GameMode
    {
        internal Time Time;
        internal HomeState State;

        internal Battle Battle;
        internal Player Player;
        internal Replay Replay;

        internal Device Device;
        internal SectorManager SectorManager;
        internal CommandManager CommandManager;
        internal AchievementManager AchievementManager;

        private readonly Random Random;
        private readonly ChecksumEncoder ChecksumEncoder;

        /// <summary>
        /// Gets the home.
        /// </summary>
        internal Home Home
        {
            get
            {
                return this.Player?.Home;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the device is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (this.Device != null)
                {
                    return this.Device.Network.IsConnected;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the checksum of this instance.
        /// </summary>
        internal int Checksum
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
        internal int RandomSeed
        {
            get
            {
                return this.Random.Seed;
            }
        }

        /// <summary>
        /// Gets if the message execution is imediate.
        /// </summary>
        internal bool IsImmediateMessageExecution
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
        internal GameMode()
        {
            // GameMode.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMode"/> class.
        /// </summary>
        /// <param name="Device">The device.</param>
        internal GameMode(Device Device)
        {
            this.Device             = Device;
            this.Random             = new Random();
            this.SectorManager      = new SectorManager(this);
            this.CommandManager     = new CommandManager(this);
            this.AchievementManager = new AchievementManager(this);
            this.ChecksumEncoder    = new ChecksumEncoder(null);
        }

        /// <summary>
        /// Adds the player to battle.
        /// </summary>
        internal void AddPlayer(Player Player, int Index)
        {
            this.Battle.AddPlayer(Player, Index);
        }

        /// <summary>
        /// Clears the client ticks.
        /// </summary>
        internal void ClearClientTicks()
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
        internal void EndBattleState()
        {
            this.State          = HomeState.None;

            this.Replay         = null;
            this.Battle         = null;
            this.SectorManager  = null;
        }

        /// <summary>
        /// Puts to end the home state.
        /// </summary>
        internal void EndHomeState()
        {
            this.State = HomeState.None;
        }

        /// <summary>
        /// Creates a fast forward of time.
        /// </summary>
        internal void FastForwardTime(int Seconds)
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
        internal int GetRandomInt(int Max)
        {
            return this.Random.Rand(Max);
        }

        /// <summary>
        /// Loads home state.
        /// </summary>
        internal void LoadHomeState(Player Player, int SecondsSinceLastSave, int RandomSeed)
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
        internal void LoadBattleState()
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
        internal void LoadReplay(string Json)
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
        internal void SetPlayer(Player Player)
        {
            if (this.Player != null)
            {
                Players.Save(this.Player);
            }

            this.Player                = Player;
            this.Player.GameMode       = this;
            this.Player.Home.GameMode  = this;
        }

        /// <summary>
        /// Sets the random seed.
        /// </summary>
        internal void SetRandomSeed(int Seed)
        {
            this.Random.Seed = Seed;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
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
        internal void UpdateSectorTicks(int Tick)
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
        internal void UpdateOneTick()
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
        internal void Encode(ChecksumEncoder Stream, bool EncodeCommandManager)
        {
            Stream.ResetChecksum();
            
            Stream.WriteVInt(this.Time);
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
            }
        }

        /// <summary>
        /// Encodes the first bytes of LogicGameMode.
        /// </summary>
        internal void EncodeOnce(ChecksumEncoder Stream)
        {
            Stream.WriteVInt(42); // Value != 42: decodeOnce stream is corrupted! #1
            
            Stream.WriteBoolean(false);

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
            }

            Stream.WriteVInt(43); // Value != 43: decodeOnce stream is corrupted! #2
        }
    }
}