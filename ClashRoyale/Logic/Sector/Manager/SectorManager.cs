namespace ClashRoyale.Logic.Sector.Manager
{
    using System;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    using ClashRoyale.Logic.Battle.Event;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Commands.Storage;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Logic.Time;

    using ClashRoyale.Messages.Server.Attack;
    using ClashRoyale.Messages.Server.Avatar;
    using ClashRoyale.Messages.Server.Sector;

    using Math = ClashRoyale.Maths.Math;

    public class SectorManager
    {
        public GameMode GameMode;

        public Time Time;
        public CommandStorage Queue;
        public CommandStorage Commands;
        
        public int LastClientTurn;
        public object Locker;

        public byte[] Update
        {
            get
            {
                ChecksumEncoder Encoder = new ChecksumEncoder(new ByteStream());

                this.GameMode.EncodeOnce(Encoder);
                this.GameMode.Encode(Encoder, true);

                return Encoder.ByteStream.ToArray();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorManager"/> class.
        /// </summary>
        public SectorManager(GameMode GameMode)
        {
            this.GameMode = GameMode;
        }

        /// <summary>
        /// Sets the command queue instance.
        /// </summary>
        public void SetCommandQueue(CommandStorage CommandStorage)
        {
            this.Queue = CommandStorage;
        }

        /// <summary>
        /// Sets the command storage instance.
        /// </summary>
        public void SetCommandStorage(CommandStorage CommandStorage)
        {
            this.Commands = CommandStorage;
        }

        /// <summary>
        /// Sets the battle locker.
        /// </summary>
        public void SetLocker(object Locker)
        {
            this.Locker = Locker;
        }

        /// <summary>
        /// Sends a sector state.
        /// </summary>
        public void ReceiveBattleEvent(BattleEvent Event)
        {
            if (Event.SenderId != this.GameMode.Player.PlayerId)
            {
                Logging.Error(this.GetType(), "ReceiveBattleEvent() - Sender id is not valid. AccountId:" + this.GameMode.Player.PlayerId + " SenderId:" + Event.HighId + "-" + Event.LowId);
                return;
            }

            foreach (Player Player in this.GameMode.Battle.Players)
            {
                if (Player != null)
                {
                    Player.GameMode.SectorManager.SendBattleEvent(Event);

                    if (Player.GameMode.Replay != null)
                    {
                        Player.GameMode.Replay.RecordEvent(Event);
                    }
                }
            }
        }

        /// <summary>
        /// Called when the server receive a sector command.
        /// </summary>
        public void ReceiveSectorCommand(int ClientTick, int ClientChecksum, Command Command)
        {
            this.LastClientTurn = ClientTick;

            if (Command != null)
            {
                if (Command.Type < 100)
                {
                    switch (Command.Type)
                    {
                        case 1:
                        {
                            DoSpellCommand DoSpellCommand = (DoSpellCommand) Command;
                            DoSpellCommand.Spell = this.GameMode.Home.GetSpellByData(DoSpellCommand.SpellData);

                            if (DoSpellCommand.Spell == null)
                            {
                                return;
                            }
                            
                            break;
                        }
                    }   
                }

                lock (this.Locker)
                {
                    this.Queue.AddCommand(Command);
                }
            }

            // TODO : Implement checksum checking.
        }

        /// <summary>
        /// Called for end the battle.
        /// </summary>
        public void EndBattle()
        {
            if (this.GameMode.State == HomeState.Attack)
            {
                if (this.GameMode.Battle.IsBattleEndedCalled)
                {
                    throw new Exception("SectorManager: EndBattle already called.");
                }

                this.SendBattleResult();
                this.GameMode.EndBattleState();
            }
        }
        
        /// <summary>
        /// Sends a battle event.
        /// </summary>
        public void SendBattleEvent(BattleEvent Event)
        {
            this.GameMode.Listener.SendMessage(new BattleEventMessage(Event));
        }

        /// <summary>
        /// Sends the battle result.
        /// </summary>
        public void SendBattleResult()
        {
            this.GameMode.Listener.SendMessage(new BattleResultMessage(this.Update));
        }

        /// <summary>
        /// Sends a sector state.
        /// </summary>
        public void SendSectorState()
        {
            this.GameMode.Listener.SendMessage(new SectorStateMessage(this.Update));
        }

        /// <summary>
        /// Sends a sector heatbeat.
        /// </summary>
        public void SendSectorHeartbeat(int Time, int Checksum, Command[] Commands)
        {
            this.GameMode.Listener.SendMessage(new SectorHearbeatMessage(Time, Checksum, Commands));
        }

        /// <summary>
        /// Called when the oppenent left the match.
        /// </summary>
        public void OpponentLeftMatch()
        {
            this.GameMode.Listener.SendMessage(new OpponentLeftMatchNotificationMessage());
        }

        /// <summary>
        /// Called when the oppenent rejoins the match.
        /// </summary>
        public void OpponentRejoinsMatch()
        {
            this.GameMode.Listener.SendMessage(new OpponentRejoinsMatchNotificationMessage());
        }
        
        /// <summary>
        /// Updates the logic tick.
        /// </summary>
        public void IncreaseTick()
        {
            if (this.Time == 0)
            {
                this.GameMode.Time = new Time();
                this.SendSectorState();
            }

            this.GameMode.UpdateSectorTicks(this.Time);

            if (this.Time == 10 * (this.Time / 10))
            {
                this.Queue.Commands.ForEach(Command =>
                {
                    Command.TickWhenGiven = Math.Clamp(Command.TickWhenGiven, Command.TickWhenGiven - 5, Command.TickWhenGiven + 20);
                    this.GameMode.CommandManager.AddCommand(Command);
                });

                this.SendSectorHeartbeat(this.Time / 10, this.GameMode.Checksum, this.Queue.Commands.ToArray());
            }

            this.Time.IncreaseTick();
        }
    }
}