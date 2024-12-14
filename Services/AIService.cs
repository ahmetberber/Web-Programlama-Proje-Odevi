using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HairSalonManagement.Services
{
    public class AiService
    {
        private readonly HttpClient _httpClient;

        public AiService(string apiKey)
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> AnalyzeImageAsync(byte[] imageBytes)
        {
            string base64Image = Convert.ToBase64String(imageBytes);

            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = "Görsele göre en uygun saç modelini önerir misiniz?"
                            },
                            new
                            {
                                type = "image_url",
                                image_url = new
                                {
                                    url = $"data:image/jpeg;base64,{base64Image}"
                                }
                            }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Hatası: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonDocument.Parse(responseContent);

            var choices = result.RootElement.GetProperty("choices");
            if (choices.ValueKind != JsonValueKind.Array || choices.GetArrayLength() == 0)
            {
                throw new Exception("API response does not contain choices.");
            }

            var message = choices[0].GetProperty("message");
            if (message.ValueKind != JsonValueKind.Object || !message.TryGetProperty("content", out var content))
            {
                throw new Exception("API response does not contain message content.");
            }

            return content.GetString() ?? throw new Exception("Message content is null.");
        }
    }
}
