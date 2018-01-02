namespace ClashRoyale.Logic.Commands
{
    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Files.Csv.Logic;
    using ClashRoyale.Logic.Mode;
    using ClashRoyale.Logic.Player;

    public class ClaimAchievementRewardCommand : Command
    {
        private AchievementData AchievementData;

        /// <summary>
        /// Gets the type of this command.
        /// </summary>
        public override int Type
        {
            get
            {
                return 523;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimAchievementRewardCommand"/> class.
        /// </summary>
        public ClaimAchievementRewardCommand()
        {
            // ClaimAchievementRewardCommand.
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode(ByteStream Stream)
        {
            this.AchievementData = Stream.DecodeData<AchievementData>();
            base.Decode(Stream);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode(ChecksumEncoder Stream)
        {
            Stream.EncodeData(this.AchievementData);
            base.Encode(Stream);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override byte Execute(GameMode GameMode)
        {
            Player Player = GameMode.Player;

            if (Player != null)
            {
                if (this.AchievementData != null)
                {
                    if (Player.IsAchievementCompleted(this.AchievementData))
                    {
                        if (!Player.GetIsAchievementRewardClaimed(this.AchievementData))
                        {
                            Player.XpGainHelper(this.AchievementData.ExpReward);
                            Player.AddFreeDiamonds(this.AchievementData.DiamondReward);
                            Player.SetAchievementRewardClaimed(this.AchievementData, 1);

                            GameMode.AchievementManager.RefreshStatus();

                            return 0;
                        }

                        return 4;
                    }

                    return 3;
                }

                return 2;
            }

            return 1;
        }
    }
}