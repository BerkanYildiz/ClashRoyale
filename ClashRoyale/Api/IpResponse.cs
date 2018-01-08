namespace ClashRoyale.Api
{
    using Newtonsoft.Json;

    public class IpResponse
    {
        [JsonProperty("city")]          public string City;
        [JsonProperty("countryCode")]   public string Country;
        [JsonProperty("region")]        public string Region;
        [JsonProperty("status")]        public string Status;

        /// <summary>
        /// Initializes a new instance of the <see cref="IpResponse"/> class.
        /// </summary>
        public IpResponse()
        {
            // IpResponse.
        }

        /// <summary>
        /// Gets a value indicating whether the request has been a success.
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return this.Status == "success";
            }
        }
    }
}
