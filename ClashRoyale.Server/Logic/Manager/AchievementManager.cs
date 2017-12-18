namespace ClashRoyale.Server.Logic.Manager
{
    using ClashRoyale.Enums;
    using ClashRoyale.Files.Csv;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Maths;
    using ClashRoyale.Server.Logic.Home;
    using ClashRoyale.Server.Logic.Mode;
    using ClashRoyale.Server.Logic.Player;

    internal class AchievementManager
    {
        private readonly GameMode GameMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AchievementManager"/> class.
        /// </summary>
        public AchievementManager(GameMode GameMode)
        {
            this.GameMode = GameMode;
        }

        /// <summary>
        /// Refreshes the specified achievement.
        /// </summary>
        internal void RefreshAchievementProgress(AchievementData Data, int Count)
        {
            if (this.GameMode.State != HomeState.Replay)
            {
                int Existing = this.GameMode.Player.GetAchievementProgress(Data);
                int Value = Math.Min(Count, 1000000000);

                if (Existing < Value)
                {
                    this.GameMode.Player.SetAchievementProgress(Data, Value);
                }
            }
        }

        /// <summary>
        /// Refreshes the status of all achievement progress.
        /// </summary>
        internal void RefreshStatus()
        {
            if (this.GameMode.State != HomeState.Replay)
            {
                var Home = this.GameMode.Home;
                Player Player = this.GameMode.Player;

                CsvFiles.Get(Gamefile.Achievement).Datas.ForEach(Achievement =>
                {
                    AchievementData Data = (AchievementData) Achievement;
                    int Value = 0;

                    switch (Data.ActionType)
                    {
                        case 0:
                        {
                            Value = Player.IsInAlliance ? 1 : 0;
                            break;
                        }
                        case 1:
                        {
                            break;
                        }
                        case 2:
                        {
                            Value = Player.Arena.Arena;
                            break;
                        }
                        case 3:
                        {
                            Value = Home.SpellCount;
                            break;
                        }
                        case 4:
                        {
                            // TODO : Implement LogicClientHome::getReplaySeenCount().
                            break;
                        }
                    }

                    this.RefreshAchievementProgress(Data, Value);
                });
            }
        }

        /// <summary>
        /// Updates a category of achievement progress.
        /// </summary>
        internal void UpdateAchievementProgress(int Type, int AddValue)
        {
            if (this.GameMode.State != HomeState.Replay)
            {
                Home Home       = this.GameMode.Home;
                Player Player   = this.GameMode.Player;

                CsvFiles.Get(Gamefile.Achievement).Datas.ForEach(Achievement =>
                {
                    AchievementData Data = (AchievementData) Achievement;

                    if (Data.ActionType == Type)
                    {
                        this.RefreshAchievementProgress(Data, Player.GetAchievementProgress(Data) + AddValue);
                    }
                });
            }
        }
    }
}