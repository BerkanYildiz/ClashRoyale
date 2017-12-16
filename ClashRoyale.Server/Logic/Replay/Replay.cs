namespace ClashRoyale.Server.Logic
{
    using ClashRoyale.Server.Extensions.Utils;
    using ClashRoyale.Server.Logic.Commands;
    using ClashRoyale.Server.Logic.Commands.Manager;
    using ClashRoyale.Server.Logic.Event;
    using ClashRoyale.Server.Logic.Mode;

    using Newtonsoft.Json.Linq;

    internal class Replay
    {
        internal GameMode GameMode;

        internal int EndTick;
        internal int RandomSeed;
        internal int Time;

        internal JArray Commands;
        internal JArray Events;

        /// <summary>
        /// Gets the replay json.
        /// </summary>
        internal JObject Json
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
        internal void RecordCommand(Command Command)
        {
            this.Commands.Add(CommandManager.SaveCommandToJson(Command));
        }

        /// <summary>
        /// Records the specified event.
        /// </summary>
        internal void RecordEvent(BattleEvent Event)
        {
            this.Events.Add(Event.Save());
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            ++this.EndTick;
        }
    }
}