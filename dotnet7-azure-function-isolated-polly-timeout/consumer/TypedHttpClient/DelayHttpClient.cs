namespace consumer.TypedHttpClient
{
    public class DelayHttpClient
    {
        private readonly HttpClient _httpClient;

        public DelayHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetProducer(string endpoint) =>
            await _httpClient.GetStringAsync(endpoint);
    }
}