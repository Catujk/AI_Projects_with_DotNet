using DotNetEnv;
using System.Net.Http.Headers;

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
            Console.WriteLine("Missing API key. Make sure OPENAI_API_KEY is defined in your .env file.");
            return;
        }
        string audioFilePath = Path.Combine("audios", "audio1.mp3");
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var form = new MultipartFormDataContent();

            var audioFile = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
            audioFile.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
            form.Add(audioFile, "file", Path.GetFileName(audioFilePath));
            form.Add(new StringContent("whisper-1"), "model");

            Console.WriteLine("Audio file is being processed, please wait...");

            var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.Write("Text: ");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"An error occurred while processing the audio file: {response.StatusCode}");
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
