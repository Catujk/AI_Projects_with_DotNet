using DotNetEnv;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

class Program
{
    static async Task Main(string[] args)
    {
        // .env dosyasını yükle
        Env.Load();

        // Ortam değişkeninden API key'i al
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("API key bulunamadı. Lütfen .env dosyasına OPENAI_API_KEY tanımlayın.");
            return;
        }

        Console.Write("Question: ");

        var prompt = Console.ReadLine();
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are a helpful assistant."
                },
                new
                {
                    role = "user",
                    content = prompt
                }

            },
            max_tokens = 500
        };
        var jsonData = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonNode.Parse(responseString);
                var answer = responseJson["choices"]?[0]?["message"]?["content"]?.ToString();
                Console.WriteLine($"Answer: {answer}");
            }
            else
            {
                Console.WriteLine($"Error: {responseString}");
                Console.WriteLine($"Error Code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}