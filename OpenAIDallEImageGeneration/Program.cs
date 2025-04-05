using DotNetEnv;
using Newtonsoft.Json;
using System.Text;

class Program
{
    public static async Task Main(string[] args)
    {
        Env.Load();
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Missing API key. Make sure OPENAI_API_KEY is defined in your .env file.");
            return;
        }
        Console.Write("Prompt: ");
        var prompt = Console.ReadLine();
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var requestBody = new
            {
                prompt = prompt,
                n = 1,
                size = "1024x1024"
            };
            var jsonBody = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.openai.com/v1/images/generations", content);
            var responseString = await response.Result.Content.ReadAsStringAsync();
            if (response.Result.IsSuccessStatusCode)
            {
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                var imageUrl = responseJson["data"][0]["url"].ToString();
                Console.WriteLine($"Image URL: {imageUrl}");
            }
            else
            {
                Console.WriteLine($"An error occurred: {response.Result.StatusCode}");
                Console.WriteLine(responseString);
            }

        }
    }
}