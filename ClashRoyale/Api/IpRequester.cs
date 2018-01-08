namespace ClashRoyale.Api
{
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using RestSharp;

    public static class IpRequester
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IpRequester"/> has been initialized.
        /// </summary>
        public static bool Initialized
        {
            get;
            set;
        }

        private static RestClient Rest;
        private static RestRequest Request;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Initialize()
        {
            if (IpRequester.Initialized)
            {
                return;
            }

            IpRequester.Rest      = new RestClient("http://ip-api.com");
            IpRequester.Request   = new RestRequest("json/{ip}", Method.GET);

            Request.AddParameter("fields", "countryCode,region,city,status");

            IpRequester.Initialized = true;
        }

        /// <summary>
        /// Executes a GET request to the Rest API.
        /// </summary>
        public static async Task<IpResponse> GetIpInfo(string IpAddress)
        {
            Request.AddOrUpdateParameter("ip", IpAddress, ParameterType.UrlSegment);

            var Result      = await Rest.ExecuteTaskAsync(Request);
            var IpResponse  = JsonConvert.DeserializeObject<IpResponse>(Result.Content);

            return IpResponse;
        }
    }
}
