using ImageSearchApp.Models;

namespace ImageSearchApp.Services
{
    public class PexelsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public PexelsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Pexels:ApiKey"] ?? throw new ArgumentNullException(nameof(configuration), "Pexels API key is missing in configuration.");
        }

        public async Task<List<PexelsImage>?> SearchImagesAsync(string keyword, int page = 1, int perPage = 80)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.pexels.com/v1/search?query={keyword}&page={page}&per_page={perPage}");

            request.Headers.Add("Authorization", _apiKey);
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PexelsApiResponse>();
                return result?.Photos.Select(p => new PexelsImage
                {
                    Id = p.Id,
                    Photographer = p.Photographer,
                    Url = p.Url,
                    SrcMedium = p.Src.Medium,
                    SrcLarge = p.Src.Large
                }).ToList();
            }

            return new List<PexelsImage>();
        }

        public class PexelsApiResponse
        {
            public List<PexelsPhoto> Photos { get; set; } = new List<PexelsPhoto>();
        }

        public class PexelsPhoto
        {
            public int Id { get; set; }
            public string Photographer { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
            public PexelsSrc Src { get; set; } = new PexelsSrc();
        }

        public class PexelsSrc
        {
            public string Medium { get; set; } = string.Empty;
            public string Large { get; set; } = string.Empty;
        }
    }
}
