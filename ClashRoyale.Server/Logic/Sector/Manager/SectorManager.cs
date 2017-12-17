namespace ClashRoyale.Server.Logic.Sector.Manager
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Server.Extensions;
    using ClashRoyale.Server.Logic.Battle.Event;
    using ClashRoyale.Server.Logic.Commands;
    using ClashRoyale.Server.Logic.Commands.Storage;
    using ClashRoyale.Server.Logic.Enums;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;
    using ClashRoyale.Server.Logic.Time;
    using ClashRoyale.Server.Network.Packets.Server;

    using Math = ClashRoyale.Server.Logic.Math.Math;

    internal class SectorManager
    {
        internal GameMode GameMode;

        internal Time Time;
        internal CommandStorage Queue;
        internal CommandStorage Commands;
        
        internal int LastClientTurn;
        internal object Locker;

        internal byte[] Update
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
        internal void SetCommandQueue(CommandStorage CommandStorage)
        {
            this.Queue = CommandStorage;
        }

        /// <summary>
        /// Sets the command storage instance.
        /// </summary>
        internal void SetCommandStorage(CommandStorage CommandStorage)
        {
            this.Commands = CommandStorage;
        }

        /// <summary>
        /// Sets the battle locker.
        /// </summary>
        internal void SetLocker(object Locker)
        {
            this.Locker = Locker;
        }

        /// <summary>
        /// Sends a sector state.
        /// </summary>
        internal void ReceiveBattleEvent(BattleEvent Event)
        {
            if (Event.SenderId != this.GameMode.Device.NetworkManager.AccountId)
            {
                Logging.Error(this.GetType(), "ReceiveBattleEvent() - Sender id is not valid. AccountId:" + this.GameMode.Device.NetworkManager.AccountId + " SenderId:" + Event.HighId + "-" + Event.LowId);
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
        internal void ReceiveSectorCommand(int ClientTick, int ClientChecksum, Command Command)
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
        internal void EndBattle()
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
        internal void SendBattleEvent(BattleEvent Event)
        {
            this.GameMode.Device.NetworkManager.SendMessage(new BattleEventMessage(this.GameMode.Device, Event));
        }

        /// <summary>
        /// Sends the battle result.
        /// </summary>
        internal void SendBattleResult()
        {
            this.GameMode.Device.NetworkManager.SendMessage(new BattleResultMessage(this.GameMode.Device, this.Update));
        }

        /// <summary>
        /// Sends a sector state.
        /// </summary>
        internal void SendSectorState()
        {
            this.GameMode.Device.NetworkManager.SendMessage(new SectorStateMessage(this.GameMode.Device, this.Update));
        }

        /// <summary>
        /// Sends a sector heatbeat.
        /// </summary>
        internal void SendSectorHeartbeat(int Time, int Checksum, List<Command> Commands)
        {
            this.GameMode.Device.NetworkManager.SendMessage(new SectorHearbeatMessage(this.GameMode.Device, Time, Checksum, Commands));
        }

        /// <summary>
        /// Called when the oppenent left the match.
        /// </summary>
        internal void OpponentLeftMatch()
        {
            this.GameMode.Device.NetworkManager.SendMessage(new OpponentLeftMatchNotificationMessage(this.GameMode.Device));
        }

        /// <summary>
        /// Called when the oppenent rejoins the match.
        /// </summary>
        internal void OpponentRejoinsMatch()
        {
            this.GameMode.Device.NetworkManager.SendMessage(new OpponentRejoinsMatchNotificationMessage(this.GameMode.Device));
        }
        
        /// <summary>
        /// Updates the logic tick.
        /// </summary>
        internal void IncreaseTick()
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

                this.SendSectorHeartbeat(this.Time / 10, this.GameMode.Checksum, this.Queue.Commands);
            }

            this.Time.IncreaseTick();
        }
    }
}