namespace ClashRoyale.Client.Files.Csv.Logic
{
	using System;

	internal class AchievementData : CsvData
    {
        internal int ActionType;

		/// <summary>
        /// Initializes a new instance of the <see cref="AchievementData"/> class.
        /// </summary>
        /// <param name="CsvRow">The row.</param>
        /// <param name="CsvTable">The data table.</param>
        public AchievementData(CsvRow CsvRow, CsvTable CsvTable) : base(CsvRow, CsvTable)
        {
            // AchievementData.
        }

        /// <summary>
        /// Called when all instances has been loaded for initialized members in instance.
        /// </summary>
		internal override void LoadingFinished()
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
	
        internal int Level
        {
            get; set;
        }

        internal string Tid
        {
            get; set;
        }

        internal string InfoTid
        {
            get; set;
        }

        internal string Action
        {
            get; set;
        }

        internal int ActionCount
        {
            get; set;
        }

        internal int ExpReward
        {
            get; set;
        }

        internal int DiamondReward
        {
            get; set;
        }

        internal int SortIndex
        {
            get; set;
        }

        internal bool Hidden
        {
            get; set;
        }

        internal string AndroidId
        {
            get; set;
        }

        internal string Type
        {
            get; set;
        }

    }
}