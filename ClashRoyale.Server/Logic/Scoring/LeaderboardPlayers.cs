namespace ClashRoyale.Logic.Scoring
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Player;

    public class LeaderboardPlayers
    {
        public const int SeasonMaxPlayers     = 200;
        public const int LastSeasonMaxPlayers = 3;

        public bool IsGlobal;

        public RegionData Region;

        public List<AvatarRankingEntry> Players;
        public List<AvatarRankingEntry> LastSeason;

        public DateTime StartTime;
        public TimeSpan Duration;

        public TimeSpan TimeLeft
        {
            get
            {
                return this.StartTime - (DateTime.UtcNow - this.Duration);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderboardPlayers"/> class.
        /// </summary>
        /// <param name="Region">The region.</param>
        public LeaderboardPlayers(RegionData Region = null)
        {
            this.Players    = new List<AvatarRankingEntry>(LeaderboardPlayers.SeasonMaxPlayers);
            this.LastSeason = new List<AvatarRankingEntry>(LeaderboardPlayers.LastSeasonMaxPlayers);

            this.StartTime  = DateTime.UtcNow;
            this.Duration   = TimeSpan.FromHours(1);

            this.Region     = Region;

            if (this.Region == null)
            {
                this.IsGlobal = true;
            }
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <param name="Player">The player.</param>
        public async void AddEntry(Player Player)
        {
            AvatarRankingEntry TopPlayer         = this.Players.Find(RankedPlayer => RankedPlayer.EntryId == Player.PlayerId);
            AvatarRankingEntry BypassedPlayer    = null;

            return;

            if (TopPlayer != null)
            {
                TopPlayer.Initialize(Player);

                await Task.Run(() =>
                {
                    foreach (AvatarRankingEntry ScoredPlayer in this.Players)
                    {
                        if (ScoredPlayer.EntryId == TopPlayer.EntryId)
                        {
                            continue;
                        }

                        if (ScoredPlayer.IsBetter(TopPlayer))
                        {
                            break;
                        }

                        BypassedPlayer = ScoredPlayer;
                    }
                });

                if (BypassedPlayer != null)
                {
                    this.Bypass(TopPlayer, BypassedPlayer, false);
                }
                else
                {
                    Logging.Info(this.GetType(), "The player is not good or bad enough to move in leaderboard.");
                }
            }
            else
            {
                if (Player.IsNameSet == false)
                {
                    return;
                }

                TopPlayer = new AvatarRankingEntry(Player);
                
                await Task.Run(() =>
                {
                    foreach (AvatarRankingEntry ScoredPlayer in this.Players)
                    {
                        if (ScoredPlayer.IsBetter(TopPlayer))
                        {
                            break;
                        }

                        BypassedPlayer = ScoredPlayer;
                    }
                });

                if (BypassedPlayer != null)
                {
                    this.Bypass(TopPlayer, BypassedPlayer, true);
                }
                else
                {
                    int PlayerCount = this.Players.Count;

                    if (PlayerCount < LeaderboardPlayers.SeasonMaxPlayers)
                    {
                        TopPlayer.Order         = PlayerCount;
                        TopPlayer.PreviousOrder = PlayerCount;

                        this.Players.Add(TopPlayer);
                    }
                    else
                    {
                        Logging.Info(this.GetType(), "The player is not good enough to be ranked.");
                    }
                }
            }
        }

        /// <summary>
        /// Bypasses the specified top player.
        /// </summary>
        /// <param name="TopPlayer">The top player.</param>
        /// <param name="BypassedPlayer">The bypassed player.</param>
        /// <param name="NewlyRanked">if set to <c>true</c> [newly ranked].</param>
        public void Bypass(AvatarRankingEntry TopPlayer, AvatarRankingEntry BypassedPlayer, bool NewlyRanked)
        {
            int BypassedIndex = this.Players.IndexOf(BypassedPlayer);
            int CurrentIndex  = this.Players.IndexOf(TopPlayer);
            
            if (NewlyRanked)
            {
                if (this.Players.Count == LeaderboardPlayers.SeasonMaxPlayers)
                {
                    this.Players.RemoveAt(LeaderboardPlayers.SeasonMaxPlayers - 1);
                }
            }
            else
            {
                this.Players.RemoveAt(CurrentIndex);
            }

            TopPlayer.Order         = BypassedPlayer.Order;
            TopPlayer.PreviousOrder = CurrentIndex;

            this.Players.Insert(BypassedIndex, TopPlayer);
        }
    }
}