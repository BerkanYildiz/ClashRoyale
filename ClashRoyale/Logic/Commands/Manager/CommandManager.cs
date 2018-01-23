namespace ClashRoyale.Logic.Commands.Manager
{
    using System.Collections.Generic;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Logic.Commands.Server;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Messages.Server.Home;

    using Newtonsoft.Json.Linq;

    public class CommandManager
    {
        public int Seed;

        public GameMode GameMode;
        public List<Command> Queue;
        public Dictionary<int, Command> AvailableServerCommands;

        // FOR SERVER

        public bool WaitJoinAllianceTurn;
        public bool WaitLeaveAllianceTurn;
        public bool WaitChangeAvatarNameTurn;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandManager"/> class.
        /// </summary>
        public CommandManager(GameMode GameMode)
        {
            this.GameMode = GameMode;
            this.Queue    = new List<Command>(32);
            this.AvailableServerCommands = new Dictionary<int, Command>(16);
        }

        /// <summary>
        /// Adds a available server command.
        /// </summary>
        public void AddAvailableServerCommand(ServerCommand Command)
        {
            if (this.GameMode.IsConnected)
            {
                this.AvailableServerCommands.Add(Command.Id = ++this.Seed, Command);
                this.GameMode.Listener.SendMessage(new AvailableServerCommandMessage(Command));
            }
            else
            {
                Command.Execute(this.GameMode);
            }
        }

        /// <summary>
        /// Adds a available server command.
        /// </summary>
        public void AddCommand(Command Command)
        {
            if (Command.IsServerCommand)
            {
                ServerCommand RServerCommand = (ServerCommand) Command;

                if (this.AvailableServerCommands.TryGetValue(RServerCommand.Id, out Command ACommand))
                {
                    ServerCommand AServerCommand = (ServerCommand) ACommand;

                    ChecksumEncoder Encoder1 = new ChecksumEncoder(new ByteStream());
                    ChecksumEncoder Encoder2 = new ChecksumEncoder(new ByteStream());

                    AServerCommand.Encode(Encoder1);
                    RServerCommand.Encode(Encoder2);

                    if (Encoder1.Checksum != Encoder2.Checksum)
                    {
                        return;
                    }

                    if (this.AvailableServerCommands.Remove(RServerCommand.Id))
                    {
                        AServerCommand.ExecuteTick = Command.ExecuteTick;
                        AServerCommand.TickWhenGiven = Command.TickWhenGiven;
                        AServerCommand.ExecutorId = Command.ExecutorId;

                        Command = AServerCommand;
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "Unable to remove the server command of dictionary!");
                        return;
                    }
                }
                else
                {
                    Logging.Info(this.GetType(), "Server command id is invalid. (" + RServerCommand.Id + ")");
                    return;
                }
            }

            this.Queue.Add(Command);
        }

        /// <summary>
        /// Gets if the specified command is allowed in current state.
        /// </summary>
        public bool IsCommandAllowedInCurrentState(Command Command)
        {
            if (Command.Type < 1000)
            {
                if (Command.Type >= 500 && Command.Type < 600)
                {
                    if (this.GameMode.State != HomeState.Home)
                    {
                        Logging.Error(this.GetType(), "Execute command failed! Command is only allowed in home state. Command: " + Command.Type);
                    }
                    else if (this.GameMode.State == HomeState.Visit)
                    {
                        Logging.Error(this.GetType(), "Execute command failed! Command is only allowed in visit state. Command: " + Command.Type);
                    }
                }
            }
            else
            {
                Logging.Error(this.GetType(), "Execute command failed! Debug commands are not allowed when debug is off.");
            }

            return true;
        }

        /// <summary>
        /// Creates the specified command.
        /// </summary>
        public static Command CreateCommand(int Type)
        {
            if (Type == 1000)
            {
                if (Config.IsDevelopment)
                {
                    // TODO : Handle DebugCommand().
                }
                else
                {
                    Logging.Error(typeof(CommandManager), "CreateCommand() - Debug command is not allowed when debug is off.");
                }
            }

            
            if (Type < 300)
            {
                if (Type >= 200)
                {
                    switch (Type)
                    {
                        /* case 205:
                            return new LeaveAllianceCommand();
                        case 206:
                            return new JoinAllianceCommand();
                        case 208:
                            return new AllianceUnitReceivedCommand();
                        case 210:
                            return new ClaimRewardCommand();
                        case 211:
                            return new AddChestCommand();
                        case 215:
                            return new TransactionsRevokedCommand(); */
                        case 275:
                            return new DiamondsRemovedCommand();
                        case 278:
                            return new ChangeAvatarNameCommand();
                    }
                }

                switch (Type)
                {
                    /* case 1:
                        return new DoSpellCommand(); */
                }
            }

            switch (Type)
            {
                case 501:
                    return new UnknownCommand();
                case 505:
                    return new SwapSpellsCommand();
                case 511:
                    return new BuyResourcePackCommand();
                case 512:
                    return new SelectDeckCommand();
                case 513:
                    return new FilterClanChatCommand();
                case 517:
                    return new SpellSeenCommand();
                case 518:
                    return new RefreshAchievementsCommand();
                case 522:
                    return new QuestsSeenCommand();
                /* case 523:
                    return new ClaimAchievementRewardCommand();
                case 538:
                    return new SpellPageOpenedCommand(); */
                case 539:
                    return new BuyChestCommand();
                case 544:
                    return new BuySpellCommand();
                case 548:
                    return new BuyDiamondsCommand();
                case 576:
                    return new UpdateLastShownLevelUpCommand();
                case 580:
                    return new CopyDeckCommand();
                case 584:
                    return new RequestNewQuestCommand();
                case 592:
                    return new FuseSpellsCommand();
                case 594:
                    return new StartMatchmakeCommand();
                case 599:
                    return new PageOpenedCommand();
            }

            Logging.Info(typeof(CommandManager), "CreateCommand() - Command type " + Type + " does not exist.");

            return null;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Encoder)
        {
            Encoder.EnableCheckSum(false);

            Encoder.WriteVInt(this.Queue.Count);

            this.Queue.ForEach(Command =>
            {
                CommandManager.EncodeCommand(Command, Encoder);
            });

            Encoder.EnableCheckSum(true);
        }

        /// <summary>
        /// Saves the specified command to json.
        /// </summary>
        public static JObject SaveCommandToJson(Command Command)
        {
            JObject Json = new JObject();

            Json.Add("ct", Command.Type);
            Json.Add("c", Command.Save());

            return Json;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            if (this.GameMode.IsImmediateMessageExecution)
            {
                for (int I = 0; I < this.Queue.Count; I++)
                {
                    Command Command = this.Queue[I];

                    if (Command.ExecuteTick == this.GameMode.Time)
                    {
                        this.Queue.RemoveAt(I--);

                        if (!this.IsCommandAllowedInCurrentState(Command))
                        {
                            Logging.Error(this.GetType(), "IsCommandAllowedInCurrentState(Command, " + this.GameMode.State +") != true at " + Command.GetType().Name + "().");
                            this.Queue.RemoveAt(I--);
                            continue;
                        }

                        int FailCode = Command.Execute(this.GameMode);

                        if (FailCode != 0)
                        {
                            Logging.Warning(Command.GetType(), "FailCode == " + FailCode + " at Execute(GameMode).");
                        }
                        else
                        {
                            if (this.GameMode.Replay != null)
                            {
                                this.GameMode.Replay.RecordCommand(Command);
                            }
                        }
                    }
                    else
                    {
                        if (Command.ExecuteTick < this.GameMode.Time)
                        {
                            // Debugger.Error("Execute command failed! Command should have been executed already. (type=" + Command.Type + " server_tick=" + this.GameMode.Time + " command_tick=" + Command.ExecuteTick + ")");
                            this.Queue.RemoveAt(I--);
                        }
                    }
                }
            }
            else
            {
                int Tick = this.GameMode.Time;

                for (int I = 0; I < this.Queue.Count; I++)
                {
                    Command Command = this.Queue[I];

                    if (Command.GetAgeInTicks(Tick) > 21)
                    {
                        // Logging.Info(this.GetType(), "Command was late. Tick when given modified by " + (Tick - Command.TickWhenGiven));
                        Command.TickWhenGiven = Tick - 20;
                    }

                    if (Command.GetAgeInTicks(Tick) == 20)
                    {
                        this.Queue.RemoveAt(I--);

                        Command.ExecuteTick = Tick;

                        int FailCode = Command.Execute(this.GameMode);

                        if (FailCode != 0)
                        {
                            // Debugger.Warning("Failed to execute command: " + Command.Type + " failCode: " + FailCode);
                        }
                        else
                        {
                            if (this.GameMode.Replay != null)
                            {
                                this.GameMode.Replay.RecordCommand(Command);
                            }
                        }

                        continue;
                    }

                    if (Command.GetAgeInTicks(Tick) > 20)
                    {
                        // Debugger.Error("Execute command failed! Command should have been executed already. (type=" + Command.Type + " current_tick=" + this.GameMode.Time + " given=" + Command.TickWhenGiven + ")");
                        this.Queue.RemoveAt(I--);
                    }
                }
            }
        }

        /// <summary>
        /// Decodes a command.
        /// </summary>
        public static Command DecodeCommand(ByteStream Stream)
        {
            Command Command = CommandManager.CreateCommand(Stream.ReadVInt());

            if (Command != null)
            {
                Command.Decode(Stream);
            }

            return Command;
        }

        /// <summary>
        /// Encodes the specified command.
        /// </summary>
        public static void EncodeCommand(Command Command, ChecksumEncoder Encoder)
        {
            Encoder.WriteVInt(Command.Type);
            Command.Encode(Encoder);
        }
    }
}