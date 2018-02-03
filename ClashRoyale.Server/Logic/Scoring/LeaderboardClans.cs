namespace ClashRoyale.Logic.Scoring
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Alliance.Entries;

    public class LeaderboardClans
    {
        public const int SeasonMaxClans       = 200;
        public const int LastSeasonMaxClans   = 3;

        public bool IsGlobal;

        public RegionData Region;

        public List<AllianceRankingEntry> Clans;
        public List<AllianceRankingEntry> LastSeason;

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
        /// Initializes a new instance of the <see cref="LeaderboardClans"/> class.
        /// </summary>
        /// <param name="Region">The region.</param>
        public LeaderboardClans(RegionData Region = null)
        {
            this.Clans      = new List<AllianceRankingEntry>(LeaderboardClans.SeasonMaxClans);
            this.LastSeason = new List<AllianceRankingEntry>(LeaderboardClans.LastSeasonMaxClans);

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
        /// <param name="HeaderEntry">The alliance header entry.</param>
        public async void AddEntry(AllianceHeaderEntry HeaderEntry)
        {
            AllianceRankingEntry TopAlliance    = this.Clans.Find(T => T.EntryId == HeaderEntry.ClanId);
            AllianceRankingEntry BypassedClan   = null;

            if (TopAlliance != null)
            {
                TopAlliance.Initialize(HeaderEntry);

                await Task.Run(() =>
                {
                    foreach (AllianceRankingEntry ScoredClan in this.Clans)
                    {
                        if (ScoredClan.EntryId == TopAlliance.EntryId)
                        {
                            continue;
                        }

                        if (ScoredClan.IsBetter(TopAlliance))
                        {
                            break;
                        }

                        BypassedClan = ScoredClan;
                    }
                });

                if (BypassedClan != null)
                {
                    this.Bypass(TopAlliance, BypassedClan, false);
                }
                else
                {
                    Logging.Info(this.GetType(), "The alliance is not good or bad enough to move in leaderboard.");
                }
            }
            else
            {
                TopAlliance = new AllianceRankingEntry(HeaderEntry);

                await Task.Run(() =>
                {
                    foreach (AllianceRankingEntry ScoredClan in this.Clans)
                    {
                        if (ScoredClan.IsBetter(TopAlliance))
                        {
                            break;
                        }

                        BypassedClan = ScoredClan;
                    }
                });

                if (BypassedClan != null)
                {
                    this.Bypass(TopAlliance, BypassedClan, true);
                }
                else
                {
                    int AllianceCount = this.Clans.Count;

                    if (AllianceCount < LeaderboardClans.SeasonMaxClans)
                    {
                        TopAlliance.Order           = AllianceCount;
                        TopAlliance.PreviousOrder   = AllianceCount;

                        this.Clans.Add(TopAlliance);
                    }
                    else
                    {
                        Logging.Info(this.GetType(), "The alliance is not good enough to be ranked.");
                    }
                }
            }
        }

        /// <summary>
        /// Bypasses the specified top clan.
        /// </summary>
        /// <param name="TopClan">The top clan.</param>
        /// <param name="BypassedClan">The bypassed clan.</param>
        /// <param name="NewlyRanked">if set to <c>true</c> [newly ranked].</param>
        public void Bypass(AllianceRankingEntry TopClan, AllianceRankingEntry BypassedClan, bool NewlyRanked)
        {
            int BypassedIndex = this.Clans.IndexOf(BypassedClan);
            int CurrentIndex  = this.Clans.IndexOf(TopClan);
            
            if (NewlyRanked)
            {
                if (this.Clans.Count == LeaderboardClans.SeasonMaxClans)
                {
                    this.Clans.RemoveAt(LeaderboardClans.SeasonMaxClans - 1);
                }
            }
            else
            {
                this.Clans.RemoveAt(CurrentIndex);
            }

            this.Clans.Insert(BypassedIndex, TopClan);
        }
    }
}