namespace ClashRoyale.Logic.Battle
{
    using System;

    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Files.Csv.Tilemaps;
    using ClashRoyale.Logic.GameObject;
    using ClashRoyale.Logic.GameObject.Factory;
    using ClashRoyale.Logic.GameObject.Manager;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    using Newtonsoft.Json.Linq;

    public class Battle
    {
        public readonly GameMode GameMode;

        public bool InOvertime;
        public bool EndedCalled;
        public bool EndedWithTimeOut;

        public bool TournamentMode;
        public bool ChallengeMatch;
        public bool TeamVsTeamMatch;

        public int Type;
        public int DeckCount;
        public int PlayerCount;
        public int EndScreenLoadTime;

        public NpcData NpcData;
        public ArenaData ArenaData;
        public GameModeData GameModeData;
        public LocationData LocationData;
        
        public Player[] Players;
        public SpellDeck[] Decks;

        public GameObjectManager GameObjectManager;

        /// <summary>
        /// Gets the length of match in seconds.
        /// </summary>
        public int MatchLengthSeconds
        {
            get
            {
                if (this.TournamentMode && this.Type != 1)
                {
                    return Globals.TournamentMatchLengthSeconds;
                }

                return this.LocationData.MatchLength;
            }
        }

        /// <summary>
        /// Gets the length of overtime in seconds.
        /// </summary>
        public int OvertimeLengthSeconds
        {
            get
            {
                if (this.TournamentMode && this.Type != 1)
                {
                    return Globals.TournamentOvertimeLengthSeconds;
                }

                return this.LocationData.OvertimeSeconds;
            }
        }

        /// <summary>
        /// Gets the milliseconds gone.
        /// </summary>
        public int MillisecondsGone
        {
            get
            {
                return 50 * this.GameMode.Time;
            }
        }

        /// <summary>
        /// Gets the seconds left before the end of battle.
        /// </summary>
        public int SecondsLeft
        {
            get
            {
                int Passed      = this.GameMode.Time / 20 ;
                int MatchLength = this.MatchLengthSeconds;

                int Left        = MatchLength - Passed;

                if (this.InOvertime)
                {
                    Left += this.OvertimeLengthSeconds;
                }

                if (Left < 0)
                {
                    return 0;
                }

                return Left;
            }
        }

        /// <summary>
        /// Gets if this battle is spectate.
        /// </summary>
        public bool IsSpectate
        {
            get
            {
                return this.Type == 4;
            }
        }

        /// <summary>
        /// Gets if the battle is finished.
        /// </summary>
        public bool IsFinished
        {
            get
            {
                if (this.EndConditionMatched)
                {
                    if (this.EndScreenLoadTime >= this.LocationData.EndScreenDelay)
                    {
                        return true;
                    }
                }
                
                return false;
            }
        }

        /// <summary>
        /// Gets if the battle ended has been called.
        /// </summary>
        public bool IsBattleEndedCalled
        {
            get
            {
                return this.EndedCalled;
            }
        }

        /// <summary>
        /// Gets if the battle has been ended with time out.
        /// </summary>
        public bool IsBattleEndedWithTimeOut
        {
            get
            {
                return this.EndedWithTimeOut;
            }
        }

        /// <summary>
        /// Gets if the end condtion matched.
        /// </summary>
        public bool EndConditionMatched
        {
            get
            {
                if (this.EndScreenLoadTime > 0)
                {
                    return true;
                }

                int MatchLengthSecs = this.MatchLengthSeconds;
                int OvertimeLengthSecs = this.OvertimeLengthSeconds;

                if (MatchLengthSecs <= 0)
                {
                    int Passed = this.GameMode.Time / 20;

                    if (this.GameMode.Time / 20 < MatchLengthSecs + OvertimeLengthSecs)
                    {
                        if (Passed < MatchLengthSecs)
                        {
                            return false;
                        }

                        return false;
                    }

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        public Battle()
        {
            // Battle.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Battle"/> class.
        /// </summary>
        /// <param name="GameMode">The game mode.</param>
        public Battle(GameMode GameMode)
        {
            this.GameMode   = GameMode;

            this.Players    = new Player[4];
            this.Decks      = new SpellDeck[4];
        }

        /// <summary>
        /// Adds the player to battle.
        /// </summary>
        public void AddPlayer(Player Player, int Index)
        {
            if (Index < 4)
            {
                if (this.GetPlayerByAccountId(Player.HighId, Player.LowId) == null)
                {
                    if (this.Players[Index] == null)
                    {
                        this.Decks[Index] = Player.Home.SpellDeck.Clone();
                        this.Players[Index] = Player;

                        ++this.PlayerCount;
                    }
                    else
                    {
                        Logging.Error(this.GetType(), "AddPlayer() - A player already exist at specified index.");
                    }
                }
                else
                {
                    Logging.Error(this.GetType(), "AddPlayer() - Player with same index already set.");
                }
            }
            else
            {
                Logging.Error(this.GetType(), "AddPlayer() - Index is out of bounds (" + (Index + 1) + "/" + 4 + ")");
            }
        }

        /// <summary>
        /// Returns if the specified spell position is valid.
        /// </summary>
        public bool CheckSpellPosition(int X, int Y, SpellData SpellData)
        {
            return true;
        }

        /// <summary>
        /// Gets the player by id.
        /// </summary>
        public Player GetPlayerByAccountId(int HighId, int LowId)
        {
            return Array.Find(this.Players, T => T != null && T.HighId == HighId && T.LowId == LowId);
        }

        /// <summary>
        /// Gets the enemy players.
        /// </summary>
        public Player[] GetEnemyPlayers()
        {
            if (this.TeamVsTeamMatch)
            {
                Player[] Enemies = new Player[2];

                if (this.PlayerCount == 4)
                {
                    if (this.Players[0] == this.GameMode.Player || this.Players[2] == this.GameMode.Player)
                    {
                        Enemies[0] = this.Players[1];
                        Enemies[1] = this.Players[3];
                    }
                    else
                    {
                        Enemies[0] = this.Players[0];
                        Enemies[1] = this.Players[2];
                    }
                }

                return Enemies;
            }

            return null;
        }

        /// <summary>
        /// Initializes the sector.
        /// </summary>
        public void InitDefaultSector(TilemapData TileMapData)
        {
            for (int I = 0; I < TileMapData.Objects.Count; I++)
            {
                CsvData Data = TileMapData.Objects[I].Data;

                if (Data.Type != 42)
                {
                    GameObject GameObject = GameObjectFactory.CreateGameObjectByData(Data);
                    GameObject.SetPosition(500 * TileMapData.Objects[I].X, 500 * TileMapData.Objects[I].Y, 0);

                    if (GameObject.Type == 4)
                    {
                        // TODO : GameObject.Type == 4.
                    }
                }
            }
        }

        /// <summary>
        /// Gets a random value between 0 and <paramref name="Max"/>.
        /// </summary>
        public int Rand(int Max)
        {
            return this.GameMode.GetRandomInt(Max);
        }
        
        /// <summary>
        /// Sets the location.
        /// </summary>
        public void SetLocation(LocationData Data)
        {
            this.LocationData = Data;
        }

        /// <summary>
        /// Sets the arena.
        /// </summary>
        public void SetArena(ArenaData Data)
        {
            this.ArenaData = Data;
        }

        /// <summary>
        /// Sets the game mode.
        /// </summary>
        public void SetGameMode(GameModeData GameMode)
        {
            this.GameModeData = GameMode;
        }

        /// <summary>
        /// Saves this instance for replay.
        /// </summary>
        public JObject SaveReplay()
        {
            JObject Json = new JObject();

            Json.Add("gmt", 1);
            Json.Add("plt", 1);

            JsonHelper.SetLogicData(Json, "gamemode", this.GameModeData);

            Json.Add("t1s", 0);
            Json.Add("t2s", 0);

            for (int I = 0; I < 4; I++)
            {
                if (this.Decks[I] != null)
                {
                    Json.Add("deck" + I, this.Decks[I].Save());
                }
            }

            for (int I = 0; I < 4; I++)
            {
                if (this.Players[I] != null)
                {
                    Json.Add("avatar" + I, this.Players[I].Save(true));
                }
            }

            JsonHelper.SetLogicData(Json, "location", this.LocationData);
            JsonHelper.SetLogicData(Json, "arena", this.ArenaData);

            return Json;
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            if (!this.InOvertime && !this.IsBattleEndedCalled)
            {
                int MatchLengthSecs = this.MatchLengthSeconds;

                if ((MatchLengthSecs > 0 || this.Type != 1 || this.NpcData == null) && this.GameMode.Time / 20 >= MatchLengthSecs)
                {
                    this.InOvertime = true;
                }
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ChecksumEncoder Stream, Player Player)
        {
            Stream.EncodeLogicData(this.LocationData, 15);
            Stream.WriteVInt(this.PlayerCount);
            Stream.EncodeLogicData(this.NpcData, 48);
            Stream.EncodeLogicData(this.ArenaData, 54);
            
            for (int I = 0; I < this.PlayerCount; I++)
            {
                Stream.WriteLogicLong(this.Players[I].HighId, this.Players[I].LowId);
                Stream.WriteVInt(0);
            }
            
            Stream.EncodeConstantSizeIntArray(new int[8], 8);
            {
                Stream.WriteVInt(1);
                Stream.WriteVInt(1);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);

                Stream.WriteVInt(7); // EncodeLogicData (type=72)
                Stream.WriteVInt(0); // EncodeLogicData (type=79)
                Stream.WriteVInt(0); // EncodeLogicData (type=81)
            }

            Stream.WriteBoolean(this.EndedCalled);
            Stream.WriteBoolean(this.EndedWithTimeOut);
            Stream.WriteBoolean(false); // ?
            Stream.WriteBoolean(false); // hasPlayerFinishedNpcLevel
            Stream.WriteBoolean(this.InOvertime);
            Stream.WriteBoolean(false); // Live

            Stream.WriteVInt(this.Type);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            {
                // LogicGameObjectManager::encode().
                Stream.AddRange("00-B9-03-C7-7C-00-00-06-7A".HexaToBytes());
                
                Stream.WriteVInt(6); // Count

                Stream.EncodeData(CsvFiles.Get(Gamefile.Buildings).GetWithInstanceId(1)); // PrincessTower
                Stream.EncodeData(CsvFiles.Get(Gamefile.Buildings).GetWithInstanceId(1)); // PrincessTower
                Stream.EncodeData(CsvFiles.Get(Gamefile.Buildings).GetWithInstanceId(1)); // PrincessTower
                Stream.EncodeData(CsvFiles.Get(Gamefile.Buildings).GetWithInstanceId(1)); // PrincessTower
                Stream.EncodeData(CsvFiles.Get(Gamefile.Buildings).GetWithInstanceId(0)); // KingTower
                Stream.EncodeData(CsvFiles.Get(Gamefile.Buildings).GetWithInstanceId(0)); // KingTower

                // Type (Enemy, etc...)
                Stream.WriteVInt(1);
                Stream.WriteVInt(0);
                Stream.WriteVInt(1);
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
                Stream.WriteVInt(1);

                // ID (Like 500xxxxxx for CoC)
                Stream.AddRange("05-00".HexaToBytes());
                Stream.AddRange("05-01".HexaToBytes());
                Stream.AddRange("05-02".HexaToBytes());
                Stream.AddRange("05-03".HexaToBytes());
                Stream.AddRange("05-04".HexaToBytes());
                Stream.AddRange("05-05".HexaToBytes());

                // LogicGameObject::encode(): vInt, Vector2, vInt
                // GameObjectType:
                // 0: LogicDeco
                // 1: LogicSpawnPoint
                // 2: LogicAreaEffectObject
                // 3: LogicProjectile
                // 4: LogicCharacter
                Stream.AddRange("00-0D-A4-E2-01-9C-8E-03-00-00-7F-00-C0-7C-00-00-02-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode
                Stream.AddRange("00-0D-AC-36-A4-65-00-00-7F-00-80-04-00-00-01-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode
                Stream.AddRange("00-0D-AC-36-9C-8E-03-00-00-7F-00-C0-7C-00-00-01-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode
                Stream.AddRange("00-0D-A4-E2-01-A4-65-00-00-7F-00-80-04-00-00-02-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode

                if (this.Players[1] == Player)
                {
                    Stream.AddRange("00-0D-A8-8C-01-B8-2E-00-00-7F-00-80-04-00-00-00-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode
                    Stream.AddRange("0C-00-00-A0-9A-0C-00-00-00-00-00-7F-7F-7F-7F-7F-7F-7F-7F-00-00-00-00".HexaToBytes()); // LogicSummoner::encode

                    // LogicSummoner::encode
                    Stream.AddRange("00-0D-A8-8C-01-88-C5-03-00-00-7F-00-C0-7C-00-00-00-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode

                    Stream.WriteBoolean(true); // HAS DECK
                    Stream.WriteBoolean(false); // ?
                    Stream.WriteBoolean(true); // AI ENABLED ?

                    if (true)
                    {
                        // If HAS DECK
                        Stream.WriteVInt(4);
                        {
                            // ByteStreamHelper::writeConstantSizeIntArray(4)
                            Stream.WriteVInt(6);
                            Stream.WriteVInt(-1);
                            Stream.WriteVInt(-4);
                            Stream.WriteVInt(-1);
                        }

                        Stream.WriteVInt(4);
                        {
                            // Count
                            Stream.WriteVInt(3);
                            Stream.WriteVInt(4);
                            Stream.WriteVInt(2);
                            Stream.WriteVInt(7);
                        }

                        Stream.WriteVInt(0);
                        {
                            // Same, Is Count
                            
                        }

                        Stream.WriteVInt(-1);
                        Stream.WriteVInt(-1);
                        Stream.WriteVInt(0);

                        if (Globals.QuestsEnabled)
                        {
                            Stream.WriteVInt(0); // Count

                            // LogicQuestManager::encode()
                        }
                    }

                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0); // MsBeforeStartRegenMana
                    Stream.WriteVInt(5); // Mana
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                    {
                        // LogicSpellAIBlackboard::encode
                        Stream.WriteVInt(0);
                        Stream.WriteVInt(0);
                    }

                    for (int I = 0; I < 8; I++)
                    {
                        Stream.WriteVInt(-1);
                    }

                    Stream.WriteBoolean(false);
                    {
                        // ?
                    }

                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                }
                else
                {
                    Stream.AddRange("00-0D-A8-8C-01-B8-2E-00-00-7F-00-80-04-00-00-00-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode

                    Stream.WriteBoolean(true); // HAS DECK
                    Stream.WriteBoolean(false); // ?
                    Stream.WriteBoolean(true); // AI ENABLED ?

                    if (true)
                    {
                        // If HAS DECK
                        Stream.WriteVInt(4);
                        {
                            // ByteStreamHelper::writeConstantSizeIntArray(4)
                            Stream.WriteVInt(6);  // 6 = SpellDeck[6]
                            Stream.WriteVInt(-1); // 6 - 1 = SpellDeck[6 - 1]
                            Stream.WriteVInt(-4); // 6 - 1 - 4 = SpellDeck[6 - 1 - 4]
                            Stream.WriteVInt(-1); // 6 - 1 - 4 - 1 =  = SpellDeck[6 - 1 - 4 - 1]
                        }

                        Stream.WriteVInt(4);
                        {
                            // Spell Queue Count
                            Stream.WriteVInt(3); 
                            Stream.WriteVInt(4);
                            Stream.WriteVInt(2);
                            Stream.WriteVInt(7);
                        }

                        Stream.WriteVInt(0);
                        {
                            // Spell Queue 2 Count
                        }

                        Stream.WriteVInt(-1);
                        Stream.WriteVInt(-1);
                        Stream.WriteVInt(0);

                        if (Globals.QuestsEnabled)
                        {
                            Stream.WriteVInt(0); // Count

                            // LogicQuestManager::encode()
                        }
                    }

                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0); // MsBeforeStartRegenMana
                    Stream.WriteVInt(5); // Mana
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                    {
                        // LogicSpellAIBlackboard::encode
                        Stream.WriteVInt(0);
                        Stream.WriteVInt(0);
                    }

                    for (int I = 0; I < 8; I++)
                    {
                        Stream.WriteVInt(-1);
                    }

                    Stream.WriteBoolean(false);
                    {
                        // ?
                    }

                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);
                    Stream.WriteVInt(0);

                    Stream.AddRange("00-0D-A8-8C-01-88-C5-03-00-00-7F-00-C0-7C-00-00-00-00-00-00-00-00".HexaToBytes()); // LogicCharacter::encode
                    Stream.AddRange("0C-00-00-A0-9A-0C-00-00-00-00-00-7F-7F-7F-7F-7F-7F-7F-7F-00-00-00-00".HexaToBytes()); // LogicSummoner::encode
                }

                // -- COMPONENT --
                Stream.AddRange("00-00-00-00-00-00-00-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-00".HexaToBytes());

                // LogicHitpointComponent
                Stream.AddRange("B8-15-00".HexaToBytes());
                Stream.AddRange("B8-15-00".HexaToBytes());
                Stream.AddRange("B8-15-00".HexaToBytes());
                Stream.AddRange("B8-15-00".HexaToBytes());
                Stream.AddRange("A0-25-00".HexaToBytes());
                Stream.AddRange("A0-25-00".HexaToBytes());

                // LogicCharacterBuffComponent
                Stream.AddRange("00-00-00-00-00-00-00-A4-01-A4-01-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-A4-01-A4-01-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-A4-01-A4-01-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-A4-01-A4-01-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-A4-01-A4-01-00".HexaToBytes());
                Stream.AddRange("00-00-00-00-00-00-00-A4-01-A4-01-00".HexaToBytes());
            }

            for (int I = 0; I < 2; I++)
            {
                if (this.Players[I] == Player)
                {
                    Stream.WriteBoolean(true);
                    this.Decks[I].EncodeAttack(Stream);
                    break;
                }

                Stream.WriteBoolean(false);
            }
            
            Stream.AddRange("00-00-05-06-02-02-04-02-01-03-00-00-00-00-00-00-00-00".HexaToBytes());
        }
    }
}