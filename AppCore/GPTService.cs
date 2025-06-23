using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YoutubeAutoFactory
{
    public class GPTService
    {
        private readonly string apiKey;
        private readonly HttpClient httpClient;

        public GPTService(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("❌ OpenAI API 키가 설정되어 있지 않습니다. config.json을 확인하세요.");
            }

            this.apiKey = apiKey;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> GenerateScript(string topic)
        {
            var prompt = $"다음 주제에 대한 유튜브 영상 대본을 작성해줘: {topic}";

            var requestBody = new
            {
                model = "gpt-4",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                max_tokens = 1000
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody);
            return result.choices[0].message.content;
        }
    }
}
