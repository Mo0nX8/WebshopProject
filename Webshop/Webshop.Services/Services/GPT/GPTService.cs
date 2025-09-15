using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Services.Services.GPT
{
    public class GPTService
    {
        public static async Task<string> AskAsync(string prompt, string apiKey)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var payload = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 200
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("OpenAI Response: " + json);

            dynamic data = JsonConvert.DeserializeObject(json);
            if (data == null || data.choices == null || data.choices.Count == 0 || data.choices[0].message == null)
            {
                return "Sajnálom, nem sikerült választ kapni az AI-tól.";
            }

            return data.choices[0].message.content != null
                   ? data.choices[0].message.content.ToString()
                   : "Sajnálom, az AI nem adott választ.";
        }
    }
}
