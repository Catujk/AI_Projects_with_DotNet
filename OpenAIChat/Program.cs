using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        var apiKey = "Your OpenAI Api Key";

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
            var response = await httpClient.PostAsync("https://api.openai.com/v1/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var responseJson = JsonConvert.DeserializeObject<JsonElement>(responseString);
                var answer = responseJson.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
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