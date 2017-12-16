namespace ClashRoyale.Server.Logic.Scoring
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ClashRoyale.Server.Files.Csv.Logic;
    using ClashRoyale.Server.Logic.Scoring.Entries;

    internal class LeaderboardClans
    {
        internal const int SeasonMaxClans       = 200;
        internal const int LastSeasonMaxClans   = 3;

        internal bool IsGlobal;

        internal RegionData Region;

        internal List<AllianceRankingEntry> Clans;
        internal List<AllianceRankingEntry> LastSeason;

        internal DateTime StartTime;
        internal TimeSpan Duration;

        internal TimeSpan TimeLeft
        {
            get
            {
                return this.StartTime - (DateTime.UtcNow - this.Duration);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderboardClans"/> class.
        /// </summary>
        internal LeaderboardClans(RegionData Region = null)
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
        /// <param name="Clan">The alliance.</param>
        internal async void AddEntry(Clan Clan)
        {
            AllianceRankingEntry TopAlliance    = this.Clans.Find(T => T.Id == Clan.AllianceId);
            AllianceRankingEntry BypassedClan   = null;

            if (TopAlliance != null)
            {
                TopAlliance.Initialize(Clan);

                await Task.Run(() =>
                {
                    foreach (AllianceRankingEntry ScoredClan in this.Clans)
                    {
                        if (ScoredClan.Id == TopAlliance.Id)
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
                TopAlliance = new AllianceRankingEntry(Clan);

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
        internal void Bypass(AllianceRankingEntry TopClan, AllianceRankingEntry BypassedClan, bool NewlyRanked)
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