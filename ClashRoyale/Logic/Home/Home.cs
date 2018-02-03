namespace ClashRoyale.Logic.Home
{
    using System;
    using System.Collections.Generic;

    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Game;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Converters;
    using ClashRoyale.Logic.Home.Spells;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;
    using ClashRoyale.Logic.Shop.Items;
    using ClashRoyale.Logic.Time;

    using Newtonsoft.Json;

    using Math = ClashRoyale.Maths.Math;

    [JsonObject(MemberSerialization.OptIn)]
    public class Home
    {
        public GameMode GameMode;

        [JsonProperty("highId")]                    public int HighId;
        [JsonProperty("lowId")]                     public int LowId;
       
        // SHOP

        [JsonProperty("shopSeed")]                  public int ShopSeed;
        [JsonProperty("shopDay")]                   public int ShopDay;
        [JsonProperty("shopDaySeen")]               public int ShopDaySeen;
        [JsonProperty("dayOfYear")]                 public int DayOfYear;

        [JsonProperty("shopTimer")]                 public Timer ShopTimer;

        [JsonProperty("shopItems")]                 public List<ShopItem> ShopItems;
        [JsonProperty("shopChests")]                public List<ChestEvent> ShopChests;

        // CHEST

        [JsonProperty("runningChestId")]            public int RunningChestId;

        [JsonProperty("freeChest")]                 public Chest FreeChest;
        [JsonProperty("starChest")]                 public Chest StarChest;
        [JsonProperty("shopChest")]                 public Chest ShopChest;
        [JsonProperty("clanCrownChest")]            public Chest ClanCrownChest;
        [JsonProperty("purchasedChest")]            public Chest PurchasedChest;

        [JsonProperty("chests")]                    public List<Chest> Chests;

        [JsonProperty("starChestCooldown")]         public bool StarChestCooldown;

        [JsonProperty("chestSlotCnt")]              public int ChestSlotCount;
        [JsonProperty("freeChestCnt")]              public int FreeChestCount;
        [JsonProperty("starChestCounter")]          public int StarChestCounter;
        [JsonProperty("crownChestIdx")]             public int CrownChestIdx;
        [JsonProperty("freeChestIdx")]              public int FreeChestIdx;

        [JsonProperty("freeChestTimer")]            public Timer FreeChestTimer;
        [JsonProperty("starChestTimer")]            public Timer StarChestTimer;

        // SEEN

        [JsonProperty("pageOpenedBits")]            public int PageOpened;
        [JsonProperty("lastLevelUpPopup")]          public int LastLevelUpPopup;

        [JsonProperty("lastArena")]                 public CsvData LastArena;

        [JsonProperty("newArenasSeen")]             public List<CsvData> NewArenasSeen;
        [JsonProperty("replaysSeen")]               public List<int> ReplaysSeen;

        // PACKS

        [JsonProperty("boughtPacks")]               public List<CsvData> BoughtPacks;

        // SPELL

        [JsonConverter(typeof(SavedDecksConverter))]
        [JsonProperty("savedDecks")]                public int[][] SavedDecks;

        [JsonProperty("selectedDeck")]              public int SelectedDeck;
        

        [JsonProperty("decks")]                     public SpellDeck SpellDeck;
        [JsonProperty("collection")]                public SpellCollection SpellCollection;

        // OTHER TIMER

        [JsonProperty("donationCooldownTimer")]     public Timer DonationCapacityCooldownTimer;
        [JsonProperty("requestCooldownTimer")]      public Timer RequestCooldownTimer;
        [JsonProperty("shareTimer")]                public Timer ShareTimer;
        [JsonProperty("sendMail")]                  public Timer SendMailTimer;
        [JsonProperty("elderKick")]                 public Timer ElderKickTimer;

        // TV

        // EVENT

        // TOURNAMENT

        // TUTORIAL

        public int Tutorial;

        // OTHER

        private bool ClaimingReward;

        public int DonationCapacityLimit;
        public DateTime LastTick;

        /// <summary>
        /// Gets the home ID.
        /// </summary>
        public long HomeId
        {
            get
            {
                return (long) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets the checksum of this instance.
        /// </summary>
        public int Checksum
        {
            get
            {
                return this.RunningChestId + (this.SpellCollection.Count << 16);
            }
        }

        /// <summary>
        /// Gets the number of assigned workers.
        /// </summary>
        public int AssignedWorkers
        {
            get
            {
                int Count = 0;

                for (int I = 0; I < this.Chests.Count; I++)
                {
                    if (this.Chests[I] != null)
                    {
                        if (this.Chests[I].IsUnlocking)
                        {
                            ++Count;
                        }
                    }
                }

                return Count;
            }
        }

        /// <summary>
        /// Gets the number of free workers.
        /// </summary>
        public int FreeWorkers
        {
            get
            {
                return Globals.MaxChestOpening - this.AssignedWorkers;
            }
        }

        /// <summary>
        /// Gets total seconds since last save.
        /// </summary>
        public int SecondsSinceLastSave
        {
            get
            {
                return Math.Max((int) DateTime.UtcNow.Subtract(this.LastTick).TotalSeconds, 0);
            }
        }

        /// <summary>
        /// Gets the number of spells.
        /// </summary>
        public int SpellCount
        {
            get
            {
                return this.SpellDeck.SpellCount + this.SpellCollection.Count;
            }
        }

        /// <summary>
        /// Gets the number of locked spell.
        /// </summary>
        public int LockedSpellCount
        {
            get
            {
                int Count = 0;

                CsvFiles.Spells.ForEach(SpellData =>
                {
                    if (!this.HasSpell(SpellData))
                    {
                        ++Count;
                    }
                });

                return Count;
            }
        }

        /// <summary>
        /// Gets if the player has all cards full.
        /// </summary>
        public bool HasAllCardsFull
        {
            get
            {
                for (int I = this.SpellCount - 1; I >= 0; I--)
                {
                    // TODO : Implement ClientHome::hasAllCardsFull().
                }

                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        public Home()
        {
            // SHOP

            this.ShopItems                      = new List<ShopItem>(8);
            this.ShopChests                     = new List<ChestEvent>(3);

            this.ShopTimer                      = new Timer();
            this.ShopTimer.StartTimer(86400);

            this.ShopDay                        = DateTime.UtcNow.Day;
            this.ShopDaySeen                    = DateTime.UtcNow.Day;
            this.ShopSeed                       = 346;

            // CHEST

            this.FreeChestTimer                 = new Timer();
            this.StarChestTimer                 = new Timer();

            this.Chests                         = new List<Chest>(8);
            
            // SEEN

            this.LastLevelUpPopup               = 1;
            this.ReplaysSeen                    = new List<int>(15);
            this.NewArenasSeen                  = new List<CsvData>(1); // 54000010

            // PACKS

            this.BoughtPacks                    = new List<CsvData>(0);

            // SPELL

            this.SpellDeck                      = new SpellDeck();
            this.SpellCollection                = new SpellCollection();

            this.SavedDecks                     = new int[5][];

            for (int i = 0; i < 5; i++)
            {
                this.SavedDecks[i]              = new int[8];
            }

            // OTHER TIMER

            this.DonationCapacityCooldownTimer  = new Timer();
            this.RequestCooldownTimer           = new Timer();
            this.ShareTimer                     = new Timer();
            this.SendMailTimer                  = new Timer();
            this.ElderKickTimer                 = new Timer();

            // TV

            // EVENT

            // TOURNAMENT

            // TUTORIAL
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Home"/> class.
        /// </summary>
        /// <param name="HighId">The high identifier.</param>
        /// <param name="LowId">The low identifier.</param>
        public Home(int HighId, int LowId) : this()
        {
            this.HighId = HighId;
            this.LowId  = LowId;
        }

        /// <summary>
        /// Adds the specified spell.
        /// </summary>
        public void AddSpell(Spell Spell)
        {
            if (!this.HasSpell(Spell.Data))
            {
                int DeckSpellCount = this.SpellDeck.SpellCount;

                if (DeckSpellCount != 8)
                {
                    this.SpellDeck.SetSpell(this.SpellDeck.SpellCount, Spell);
                }
                else
                {
                    this.SpellCollection.AddSpell(Spell);
                }

                return;
            }

            Logging.Error(this.GetType(), "Trying to add spell that already exists in collection, data:" + Spell.Data.GlobalId);
        }

        /// <summary>
        /// Adds the specified chest.
        /// </summary>
        public void AddChest(Chest Chest, int Idx)
        {
            // TODO : Implement AddChest().
        }

        /// <summary>
        /// Clears the shop item.
        /// </summary>
        public void ClearShopItems()
        {
            this.ShopItems.Clear();
        }

        /// <summary>
        /// Clears the shop item.
        /// </summary>
        public void CreateFreeChest()
        {
            if (this.FreeChest != null)
            {
                Logging.Info(this.GetType(), "CreateFreeChest() - _FreeChest should be null");
            }
            
            this.FreeChest = new Chest(this.GameMode.Player.Arena.FreeChestData);
            ++this.RunningChestId;
        }

        /// <summary>
        /// Called when the specified chest has been purchased.
        /// </summary>
        public void ChestPurchased(TreasureChestData Data, int Source)
        {
            if (Data == null)
            {
                Logging.Error(this.GetType(), "ChestPurchased() - chest data is NULL");
                return;
            }

            if (this.PurchasedChest != null)
            {
                Logging.Error(this.GetType(), "ChestPurchased() - Previous purchased chest not collected");
                return;
            }

            ++this.RunningChestId;

            this.PurchasedChest = new Chest(Data);
            this.PurchasedChest.SetSource(Source);
            this.PurchasedChest.SetClaimed(this.GameMode, 4);
        }

        /// <summary>
        /// Called when the crown chest has been collected.
        /// </summary>
        public void CrownChestCollected()
        {
            this.StarChest = null;

            if (this.StarChestTimer.IsFinished)
            {
                this.StartStarChestTimer(0);
                return;
            }

            this.StarChestCooldown = true;
        }

        /// <summary>
        /// Called when the free chest has been collected.
        /// </summary>
        public void FreeChestCollected()
        {
            this.FreeChest = null;
            this.FreeChestCount++;
        }

        /// <summary>
        /// Called when the purchased chest has been collected.
        /// </summary>
        public void PurchasedChestCollected()
        {
            this.PurchasedChest = null;
        }

        /// <summary>
        /// Gets the spell at specified index.
        /// </summary>
        public Spell GetSpellAt(int Index)
        {
            int DeckSpellCount = this.SpellDeck.SpellCount;

            if (DeckSpellCount > Index)
            {
                return this.SpellDeck[Index];
            }

            if (this.SpellCollection.Count > Index - DeckSpellCount)
            {
                return this.SpellCollection[Index];
            }

            Logging.Error(this.GetType(), "GetSpellAt() - Index out of bounds.");

            return null;
        }

        /// <summary>
        /// Gets spell by data.
        /// </summary>
        public Spell GetSpellByData(SpellData Data)
        {
            Spell Spell = this.SpellDeck.GetSpellByData(Data);

            if (Spell == null)
            {
                Spell = this.SpellCollection.GetSpellByData(Data);
            }

            return Spell;
        }

        /// <summary>
        /// Gets if the player have the specified spell.
        /// </summary>
        public bool HasSpell(SpellData Data)
        {
            return this.GetSpellByData(Data) != null;
        }

        /// <summary>
        /// Adds all spells not owned by the player.
        /// </summary>
        public void AddAllSpells()
        {
            foreach (SpellData SpellData in CsvFiles.Spells)
            {
                if (this.HasSpell(SpellData))
                {
                    continue;
                }

                this.AddSpell(new Spell(SpellData));
            }
        }

        /// <summary>
        /// Max all spells owned by the player.
        /// </summary>
        public void MaxAllSpells()
        {
            foreach (var Spell in this.SpellDeck.GetSpells())
            {
                Spell.SetMaterialCount(0);
                Spell.Level = 99;
            }

            foreach (var Spell in this.SpellCollection.GetSpells())
            {
                Spell.SetMaterialCount(0);
                Spell.Level = 99;
            }
        }

        /// <summary>
        /// Increases the number of stars for star chest.
        /// </summary>
        public void IncreaseStarsToStarChest(int Count)
        {
            if (this.StarChest != null)
            {
                if (!this.StarChestCooldown)
                {
                    this.StarChestCounter = Math.Min(this.StarChestCounter + Count, Globals.CrownChestCrownCount);
                }
            }
        }

        /// <summary>
        /// Called after loading of state for refresh this instance.
        /// </summary>
        public void LoadingFinished()
        {
            this.SaveCurrentDeckTo(this.SelectedDeck);

            if (Globals.RefreshArenaInLoadingFinished)
            {
                this.GameMode.Player.RefreshArena();
            }

            this.UpdateChestCountToPlayer();
            this.UpdateCardCountToPlayer();

            if (this.GameMode.Player.IsInAlliance)
            {
                this.GameMode.AchievementManager.UpdateAchievementProgress(0, 1);
            }
        }

        /// <summary>
        /// Sets if the player claim a reward.
        /// </summary>
        public void SetClaimingReward(bool Value)
        {
            this.ClaimingReward = Value;
        }

        /// <summary>
        /// Sets the last shown level up.
        /// </summary>
        public void SetLastShownLevelUp(int ExpLevel)
        {
            this.LastLevelUpPopup = ExpLevel;
        }

        /// <summary>
        /// Sets the opened page.
        /// </summary>
        public void SetPageOpened(int Page)
        {
            this.PageOpened |= 1 << Page;
        }

        /// <summary>
        /// Sets the shop weekday seen.
        /// </summary>
        public void SetShopWeekdayIndexSeen(int Day)
        {
            this.ShopDaySeen = Day;
        }

        /// <summary>
        /// Sets the tutorial to finish step.
        /// </summary>
        public void SetTutorialFinished(TutorialData Data)
        {
            // TODO : Implement LogicClientHome::setTutorialFinished().
        }

        /// <summary>
        /// Starts the donation cooldown.
        /// </summary>
        public void StartDonationCooldown(int Time)
        {
            this.RequestCooldownTimer.StartTimer(Time);
        }

        /// <summary>
        /// Starts the start chest timer.
        /// </summary>
        public void StartFreeChestTimer(int SkipSeconds)
        {
            this.FreeChestTimer.StartTimer(3600 * Globals.FreeChestIntervalHours - this.FreeChestTimer.RemainingSeconds - SkipSeconds);
        }

        /// <summary>
        /// Starts the start chest timer.
        /// </summary>
        public void StartStarChestTimer(int SkipSeconds)
        {
            if (!this.StarChestTimer.IsFinished)
            {
                Logging.Error(this.GetType(), "StartStarChestTimer() - Timer still runing");
            }

            this.StarChestTimer.StartTimer(3600 * Globals.CrownChestCooldownHours - this.StarChestTimer.RemainingSeconds - SkipSeconds);
        }

        /// <summary>
        /// Updates the number of chest to player.
        /// </summary>
        public void UpdateArenaFromPlayer()
        {
            Player Player = this.GameMode.Player;

            if (Player != null)
            {
                this.LastArena = Player.Arena;
            }
        }

        /// <summary>
        /// Updates the number of card to player.
        /// </summary>
        public void UpdateCardCountToPlayer()
        {
            Player Player = this.GameMode.Player;

            if (Player != null)
            {
                Player.SetCardsFound(this.SpellCount);
            }
        }

        /// <summary>
        /// Updates the number of chest to player.
        /// </summary>
        public void UpdateChestCountToPlayer()
        {
            Player Player = this.GameMode.Player;

            if (Player != null)
            {
                Player.SetChestCount(this.Chests.Count);
            }
        }

        /// <summary>
        /// Updates the free chest diamond index.
        /// </summary>
        public void UpdateFreeChestDiamondIndex()
        {
            this.FreeChestIdx += 1;
            this.FreeChestIdx %= Globals.FreeChestDiamondLoop.Length;
        }

        /// <summary>
        /// Updates the crown chest diamond index.
        /// </summary>
        public void UpdateCrownChestDiamondIndex()
        {
            this.CrownChestIdx += 1;
            this.CrownChestIdx %= Globals.FreeChestDiamondLoop.Length;
        }

        /// <summary>
        /// Updates the stars from avatar.
        /// </summary>
        public void UpdateStarsFromAvatar()
        {
            if (this.GameMode != null)
            {
                Player Player = this.GameMode.Player;

                if (Player != null)
                {
                    int StarCount = Player.StarCount;

                    if (StarCount > 0)
                    {
                        Player.SetStarCount(0);
                        this.IncreaseStarsToStarChest(StarCount);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the current deco to the specified preset.
        /// </summary>
        public void SaveCurrentDeckTo(int Index)
        {
            if (Index >= 5)
            {
                Logging.Error(this.GetType(), "SaveCurrentDeckTo() - deckIdx out of range.");
                return;
            }

            for (int I = 0; I < 8; I++)
            {
                if (this.SpellDeck[I] != null)
                {
                    this.SavedDecks[Index][I] = this.SpellDeck[I].Data.GlobalId;
                }
                else
                {
                    this.SavedDecks[Index][I] = 0;
                }
            }
        }

        /// <summary>
        /// Sets the selected deck.
        /// </summary>
        public void SetSelectedDeck(int Index)
        {
            if (Globals.MultipleDecks)
            {
                if (Index >= 5)
                {
                    Logging.Error(this.GetType(), "SetSelectedDeck() - deckIdx out of range.");
                    return;
                }

                if (this.SelectedDeck != Index)
                {
                    if (Array.TrueForAll(this.SavedDecks[Index], Id => Id == 0))
                    {
                        this.SaveCurrentDeckTo(Index);
                        return;
                    }

                    for (int I = 0; I < 8; I++)
                    {
                        SpellData Data = CsvFiles.Spells.Find(T => T.GlobalId == this.SavedDecks[Index][I]);

                        if (Data != null)
                        {
                            int SpellIdx = this.SpellDeck.GetSpellIdxByData(Data);

                            if (SpellIdx == -1)
                            {
                                SpellIdx = this.SpellCollection.GetSpellIdxByData(Data);

                                if (SpellIdx == -1)
                                {
                                    if (this.SpellCollection.Count < 1)
                                    {
                                        Spell Spell = this.SpellDeck[I];

                                        if (Spell != null)
                                        {
                                            this.SavedDecks[Index][I] = Spell.Data.GlobalId;
                                        }
                                        else
                                        {
                                            this.SavedDecks[Index][I] = -1;
                                        }

                                        continue;
                                    }

                                    this.SavedDecks[Index][I] = this.SpellCollection[0].Data.GlobalId;
                                }

                                Spell InDeck = this.SpellDeck[I];
                                Spell InCollection = this.SpellCollection[SpellIdx];

                                if (InDeck != null)
                                {
                                    this.SpellCollection.SetSpell(SpellIdx, InDeck);
                                }
                                else
                                {
                                    this.SpellCollection.RemoveSpell(SpellIdx);
                                }

                                this.SpellDeck.SetSpell(I, InCollection);
                            }
                            else if (I != SpellIdx)
                            {
                                this.SpellDeck.SwapSpells(SpellIdx, I);
                            }
                        }
                    }
                }

                this.SelectedDeck = Index;
            }
        }

        /// <summary>
        /// Creates a fast forward.
        /// </summary>
        public void FastForward(int Seconds)
        {
            for (int I = 0; I < this.Chests.Count; I++)
            {
                if (this.Chests[I] != null)
                {
                    this.Chests[I].FastForward(Seconds);
                }
            }

            if (this.FreeChestCount == 0)
            {
                if (this.FreeChest == null)
                {
                    this.CreateFreeChest();
                }
            }

            if (this.FreeChestTimer.RemainingSeconds <= Seconds)
            {
                int Remaining = this.StarChestTimer.RemainingSeconds;

                this.FreeChestTimer.Reset();

                if (this.FreeChest == null)
                {
                    if (this.FreeChestTimer.IsFinished)
                    {
                        this.CreateFreeChest();
                        this.StartFreeChestTimer(Seconds - Remaining);
                    }
                }
            }
            else
            {
                this.FreeChestTimer.FastForward(Seconds);
            }

            this.DonationCapacityCooldownTimer.FastForward(Seconds);
            this.RequestCooldownTimer.FastForward(Seconds);
            this.ElderKickTimer.FastForward(Seconds);
            this.SendMailTimer.FastForward(Seconds);
            this.ShareTimer.FastForward(Seconds);
            this.ShopTimer.FastForward(Seconds);

            if (this.StarChestTimer.RemainingSeconds <= Seconds)
            {
                int Remaining = this.StarChestTimer.RemainingSeconds;

                this.StarChestTimer.Reset();

                if (this.StarChestCooldown)
                {
                    this.StartStarChestTimer(Seconds - Remaining);
                }
            }
            else
            {
                this.StarChestTimer.FastForward(Seconds);
            }
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            this.LastTick = DateTime.UtcNow;

            this.FreeChestTimer.Tick();

            if (this.FreeChestCount == 0)
            {
                if (this.FreeChest == null)
                {
                    this.CreateFreeChest();
                }

                this.StartFreeChestTimer(0);
            }

            if (this.FreeChestTimer.IsFinished)
            {
                if (this.FreeChest == null)
                {
                    this.CreateFreeChest();
                    this.StartFreeChestTimer(0);
                }
            }

            if (this.StarChestTimer.IsFinished)
            {
                if (this.StarChestCooldown)
                {
                    this.StartFreeChestTimer(0);
                }

                this.StarChestCooldown = false;
            }
            else
            {
                this.StarChestTimer.Tick();
            }

            if (this.PurchasedChest != null)
            {
                if (!this.ClaimingReward)
                {
                    this.PurchasedChest.SetClaimed(this.GameMode, 4);
                }
            }

            this.DonationCapacityCooldownTimer.Tick();
            this.RequestCooldownTimer.Tick();
            this.ElderKickTimer.Tick();
            this.SendMailTimer.Tick();
            this.ShareTimer.Tick();
            this.ShopTimer.Tick();

            if (this.StarChestCounter >= Globals.CrownChestCrownCount)
            {
                this.StarChest = null;
                this.StarChest = new Chest(this.GameMode.Player.Arena.StarChestData);
            }

            this.UpdateStarsFromAvatar();
            this.UpdateArenaFromPlayer();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream Stream)
        {
            Stream.WriteLong(this.HomeId);

            Stream.WriteVInt(this.RunningChestId);
            Stream.WriteVInt(this.FreeChestCount);

            this.FreeChestTimer.Encode(Stream);

            Stream.WriteVInt(this.DonationCapacityLimit);

            Stream.WriteVInt(this.SavedDecks.Length);

            for (int I = 0; I < this.SavedDecks.Length; I++)
            {
                Stream.WriteVInt(this.SavedDecks[I].Length);

                for (int J = 0; J < this.SavedDecks[I].Length; J++)
                {
                    Stream.WriteVInt(this.SavedDecks[I][J]);
                }
            }

            this.SpellDeck.Encode(Stream);
            this.SpellCollection.Encode(Stream);

            Stream.WriteVInt(this.SelectedDeck);

            this.SpellDeck.Encode(Stream); // Last Deck ?

            Stream.AddRange("94-11-34-7F-93-9B-FE-A2-0B-01-00-0C-83-09-00-00-00-19-44-65-6D-6F-20-41-63-63-6F-75-6E-74-20-32-76-32-20-46-72-69-65-6E-64-6C-79-05-A8-C0-D3-94-0B-A8-86-C7-A4-0B-A8-C0-D3-94-0B-00-00-00-00-00-00-00-00-00-00-00-19-44-65-6D-6F-20-41-63-63-6F-75-6E-74-20-32-76-32-20-46-72-69-65-6E-64-6C-79-00-00-00-4D-7B-22-54-61-72-67-65-74-5F-41-63-63-6F-75-6E-74-54-79-70-65-22-3A-22-44-65-6D-6F-41-63-63-6F-75-6E-74-22-2C-22-48-69-64-65-54-69-6D-65-72-22-3A-74-72-75-65-2C-22-47-61-6D-65-4D-6F-64-65-22-3A-22-54-65-61-6D-56-73-54-65-61-6D-22-7D-00-84-09-00-00-00-1F-44-65-6D-6F-20-41-63-63-6F-75-6E-74-20-32-76-32-20-44-72-61-66-74-20-46-72-69-65-6E-64-6C-79-05-A8-C0-D3-94-0B-A8-86-C7-A4-0B-A8-C0-D3-94-0B-00-00-00-00-00-00-00-00-00-00-00-1F-44-65-6D-6F-20-41-63-63-6F-75-6E-74-20-32-76-32-20-44-72-61-66-74-20-46-72-69-65-6E-64-6C-79-00-00-00-5B-7B-22-47-61-6D-65-4D-6F-64-65-22-3A-22-54-65-61-6D-56-73-54-65-61-6D-44-72-61-66-74-43-68-61-6C-6C-65-6E-67-65-22-2C-22-48-69-64-65-54-69-6D-65-72-22-3A-74-72-75-65-2C-22-54-61-72-67-65-74-5F-41-63-63-6F-75-6E-74-54-79-70-65-22-3A-22-44-65-6D-6F-41-63-63-6F-75-6E-74-22-7D-00-85-09-00-00-00-1F-44-65-6D-6F-20-41-63-63-6F-75-6E-74-20-31-76-31-20-44-72-61-66-74-20-46-72-69-65-6E-64-6C-79-05-A8-C0-D3-94-0B-A8-86-C7-A4-0B-A8-C0-D3-94-0B-00-00-00-00-00-00-00-00-00-00-00-1F-44-65-6D-6F-20-41-63-63-6F-75-6E-74-20-31-76-31-20-44-72-61-66-74-20-46-72-69-65-6E-64-6C-79-00-00-00-4C-7B-22-48-69-64-65-54-69-6D-65-72-22-3A-74-72-75-65-2C-22-54-61-72-67-65-74-5F-41-63-63-6F-75-6E-74-54-79-70-65-22-3A-22-44-65-6D-6F-41-63-63-6F-75-6E-74-22-2C-22-47-61-6D-65-4D-6F-64-65-22-3A-22-44-72-61-66-74-4D-6F-64-65-22-7D-00-95-11-00-00-00-0A-32-76-32-20-42-75-74-74-6F-6E-08-B0-93-D4-99-0B-B0-E1-DD-B7-0B-B0-A9-8A-99-0B-00-00-00-00-00-00-00-00-00-00-00-0A-32-76-32-20-42-75-74-74-6F-6E-00-00-00-75-7B-22-48-69-64-65-54-69-6D-65-72-22-3A-74-72-75-65-2C-22-48-69-64-65-50-6F-70-75-70-54-69-6D-65-72-22-3A-74-72-75-65-2C-22-45-78-74-72-61-47-61-6D-65-4D-6F-64-65-43-68-61-6E-63-65-22-3A-30-2C-22-47-61-6D-65-4D-6F-64-65-22-3A-22-54-65-61-6D-56-73-54-65-61-6D-4C-61-64-64-65-72-22-2C-22-45-78-74-72-61-47-61-6D-65-4D-6F-64-65-22-3A-22-4E-6F-6E-65-22-7D-00-A3-13-00-00-00-1C-44-6F-75-62-6C-65-20-45-6C-69-78-69-72-20-46-72-69-65-6E-64-6C-79-20-4D-61-74-63-68-05-80-B0-88-A2-0B-80-AA-CF-A4-0B-80-B0-88-A2-0B-00-00-00-00-00-00-00-00-00-00-00-1C-44-6F-75-62-6C-65-20-45-6C-69-78-69-72-20-46-72-69-65-6E-64-6C-79-20-4D-61-74-63-68-00-00-01-17-7B-22-54-69-74-6C-65-22-3A-22-44-6F-75-62-6C-65-20-C3-A9-6C-69-78-69-72-20-61-6D-69-63-61-6C-22-2C-22-53-75-62-74-69-74-6C-65-22-3A-22-4A-6F-75-65-7A-20-61-76-65-63-20-76-6F-74-72-65-20-63-6C-61-6E-C2-A0-21-22-2C-22-47-61-6D-65-4D-6F-64-65-22-3A-22-44-6F-75-62-6C-65-45-6C-69-78-69-72-5F-46-72-69-65-6E-64-6C-79-22-2C-22-48-69-64-65-54-69-6D-65-72-22-3A-66-61-6C-73-65-2C-22-42-61-63-6B-67-72-6F-75-6E-64-22-3A-7B-22-50-61-74-68-22-3A-22-2F-61-63-61-31-37-64-61-35-2D-34-32-38-35-2D-34-63-65-30-2D-39-35-31-30-2D-38-62-30-63-65-37-36-39-62-64-62-36-5F-66-72-69-65-6E-64-5F-32-65-6C-69-78-69-72-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-64-64-35-31-64-37-34-63-34-32-66-33-32-32-36-38-61-66-34-34-37-30-38-65-65-64-32-33-34-32-32-32-22-2C-22-46-69-6C-65-22-3A-22-66-72-69-65-6E-64-5F-32-65-6C-69-78-69-72-2E-70-6E-67-22-7D-7D-00-A4-13-00-00-00-16-53-75-64-64-65-6E-20-44-65-61-74-68-20-43-68-61-6C-6C-65-6E-67-65-02-80-D4-C7-A2-0B-80-EC-F1-A2-0B-80-8E-BD-A2-0B-00-00-00-00-00-00-00-00-00-00-00-16-53-75-64-64-65-6E-20-44-65-61-74-68-20-43-68-61-6C-6C-65-6E-67-65-00-00-06-8E-7B-22-47-61-6D-65-4D-6F-64-65-22-3A-22-4F-76-65-72-74-69-6D-65-5F-54-6F-75-72-6E-61-6D-65-6E-74-22-2C-22-54-69-74-6C-65-22-3A-22-44-C3-A9-66-69-20-64-65-20-6C-61-20-6D-6F-72-74-20-73-75-62-69-74-65-22-2C-22-46-72-65-65-50-61-73-73-22-3A-31-2C-22-4A-6F-69-6E-43-6F-73-74-22-3A-31-30-30-2C-22-4D-61-78-4C-6F-73-73-65-73-22-3A-33-2C-22-52-65-77-61-72-64-73-22-3A-5B-7B-22-47-6F-6C-64-22-3A-37-30-30-2C-22-43-61-72-64-73-22-3A-31-30-7D-2C-7B-22-47-6F-6C-64-22-3A-39-35-30-2C-22-43-61-72-64-73-22-3A-31-35-7D-2C-7B-22-47-6F-6C-64-22-3A-31-32-35-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-31-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-32-35-7D-2C-7B-22-47-6F-6C-64-22-3A-31-36-30-30-2C-22-43-61-72-64-73-22-3A-34-33-7D-2C-7B-22-47-6F-6C-64-22-3A-32-30-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-32-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-36-35-7D-2C-7B-22-47-6F-6C-64-22-3A-32-35-30-30-2C-22-43-61-72-64-73-22-3A-39-33-7D-2C-7B-22-47-6F-6C-64-22-3A-33-31-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-34-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-31-32-35-7D-2C-7B-22-47-6F-6C-64-22-3A-33-38-30-30-2C-22-43-61-72-64-73-22-3A-31-36-35-7D-2C-7B-22-47-6F-6C-64-22-3A-34-36-35-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-38-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-32-31-30-7D-2C-7B-22-47-6F-6C-64-22-3A-35-37-35-30-2C-22-43-61-72-64-73-22-3A-32-36-35-7D-2C-7B-22-47-6F-6C-64-22-3A-37-31-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-31-36-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-33-33-35-7D-2C-7B-22-47-6F-6C-64-22-3A-38-37-35-30-2C-22-43-61-72-64-73-22-3A-34-33-30-7D-2C-7B-22-47-6F-6C-64-22-3A-31-31-30-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-49-73-48-69-67-68-6C-69-67-68-74-65-64-22-3A-74-72-75-65-2C-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-33-32-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-35-35-30-7D-5D-2C-22-49-63-6F-6E-45-78-70-6F-72-74-4E-61-6D-65-22-3A-22-69-63-6F-6E-5F-74-6F-75-72-6E-61-6D-65-6E-74-5F-73-75-64-64-65-6E-64-65-61-74-68-5F-67-72-61-6E-64-22-2C-22-57-69-6E-49-63-6F-6E-45-78-70-6F-72-74-4E-61-6D-65-22-3A-22-74-6F-75-72-6E-61-6D-65-6E-74-5F-6F-70-65-6E-5F-77-69-6E-73-5F-62-61-64-67-65-5F-67-6F-6C-64-22-2C-22-41-72-65-6E-61-22-3A-22-41-6C-6C-22-2C-22-53-75-62-74-69-74-6C-65-22-3A-22-4C-65-20-6D-6F-64-65-20-6D-6F-72-74-20-73-75-62-69-74-65-20-64-C3-A8-73-20-6C-61-20-70-72-65-6D-69-C3-A8-72-65-20-73-65-63-6F-6E-64-65-C2-A0-21-22-2C-22-45-6E-64-4E-6F-74-69-66-69-63-61-74-69-6F-6E-22-3A-22-4C-65-20-64-C3-A9-66-69-20-64-65-20-6C-61-20-6D-6F-72-74-20-73-75-62-69-74-65-20-73-65-20-74-65-72-6D-69-6E-65-20-64-61-6E-73-20-32-68-C2-A0-21-22-2C-22-53-74-61-72-74-4E-6F-74-69-66-69-63-61-74-69-6F-6E-22-3A-22-4C-65-20-64-C3-A9-66-69-20-64-65-20-6C-61-20-6D-6F-72-74-20-73-75-62-69-74-65-20-61-20-63-6F-6D-6D-65-6E-63-C3-A9-C2-A0-21-20-4A-6F-75-65-7A-20-70-6F-75-72-20-67-61-67-6E-65-72-20-64-65-73-20-72-C3-A9-63-6F-6D-70-65-6E-73-65-73-C2-A0-21-22-2C-22-55-6E-6C-6F-63-6B-65-64-46-6F-72-58-50-22-3A-22-45-78-70-65-72-69-65-6E-63-65-64-22-2C-22-44-65-73-63-72-69-70-74-69-6F-6E-22-3A-22-4C-65-20-70-72-65-6D-69-65-72-20-C3-A0-20-64-C3-A9-74-72-75-69-72-65-20-75-6E-65-20-74-6F-75-72-20-61-20-67-61-67-6E-C3-A9-C2-A0-21-20-4F-62-74-65-6E-65-7A-20-64-65-73-20-72-C3-A9-63-6F-6D-70-65-6E-73-65-73-20-75-6E-69-71-75-65-73-20-65-6E-20-70-72-6F-67-72-65-73-73-61-6E-74-20-65-74-20-72-65-6D-70-6F-72-74-65-7A-20-6C-65-20-64-C3-A9-66-69-20-61-76-65-63-20-31-32-C2-A0-76-69-63-74-6F-69-72-65-73-2E-20-33-C2-A0-64-C3-A9-66-61-69-74-65-73-20-65-74-20-63-27-65-73-74-20-66-69-6E-69-C2-A0-21-22-2C-22-4A-6F-69-6E-43-6F-73-74-52-65-73-6F-75-72-63-65-22-3A-22-47-65-6D-73-22-2C-22-42-61-63-6B-67-72-6F-75-6E-64-22-3A-7B-22-50-61-74-68-22-3A-22-2F-66-61-63-31-30-65-34-38-2D-65-63-34-39-2D-34-33-32-63-2D-38-38-63-39-2D-37-65-39-32-34-65-63-39-36-34-35-37-5F-73-75-64-64-65-6E-64-65-61-74-68-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-65-66-33-66-32-34-33-66-31-30-66-36-30-30-33-39-34-33-35-31-64-62-39-31-37-62-35-35-34-33-31-37-22-2C-22-46-69-6C-65-22-3A-22-73-75-64-64-65-6E-64-65-61-74-68-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-7D-2C-22-42-61-63-6B-67-72-6F-75-6E-64-5F-43-6F-6D-70-6C-65-74-65-22-3A-7B-22-50-61-74-68-22-3A-22-2F-34-34-31-62-62-38-31-32-2D-31-66-33-63-2D-34-38-63-64-2D-61-61-64-37-2D-38-35-39-61-35-30-36-65-66-38-30-64-5F-73-75-64-64-65-6E-64-65-61-74-68-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-65-66-33-66-32-34-33-66-31-30-66-36-30-30-33-39-34-33-35-31-64-62-39-31-37-62-35-35-34-33-31-37-22-2C-22-46-69-6C-65-22-3A-22-73-75-64-64-65-6E-64-65-61-74-68-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-7D-7D-00-A5-13-00-00-00-16-53-75-64-64-65-6E-20-44-65-61-74-68-20-46-72-69-65-6E-64-6C-79-20-05-80-D4-C7-A2-0B-80-EC-F1-A2-0B-80-D4-C7-A2-0B-00-00-00-00-00-00-00-00-00-00-00-16-53-75-64-64-65-6E-20-44-65-61-74-68-20-46-72-69-65-6E-64-6C-79-20-00-00-01-0D-7B-22-54-69-74-6C-65-22-3A-22-4D-6F-72-74-20-73-75-62-69-74-65-20-61-6D-69-63-61-6C-65-22-2C-22-47-61-6D-65-4D-6F-64-65-22-3A-22-4F-76-65-72-74-69-6D-65-5F-46-72-69-65-6E-64-6C-79-22-2C-22-53-75-62-74-69-74-6C-65-22-3A-22-4A-6F-75-65-7A-20-61-76-65-63-20-76-6F-74-72-65-20-63-6C-61-6E-C2-A0-21-22-2C-22-42-61-63-6B-67-72-6F-75-6E-64-22-3A-7B-22-50-61-74-68-22-3A-22-2F-38-33-37-34-62-39-30-62-2D-33-39-30-30-2D-34-65-66-32-2D-62-32-38-63-2D-66-37-63-66-33-31-65-64-61-61-34-30-5F-66-72-69-65-6E-64-5F-73-75-64-64-65-6E-64-65-61-74-68-5F-30-31-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-61-34-61-62-36-36-30-36-63-64-31-39-63-32-66-35-39-31-64-35-61-30-37-62-37-66-36-38-61-66-38-61-22-2C-22-46-69-6C-65-22-3A-22-66-72-69-65-6E-64-5F-73-75-64-64-65-6E-64-65-61-74-68-5F-30-31-2E-70-6E-67-22-7D-7D-00-A6-13-00-00-00-12-53-75-64-64-65-6E-20-44-65-61-74-68-20-51-75-65-73-74-07-80-D4-C7-A2-0B-80-EC-F1-A2-0B-80-D4-C7-A2-0B-00-00-00-00-00-00-00-00-00-00-00-12-53-75-64-64-65-6E-20-44-65-61-74-68-20-51-75-65-73-74-00-00-01-09-7B-22-54-69-74-6C-65-22-3A-22-51-75-C3-AA-74-65-20-64-75-20-64-C3-A9-66-69-20-64-65-20-6C-61-20-6D-6F-72-74-20-73-75-62-69-74-65-22-2C-22-49-6E-66-6F-22-3A-22-47-61-67-6E-65-7A-20-36-C2-A0-63-6F-75-72-6F-6E-6E-65-73-20-64-61-6E-73-20-6C-65-20-64-C3-A9-66-69-20-64-65-20-6C-61-20-6D-6F-72-74-20-73-75-62-69-74-65-22-2C-22-43-6F-75-6E-74-22-3A-36-2C-22-50-6F-69-6E-74-73-22-3A-32-30-2C-22-43-68-72-6F-6E-6F-73-51-75-65-73-74-52-65-77-61-72-64-73-22-3A-5B-7B-22-54-79-70-65-22-3A-22-47-65-6D-73-22-2C-22-43-6F-75-6E-74-22-3A-32-30-7D-5D-2C-22-51-75-65-73-74-54-79-70-65-22-3A-22-57-69-6E-22-2C-22-41-74-74-61-63-6B-22-3A-7B-22-54-79-70-65-22-3A-22-44-65-73-74-72-6F-79-22-7D-2C-22-57-69-6E-22-3A-7B-22-54-79-70-65-22-3A-22-43-72-6F-77-6E-73-22-2C-22-45-76-65-6E-74-73-22-3A-5B-31-32-35-32-5D-7D-7D-00-A7-13-00-00-00-0D-44-65-63-20-31-31-20-50-4F-50-20-55-50-0F-80-EC-F1-A2-0B-80-D6-BB-A3-0B-80-EC-F1-A2-0B-00-00-00-00-00-00-00-00-00-00-00-0D-44-65-63-20-31-31-20-50-4F-50-20-55-50-00-00-01-EC-7B-22-53-68-6F-77-54-69-6D-65-72-22-3A-66-61-6C-73-65-2C-22-53-75-62-74-69-74-6C-65-22-3A-22-55-6E-65-20-6D-69-73-65-20-C3-A0-20-6A-6F-75-72-20-C3-A9-6C-65-63-74-72-69-73-61-6E-74-65-22-2C-22-4D-61-69-6E-54-65-78-74-22-3A-22-54-6F-75-74-20-6E-6F-75-76-65-61-75-C2-A0-3A-20-75-6E-65-20-61-72-C3-A8-6E-65-2C-20-64-65-75-78-20-63-61-72-74-65-73-20-65-74-20-74-72-6F-69-73-20-63-6F-66-66-72-65-73-C2-A0-21-20-41-63-63-C3-A9-64-65-7A-20-C3-A0-20-6C-61-20-76-61-6C-6C-C3-A9-65-20-C3-A9-6C-65-63-74-72-69-71-75-65-20-61-76-65-63-20-33-34-30-30-C2-A0-74-72-6F-70-68-C3-A9-65-73-2E-20-44-C3-A9-62-6C-6F-71-75-65-7A-20-6C-65-73-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-65-74-20-6C-65-20-63-68-61-73-73-65-75-72-20-6D-61-69-6E-74-65-6E-61-6E-74-C2-A0-21-22-2C-22-42-61-63-6B-67-72-6F-75-6E-64-22-3A-7B-22-50-61-74-68-22-3A-22-2F-35-36-30-33-30-37-37-65-2D-61-64-36-64-2D-34-37-36-34-2D-38-61-65-31-2D-38-38-36-64-39-33-33-35-31-35-32-36-5F-64-65-63-65-6D-62-65-72-5F-70-6F-70-75-70-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-32-61-62-32-61-66-37-31-33-35-39-37-39-37-39-62-35-30-61-32-34-31-63-39-61-64-35-38-32-34-37-31-22-2C-22-46-69-6C-65-22-3A-22-64-65-63-65-6D-62-65-72-5F-70-6F-70-75-70-2E-70-6E-67-22-7D-2C-22-54-69-74-6C-65-22-3A-22-4E-6F-75-76-65-6C-6C-65-20-6D-69-73-65-20-C3-A0-20-6A-6F-75-72-22-2C-22-42-75-74-74-6F-6E-54-65-78-74-22-3A-22-47-C3-A9-6E-69-61-6C-C2-A0-21-22-2C-22-53-68-6F-77-4F-6E-45-61-63-68-53-74-61-72-74-75-70-22-3A-66-61-6C-73-65-7D-00-A8-13-00-00-00-21-5A-61-70-70-69-65-73-20-76-73-20-48-75-6E-74-65-72-20-44-72-61-66-74-20-43-68-61-6C-6C-65-6E-67-65-02-80-EC-F1-A2-0B-80-84-9C-A3-0B-80-EC-F1-A2-0B-00-00-00-00-00-00-00-00-00-00-00-21-5A-61-70-70-69-65-73-20-76-73-20-48-75-6E-74-65-72-20-44-72-61-66-74-20-43-68-61-6C-6C-65-6E-67-65-00-00-07-5A-7B-22-43-61-73-75-61-6C-22-3A-66-61-6C-73-65-2C-22-47-61-6D-65-4D-6F-64-65-22-3A-22-44-72-61-66-74-4D-6F-64-65-22-2C-22-46-72-65-65-50-61-73-73-22-3A-31-2C-22-4A-6F-69-6E-43-6F-73-74-22-3A-31-30-30-2C-22-4A-6F-69-6E-43-6F-73-74-52-65-73-6F-75-72-63-65-22-3A-22-47-65-6D-73-22-2C-22-4D-61-78-4C-6F-73-73-65-73-22-3A-33-2C-22-52-65-77-61-72-64-73-22-3A-5B-7B-22-47-6F-6C-64-22-3A-37-30-30-2C-22-43-61-72-64-73-22-3A-31-30-7D-2C-7B-22-47-6F-6C-64-22-3A-39-35-30-2C-22-43-61-72-64-73-22-3A-31-35-7D-2C-7B-22-47-6F-6C-64-22-3A-31-32-35-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-31-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-32-35-7D-2C-7B-22-47-6F-6C-64-22-3A-31-36-30-30-2C-22-43-61-72-64-73-22-3A-34-33-7D-2C-7B-22-47-6F-6C-64-22-3A-32-30-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-33-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-36-35-7D-2C-7B-22-47-6F-6C-64-22-3A-32-35-30-30-2C-22-43-61-72-64-73-22-3A-39-33-7D-2C-7B-22-47-6F-6C-64-22-3A-33-31-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-35-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-31-32-35-7D-2C-7B-22-47-6F-6C-64-22-3A-33-38-30-30-2C-22-43-61-72-64-73-22-3A-31-36-35-7D-2C-7B-22-47-6F-6C-64-22-3A-34-36-35-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-53-70-65-6C-6C-22-2C-22-41-6D-6F-75-6E-74-22-3A-31-30-2C-22-53-70-65-6C-6C-22-3A-22-4D-69-6E-69-53-70-61-72-6B-79-73-22-7D-2C-22-43-61-72-64-73-22-3A-32-31-30-7D-2C-7B-22-47-6F-6C-64-22-3A-35-37-35-30-2C-22-43-61-72-64-73-22-3A-32-36-35-7D-2C-7B-22-47-6F-6C-64-22-3A-37-31-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-41-6D-6F-75-6E-74-22-3A-31-30-30-30-30-7D-2C-22-43-61-72-64-73-22-3A-33-33-35-7D-2C-7B-22-47-6F-6C-64-22-3A-38-37-35-30-2C-22-43-61-72-64-73-22-3A-34-33-30-7D-2C-7B-22-47-6F-6C-64-22-3A-31-31-30-30-30-2C-22-4D-69-6C-65-73-74-6F-6E-65-22-3A-7B-22-49-73-48-69-67-68-6C-69-67-68-74-65-64-22-3A-74-72-75-65-2C-22-54-79-70-65-22-3A-22-53-70-65-6C-6C-22-2C-22-41-6D-6F-75-6E-74-22-3A-35-2C-22-53-70-65-6C-6C-22-3A-22-48-75-6E-74-65-72-22-7D-2C-22-43-61-72-64-73-22-3A-35-35-30-7D-5D-2C-22-57-69-6E-49-63-6F-6E-45-78-70-6F-72-74-4E-61-6D-65-22-3A-22-74-6F-75-72-6E-61-6D-65-6E-74-5F-6F-70-65-6E-5F-77-69-6E-73-5F-62-61-64-67-65-5F-64-72-61-66-74-22-2C-22-53-75-62-74-69-74-6C-65-22-3A-22-4A-6F-75-65-7A-20-65-74-20-64-C3-A9-62-6C-6F-71-75-65-7A-20-64-65-75-78-20-6E-6F-75-76-65-6C-6C-65-73-20-63-61-72-74-65-73-22-2C-22-53-75-62-74-69-74-6C-65-5F-53-68-6F-72-74-22-3A-22-44-C3-A9-62-6C-6F-71-75-65-7A-20-64-65-75-78-20-6E-6F-75-76-65-6C-6C-65-73-20-63-61-72-74-65-73-22-2C-22-44-65-73-63-72-69-70-74-69-6F-6E-22-3A-22-50-72-C3-A9-70-61-72-65-7A-20-76-6F-74-72-65-20-64-65-63-6B-20-65-74-20-6A-6F-75-65-7A-20-61-76-65-63-20-64-65-75-78-20-6E-6F-75-76-65-6C-6C-65-73-20-63-61-72-74-65-73-C2-A0-21-20-50-61-72-74-69-63-69-70-65-7A-20-61-75-20-64-C3-A9-66-69-20-70-6F-75-72-20-64-C3-A9-62-6C-6F-71-75-65-72-20-63-65-73-20-64-65-75-78-20-6E-6F-75-76-65-6C-6C-65-73-20-63-61-72-74-65-73-2E-20-33-20-70-65-72-74-65-73-20-65-74-20-74-6F-75-74-20-73-27-61-72-72-C3-AA-74-65-2E-22-2C-22-55-6E-6C-6F-63-6B-65-64-46-6F-72-58-50-22-3A-22-45-78-70-65-72-69-65-6E-63-65-64-22-2C-22-44-72-61-66-74-44-65-63-6B-22-3A-22-44-72-61-66-74-5F-76-31-5F-64-65-63-5F-63-61-72-64-5F-72-65-6C-65-61-73-65-22-2C-22-54-69-74-6C-65-22-3A-22-44-C3-A9-66-69-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-22-2C-22-49-63-6F-6E-45-78-70-6F-72-74-4E-61-6D-65-22-3A-22-69-63-6F-6E-5F-74-6F-75-72-6E-61-6D-65-6E-74-5F-64-72-61-66-74-5F-67-72-61-6E-64-22-2C-22-41-72-65-6E-61-22-3A-22-41-72-65-6E-61-5F-45-6C-65-63-74-72-69-63-22-2C-22-45-6E-64-4E-6F-74-69-66-69-63-61-74-69-6F-6E-22-3A-22-4C-65-20-64-C3-A9-66-69-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-20-65-73-74-20-70-72-65-73-71-75-65-20-66-69-6E-69-C2-A0-21-20-44-C3-A9-62-6C-6F-71-75-65-7A-20-64-65-75-78-20-6E-6F-75-76-65-6C-6C-65-73-20-63-61-72-74-65-73-2E-22-2C-22-53-74-61-72-74-4E-6F-74-69-66-69-63-61-74-69-6F-6E-22-3A-22-4C-65-20-64-C3-A9-66-69-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-20-64-C3-A9-62-75-74-65-C2-A0-21-20-4A-6F-75-65-7A-20-65-74-20-64-C3-A9-62-6C-6F-71-75-65-7A-20-64-65-75-78-20-6E-6F-75-76-65-6C-6C-65-73-20-63-61-72-74-65-73-2E-22-2C-22-42-61-63-6B-67-72-6F-75-6E-64-22-3A-7B-22-50-61-74-68-22-3A-22-2F-66-65-32-31-38-34-64-39-2D-65-39-62-31-2D-34-37-65-39-2D-38-30-39-64-2D-65-39-66-66-65-63-35-39-65-63-30-61-5F-7A-61-70-70-69-65-73-5F-68-75-6E-74-65-72-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-38-37-31-65-36-65-36-63-61-37-39-63-37-33-34-32-33-34-30-39-62-34-31-35-63-65-65-31-33-33-32-62-22-2C-22-46-69-6C-65-22-3A-22-7A-61-70-70-69-65-73-5F-68-75-6E-74-65-72-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-7D-2C-22-42-61-63-6B-67-72-6F-75-6E-64-5F-43-6F-6D-70-6C-65-74-65-22-3A-7B-22-50-61-74-68-22-3A-22-2F-65-64-31-32-64-34-62-30-2D-62-65-33-64-2D-34-66-36-32-2D-61-64-62-37-2D-33-36-64-38-64-65-62-39-33-36-38-37-5F-7A-61-70-70-69-65-73-5F-68-75-6E-74-65-72-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-38-37-31-65-36-65-36-63-61-37-39-63-37-33-34-32-33-34-30-39-62-34-31-35-63-65-65-31-33-33-32-62-22-2C-22-46-69-6C-65-22-3A-22-7A-61-70-70-69-65-73-5F-68-75-6E-74-65-72-5F-63-68-61-6C-6C-65-6E-67-65-5F-30-31-2E-70-6E-67-22-7D-7D-00-A9-13-00-00-00-17-5A-61-70-70-69-65-73-20-76-73-20-48-75-6E-74-65-72-20-51-75-65-73-74-07-80-EC-F1-A2-0B-80-84-9C-A3-0B-80-EC-F1-A2-0B-00-00-00-00-00-00-00-00-00-00-00-17-5A-61-70-70-69-65-73-20-76-73-20-48-75-6E-74-65-72-20-51-75-65-73-74-00-00-01-18-7B-22-54-69-74-6C-65-22-3A-22-51-75-C3-AA-74-65-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-22-2C-22-49-6E-66-6F-22-3A-22-47-61-67-6E-65-7A-20-36-C2-A0-63-6F-75-72-6F-6E-6E-65-73-20-64-61-6E-73-20-6C-65-20-64-C3-A9-66-69-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-22-2C-22-43-6F-75-6E-74-22-3A-36-2C-22-50-6F-69-6E-74-73-22-3A-32-30-2C-22-43-68-72-6F-6E-6F-73-51-75-65-73-74-52-65-77-61-72-64-73-22-3A-5B-7B-22-54-79-70-65-22-3A-22-47-6F-6C-64-22-2C-22-43-6F-75-6E-74-22-3A-31-30-30-30-7D-5D-2C-22-51-75-65-73-74-54-79-70-65-22-3A-22-57-69-6E-22-2C-22-57-69-6E-22-3A-7B-22-54-79-70-65-22-3A-22-43-72-6F-77-6E-73-22-2C-22-45-76-65-6E-74-73-22-3A-5B-31-32-35-36-5D-7D-2C-22-54-61-72-67-65-74-5F-4D-69-6E-58-50-4C-65-76-65-6C-22-3A-38-7D-00-AA-13-00-00-00-27-5A-61-70-70-69-65-73-20-76-73-20-48-75-6E-74-65-72-20-45-76-65-6E-74-20-49-63-6F-6E-20-61-6E-64-20-48-65-61-64-65-72-0D-80-EC-F1-A2-0B-80-84-9C-A3-0B-80-EC-F1-A2-0B-00-00-00-00-00-00-00-00-00-00-00-27-5A-61-70-70-69-65-73-20-76-73-20-48-75-6E-74-65-72-20-45-76-65-6E-74-20-49-63-6F-6E-20-61-6E-64-20-48-65-61-64-65-72-00-00-01-05-7B-22-49-6D-61-67-65-22-3A-7B-22-50-61-74-68-22-3A-22-2F-30-64-64-37-37-30-37-32-2D-65-30-63-39-2D-34-64-30-37-2D-62-38-33-38-2D-30-30-63-38-30-30-65-64-32-32-64-38-5F-7A-61-70-70-69-65-73-5F-68-75-6E-74-65-72-5F-68-65-61-64-65-72-2E-70-6E-67-22-2C-22-43-68-65-63-6B-73-75-6D-22-3A-22-39-61-30-34-36-66-64-31-33-34-66-37-66-33-33-37-35-39-64-35-61-36-30-31-66-39-39-35-36-30-39-39-22-2C-22-46-69-6C-65-22-3A-22-7A-61-70-70-69-65-73-5F-68-75-6E-74-65-72-5F-68-65-61-64-65-72-2E-70-6E-67-22-7D-2C-22-54-69-74-6C-65-47-6C-6F-77-22-3A-74-72-75-65-2C-22-49-63-6F-6E-22-3A-22-69-63-6F-6E-5F-74-6F-75-72-6E-61-6D-65-6E-74-5F-64-72-61-66-74-5F-67-72-61-6E-64-22-2C-22-54-69-74-6C-65-22-3A-22-C3-89-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-22-7D-00-00-00-00-00-09-00-80-80-B4-A1-0B-00-A7-13-00-00-00-0C-83-09-00-84-09-00-85-09-00-95-11-01-A3-13-01-A4-13-01-A5-13-01-A6-13-01-A7-13-01-A8-13-01-A9-13-01-AA-13-01-09-95-11-02-00-A3-13-02-00-A4-13-03-00-A5-13-03-00-A6-13-03-00-A7-13-02-00-A8-13-02-00-A9-13-02-00-AA-13-02-00-02-00-00-00-64-7B-22-49-44-22-3A-22-43-41-52-44-5F-52-45-4C-45-41-53-45-5F-56-32-22-2C-22-50-61-72-61-6D-73-22-3A-7B-22-43-61-72-64-73-22-3A-5B-7B-22-53-68-6F-77-41-73-53-6F-6F-6E-22-3A-66-61-6C-73-65-2C-22-53-70-65-6C-6C-22-3A-22-47-68-6F-73-74-22-2C-22-44-61-74-65-22-3A-22-32-30-31-38-30-31-30-34-22-7D-5D-7D-7D-04-00-00-00-92-7B-22-49-44-22-3A-22-43-4C-41-4E-5F-43-48-45-53-54-22-2C-22-50-61-72-61-6D-73-22-3A-7B-22-53-74-61-72-74-54-69-6D-65-22-3A-22-32-30-31-37-30-33-31-37-54-30-37-30-30-30-30-2E-30-30-30-5A-22-2C-22-41-63-74-69-76-65-44-75-72-61-74-69-6F-6E-22-3A-22-50-33-64-54-30-68-22-2C-22-49-6E-61-63-74-69-76-65-44-75-72-61-74-69-6F-6E-22-3A-22-50-34-64-54-30-68-22-2C-22-43-68-65-73-74-54-79-70-65-22-3A-5B-22-43-6C-61-6E-43-72-6F-77-6E-73-22-5D-7D-7D-00".HexaToBytes());

            Stream.WriteVInt(this.ChestSlotCount);

            for (int I = 0; I < this.ChestSlotCount; I++)
            {
                if (this.Chests.Count > I)
                {
                    if (this.Chests[I] != null)
                    {
                        Stream.WriteBoolean(true);
                        this.Chests[I].Encode(Stream);

                        continue;
                    }
                }

                Stream.WriteBoolean(false);
            }

            this.FreeChestTimer.Encode(Stream);
            this.DonationCapacityCooldownTimer.Encode(Stream);

            Stream.WriteBoolean(this.FreeChest != null);

            if (this.FreeChest != null)
            {
                this.FreeChest.Encode(Stream);
            }

            Stream.WriteBoolean(this.PurchasedChest != null);

            if (this.PurchasedChest != null)
            {
                this.PurchasedChest.Encode(Stream);
            }

            Stream.EncodeData(null); // ShopData
            Stream.WriteBoolean(false);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0); // v3 + 636

            if ( /* v3 + 636 > 0 */ false)
            {
                Stream.WriteVInt(0);
                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(0);

            Stream.AddRange("90-C1-C0-01-84-F7-D2-01-92-F9-9C-9E-0B".HexaToBytes()); // Timer
            Stream.AddRange("90-C1-C0-01-84-F7-D2-01-92-F9-9C-9E-0B".HexaToBytes()); // Timer

            Stream.WriteBoolean(this.StarChest != null);

            if (this.StarChest != null)
            {
                this.StarChest.Encode(Stream);
            }

            /* ----- */

            this.RequestCooldownTimer.Encode(Stream);

            // 2817 -> Name Pop Up -> 0000101100000001
            // 2819 -> Name Pop Up -> 0000101100000011

            Stream.WriteVInt(3);

            for (int J = 0; J < 7; J++)
            {
                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(this.PageOpened);
            Stream.WriteVInt(this.LastLevelUpPopup);
            Stream.EncodeData(this.LastArena);

            Stream.WriteVInt(this.ShopDay);
            Stream.WriteVInt(this.ShopSeed); // 9A-05
            Stream.WriteVInt(this.ShopDaySeen);

            this.ShopTimer.Encode(Stream);

            Stream.WriteBoolean(this.ShopChest != null);

            if (this.ShopChest != null)
            {
                this.ShopChest.Encode(Stream);
            }

            this.ShareTimer.Encode(Stream);
            this.SendMailTimer.Encode(Stream);
            this.ElderKickTimer.Encode(Stream);

            Stream.WriteVInt(this.FreeChestIdx);
            Stream.WriteVInt(this.CrownChestIdx);
            Stream.WriteVInt(0); // 0x01
            Stream.WriteVInt(0); // tvMajor
            Stream.WriteVInt(0); // tvBuild
            Stream.WriteVInt(0); // tvContent
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(this.ReplaysSeen.Count);

            foreach (int ReplayId in this.ReplaysSeen)
            {
                Stream.WriteVInt(ReplayId);
            }

            Stream.EncodeSpellList(new List<SpellData>(0)); // Spell Request

            Stream.WriteBoolean(false);

            Stream.WriteVInt(10);

            Stream.WriteVInt(0); // Array Count
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);
            Stream.WriteBoolean(false);

            this.SpellDeck.Encode(Stream);

            /* ------------------------ */

            Stream.WriteBoolean(false);

            if (false)
            {
                Stream.WriteBoolean(true);

                // sub_2CB81A()
                {
                    // ?
                }
            }

            Stream.WriteBoolean(false);

            if (false)
            {
                Stream.WriteBoolean(true);

                // sub_2CBB8E()
                {
                    // ?
                }
            }

            /* ------------------------ */

            Stream.WriteVInt(0); // Spell Array

            for (int i = 0; i < 0; i++)
            {
                Stream.WriteVInt(0); // Spell Type
                Stream.WriteVInt(0); // Spell Id
            }

            Stream.WriteVInt(0); // Spell Array

            for (int i = 0; i < 0; i++)
            {
                Stream.WriteVInt(0); // Spell Type
                Stream.WriteVInt(0); // Spell Id
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(this.BoughtPacks.Count);

            foreach (CsvData Pack in this.BoughtPacks)
            {
                Stream.WriteVInt(Pack.GlobalId);
            }

            // Stream.AddRange("01-00-01-90-81-A1-FE-0B-00-15-01".HexaToBytes());
            
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(0); // 0x01

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);

            Stream.WriteVInt(this.NewArenasSeen.Count);

            foreach (ArenaData Arena in this.NewArenasSeen)
            {
                Stream.WriteVInt(Arena.GlobalId);
            }

            Stream.AddRange("02-00-9E-8B-26-00-5A-21-00-57-5A-24-F4-D7-B4-B8-9B-0C-00-00-00-00-00-00-00-00-00-01-04-04-01-00-00-00-00-19-54-49-44-5F-43-41-53-54-5F-51-55-45-53-54-5F-4D-49-4E-5F-45-4C-49-58-49-52-00-00-00-00-1E-00-00-00-00-00-13-43-61-73-74-5F-51-75-65-73-74-5F-48-69-67-68-43-6F-73-74-00-00-00-1E-54-49-44-5F-43-41-53-54-5F-51-55-45-53-54-5F-4D-49-4E-5F-45-4C-49-58-49-52-5F-49-4E-46-4F-00-00-00-08-73-63-2F-75-69-2E-73-63-00-00-00-0E-71-75-65-73-74-5F-69-74-65-6D-5F-70-76-70-14-01-0E-01-09-00-00-00-06-00-01-01-00-06-02-00-00-00-00-15-54-49-44-5F-52-45-51-55-45-53-54-5F-51-55-45-53-54-5F-41-4E-59-00-00-00-00-03-02-00-00-00-00-0B-52-65-71-75-65-73-74-5F-41-6E-79-00-00-00-1A-54-49-44-5F-52-45-51-55-45-53-54-5F-51-55-45-53-54-5F-41-4E-59-5F-49-4E-46-4F-00-00-00-08-73-63-2F-75-69-2E-73-63-00-00-00-0E-71-75-65-73-74-5F-69-74-65-6D-5F-70-76-70-14-01-0E-00-26-01-00-00-00-00-01-03-00-00-00-00-14-54-49-44-5F-46-52-45-45-5F-43-48-45-53-54-5F-51-55-45-53-54-00-00-00-00-03-02-00-00-00-00-10-51-75-65-73-74-5F-46-72-65-65-43-68-65-73-74-73-00-00-00-14-54-49-44-5F-46-52-45-45-5F-43-48-45-53-54-5F-51-55-45-53-54-00-00-00-08-73-63-2F-75-69-2E-73-63-00-00-00-15-71-75-65-73-74-5F-69-74-65-6D-5F-66-72-65-65-5F-63-68-65-73-74-05-03-13-07-01-13-07-01-13-07-01-03-01-01-00-03-00-04-04-01-00-00-7F-00-04-04-00-00-00-00-19-54-49-44-5F-43-41-53-54-5F-51-55-45-53-54-5F-4D-41-58-5F-45-4C-49-58-49-52-00-00-00-00-32-00-00-00-00-00-12-43-61-73-74-5F-51-75-65-73-74-5F-4C-6F-77-43-6F-73-74-00-00-00-1E-54-49-44-5F-43-41-53-54-5F-51-55-45-53-54-5F-4D-41-58-5F-45-4C-49-58-49-52-5F-49-4E-46-4F-00-00-00-08-73-63-2F-75-69-2E-73-63-00-00-00-0E-71-75-65-73-74-5F-69-74-65-6D-5F-70-76-70-14-01-05-01-8A-05-00-00-00-00-02-01-01-00-01-02-A9-13-01-00-00-00-26-51-75-C3-AA-74-65-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-00-00-00-1C-69-63-6F-6E-5F-71-75-65-73-74-5F-74-79-70-65-5F-73-70-65-63-69-61-6C-65-76-65-6E-74-06-00-00-00-00-00-00-00-00-00-41-47-61-67-6E-65-7A-20-36-C2-A0-63-6F-75-72-6F-6E-6E-65-73-20-64-61-6E-73-20-6C-65-20-64-C3-A9-66-69-20-C3-A9-6C-65-63-74-72-6F-63-75-74-65-75-72-73-20-63-6F-6E-74-72-65-20-63-68-61-73-73-65-75-72-00-00-00-08-73-63-2F-75-69-2E-73-63-00-00-00-16-71-75-65-73-74-5F-69-74-65-6D-5F-73-70-65-63-69-61-6C-5F-70-76-70-14-01-05-01-A8-0F-02-00-00-00-01-A8-13-19-32-01-00-00-9A-05-05-01-00-07-01-00-00-00-00-01-00-00-00-00-00-00-00-00-00-00-0C-04-00-02-01-04-02-02-01-04-04-04-00-04-00-02-03-05-04-05".HexaToBytes());
            
            Stream.AddRange("00-04-03-02-02  00  7F-7F-00-00  B6-01".HexaToBytes());

            Stream.WriteVInt(this.ShopItems.Count);

            foreach (ShopItem ShopItem in this.ShopItems)
            {
                ShopItem.Encode(Stream);
            }

            Stream.WriteVInt(this.ShopChests.Count);

            foreach (ChestEvent ChestEvent in this.ShopChests)
            {
                ChestEvent.Encode(Stream);
            }

            Stream.AddRange("00-03-00-01-02-00-00-00-00-00-7F-95-11-00-00".HexaToBytes());
        }
    }
}