namespace ClashRoyale.Files.Csv.Logic
{
    using System;

    public class AchievementData : CsvData
    {
        public int ActionType;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AchievementData" /> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AchievementData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AchievementData.
        }

        public int Level { get; set; }

        public string Tid { get; set; }

        public string InfoTid { get; set; }

        public string Action { get; set; }

        public int ActionCount { get; set; }

        public int ExpReward { get; set; }

        public int DiamondReward { get; set; }

        public int SortIndex { get; set; }

        public bool Hidden { get; set; }

        public string AndroidId { get; set; }

        public string Type { get; set; }

        /// <summary>
        ///     Called when all instances has been loaded for initialized members in instance.
        /// </summary>
        public override void LoadingFinished()
        {
            if (this.ActionCount < 0)
            {
                throw new Exception("achievements.csv: Invalid ActionCount");
            }

            switch (this.Action)
            {
                case "jointeam":
                {
                    this.ActionType = 0;
                    break;
                }
                case "donate":
                {
                    this.ActionType = 1;
                    break;
                }
                case "reacharena":
                {
                    this.ActionType = 2;
                    break;
                }
                case "findcard":
                {
                    this.ActionType = 3;
                    break;
                }
                case "watchtv":
                {
                    this.ActionType = 4;
                    break;
                }
                case "tournament":
                {
                    this.ActionType = 5;
                    break;
                }
                case "tournamenthost":
                {
                    this.ActionType = 6;
                    break;
                }
                case "jointournament":
                {
                    this.ActionType = 7;
                    break;
                }
                case "winstreak":
                {
                    this.ActionType = 8;
                    break;
                }
                case "friendlybattle":
                {
                    this.ActionType = 9;
                    break;
                }
                case "survivalevent":
                {
                    this.ActionType = 10;
                    break;
                }
                default:
                {
                    this.ActionType = -1;
                    break;
                }
            }
        }
    }
}