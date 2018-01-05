namespace ClashRoyale.Messages.Server.Avatar
{
    using ClashRoyale.Enums;
    using ClashRoyale.Logic;

    public class AvatarQuestsDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 21001;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarQuestsDataMessage"/> class.
        /// </summary>
        public AvatarQuestsDataMessage(Device Device) : base(Device)
        {
            // AvatarQuestsDataMessage.
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString("eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdmF0YXJJZCI6MjA2MTczNTE1Njg3LCJpc3MiOiJzdXBlcmNlbGwiLCJleHAiOjE1MDc1OTQyMzcsImlhdCI6MTUwNzU5MzkzN30.FU0IRpT_6qLZahXK1E47uP5xmsNExQCg8Hewywf3qo_3hMktL0lwiNygG07f6ahmIZIRJTf1BK4B7ur9JDQIa_6DIIBd1f3nncww9azGXmx6ZGwrUJaGXA7xtZIdiD75BadQbun7ipoiQDXn32By9fFzgEL1QLjg0U2C4QzJK3ytQu1LeWE121rerX4XWncyur4XrTa-QF6ZQepNbAaAUVc_-pPz8QIpHusJkBfnlV_KJHXR01R1I6aB4vDnBfm3Psw7sDq0R314sBAgX97t1FW_n8Ujm-Xv19k_r5JF8yX4hJrDpFVO9Fi28mTQBCyt_3ejaSMNYsSYUrpI0DF3-w");
        }
    }
}