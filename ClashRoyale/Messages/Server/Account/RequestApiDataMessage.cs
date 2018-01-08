namespace ClashRoyale.Messages.Server.Account
{
    using ClashRoyale.Enums;
    using ClashRoyale.Extensions;

    public class RequestApiDataMessage : Message
    {
        /// <summary>
        /// Gets the type of this message.
        /// </summary>
        public override short Type
        {
            get
            {
                return 22726;
            }
        }

        /// <summary>
        /// Gets the service node of this message.
        /// </summary>
        public override Node ServiceNode
        {
            get
            {
                return Node.Account;
            }
        }

        public string ApiKey = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdmF0YXJJZCI6ODU4OTk0ODkxMCwiaXNzIjoic3VwZXJjZWxsIiwiZXhwIjoxNTEzODgwOTU0LCJpYXQiOjE1MTM4ODA2NTR9.I1hz-8yhay5b1hOXEQ845cvzQhLR-cHCcaAdNfAhtcBMGcqCpeFEWzQt3epjMzuscUWi2vXvFlumZgRBIGz9y2dbpaxQf_4u4qLZXOQDp1UyhYoZlntQpxHq5JLjcRTajlcvQQGvOgjYa-Ys2uXfbeDEMwqJ4BS_Ex_8gaSFuPlnSSo7WDx-vEEbHRJlUbfU5wzWjIXKLA-OALS1UiGyhxGTOZXV1dZZ8VH8VBDxrZA2Y4UlQVcR9DQY-ES1qh-xunD3YrYjmnEs3f_2PXuWac6DRQfm7G9t-DyzWIM51b2wHbjm-yjZsOGy1q8IEQSN7wNDiMuRbKiY_2eYa-x4ug";

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestApiDataMessage"/> class.
        /// </summary>
        public RequestApiDataMessage()
        {
            // RequestApiDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestApiDataMessage"/> class.
        /// </summary>
        /// <param name="Stream">The stream.</param>
        public RequestApiDataMessage(ByteStream Stream) : base(Stream)
        {
            // RequestApiDataMessage.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestApiDataMessage"/> class.
        /// </summary>
        /// <param name="ApiKey">The API key.</param>
        public RequestApiDataMessage(string ApiKey)
        {
            this.ApiKey = ApiKey;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public override void Decode()
        {
            this.ApiKey = this.Stream.ReadString();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public override void Encode()
        {
            this.Stream.WriteString(this.ApiKey);
        }
    }
}