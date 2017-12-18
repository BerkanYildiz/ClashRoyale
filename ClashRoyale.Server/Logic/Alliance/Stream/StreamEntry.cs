namespace ClashRoyale.Server.Logic.Alliance.Stream
{
    using System;

    using ClashRoyale.Extensions;
    using ClashRoyale.Extensions.Helper;
    using ClashRoyale.Server.Logic.Player;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    [JsonConverter(typeof(StreamEntryConverter))]
    internal class StreamEntry
    {
        internal int HighId;
        internal int LowId;

        internal int SenderHighId;
        internal int SenderLowId;
        internal int SenderExpLevel;
        internal int SenderRole;

        internal string SenderName;

        internal bool Removed;

        internal DateTime CreationDateTime;

        /// <summary>
        /// Gets age in seconds.
        /// </summary>
        internal int AgeSeconds
        {
            get
            {
                return (int) DateTime.UtcNow.Subtract(this.CreationDateTime).TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the stream id.
        /// </summary>
        internal long StreamId
        {
            get
            {
                return (uint) this.HighId << 32 | (uint) this.LowId;
            }
        }

        /// <summary>
        /// Gets the type of this stream entry.
        /// </summary>
        internal virtual int Type
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEntry"/> class.
        /// </summary>
        internal StreamEntry()
        {
            this.CreationDateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamEntry"/> class.
        /// </summary>
        /// <param name="Sender">The sender.</param>
        internal StreamEntry(Player Sender) : this()
        {
            this.SenderHighId   = Sender.HighId;
            this.SenderLowId    = Sender.LowId;
            this.SenderName     = Sender.Name;
            this.SenderExpLevel = Sender.ExpLevel;
            this.SenderRole     = Sender.AllianceRole;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode(ByteStream Stream)
        {
            Stream.WriteLogicLong(this.HighId, this.LowId);
            Stream.WriteLogicLong(this.SenderHighId, this.SenderLowId);
            Stream.WriteLogicLong(this.SenderHighId, this.SenderLowId); // HomeID

            Stream.WriteString(this.SenderName);

            Stream.WriteVInt(this.SenderExpLevel);
            Stream.WriteVInt(this.SenderRole);
            Stream.WriteVInt(this.AgeSeconds);

            Stream.WriteBoolean(this.Removed);
        }

        /// <summary>
        /// Loads this instance from json.
        /// </summary>
        internal virtual void Load(JToken JToken)
        {
            if (JsonHelper.GetJsonObject(JToken, "base", out JToken Base))
            {
                JsonHelper.GetJsonNumber(Base, "highId", out this.HighId);
                JsonHelper.GetJsonNumber(Base, "lowId", out this.LowId);

                JsonHelper.GetJsonNumber(Base, "sender_highId", out this.SenderHighId);
                JsonHelper.GetJsonNumber(Base, "sender_lowId", out this.SenderLowId);

                JsonHelper.GetJsonString(Base, "sender_name", out this.SenderName);
                JsonHelper.GetJsonNumber(Base, "sender_expLevel", out this.SenderExpLevel);
                JsonHelper.GetJsonNumber(Base, "sender_role", out this.SenderRole);
                JsonHelper.GetJsonDateTime(Base, "sender_creation", out this.CreationDateTime);
            }
            else
            {
                Logging.Error(this.GetType(), "Error, the stream entry JSON didn't contain any base.");
            }
        }

        /// <summary>
        /// Saves this instance to json.
        /// </summary>
        internal virtual JObject Save()
        {
            JObject Base = new JObject();

            Base.Add("highId", this.HighId);
            Base.Add("lowId", this.LowId);

            Base.Add("sender_highId", this.SenderHighId);
            Base.Add("sender_lowId", this.SenderLowId);

            Base.Add("sender_name", this.SenderName);
            Base.Add("sender_expLevel", this.SenderExpLevel);
            Base.Add("sender_role", this.SenderRole);
            Base.Add("sender_creation", this.CreationDateTime);

            return new JObject
            {
                {
                    "type", this.Type
                },
                {
                    "base", Base
                }
            };
        }
    }
}