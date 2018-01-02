namespace ClashRoyale.Logic.Replay
{
    using ClashRoyale.Extensions.Utils;
    using ClashRoyale.Logic.Battle.Event;
    using ClashRoyale.Logic.Commands;
    using ClashRoyale.Logic.Commands.Manager;
    using ClashRoyale.Logic.Mode;

    using Newtonsoft.Json.Linq;

    public class Replay
    {
        public GameMode GameMode;

        public int EndTick;
        public int RandomSeed;
        public int Time;

        public JArray Commands;
        public JArray Events;

        /// <summary>
        /// Gets the replay json.
        /// </summary>
        public JObject Json
        {
            get
            {
                JObject Json = new JObject();

                if (this.GameMode.Battle != null)
                {
                    Json.Add("battle", this.GameMode.Battle.SaveReplay());
                }

                Json.Add("endTick", this.EndTick);
                Json.Add("cmd", this.Commands);
                Json.Add("evt", this.Events);
                Json.Add("rndSeed", this.RandomSeed);
                Json.Add("time", TimeUtil.Timestamp);

                return Json;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Replay"/> class.
        /// </summary>
        public Replay(GameMode GameMode)
        {
            this.GameMode   = GameMode;
            this.EndTick    = GameMode.Time;
            this.RandomSeed = GameMode.RandomSeed;

            this.Time       = TimeUtil.Timestamp;

            this.Commands   = new JArray();
            this.Events     = new JArray();
        }

        /// <summary>
        /// Records the specified command.
        /// </summary>
        public void RecordCommand(Command Command)
        {
            this.Commands.Add(CommandManager.SaveCommandToJson(Command));
        }

        /// <summary>
        /// Records the specified event.
        /// </summary>
        public void RecordEvent(BattleEvent Event)
        {
            this.Events.Add(Event.Save());
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        public void Tick()
        {
            ++this.EndTick;
        }
    }
}