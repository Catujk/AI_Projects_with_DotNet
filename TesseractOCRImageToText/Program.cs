using System.Text;
using Tesseract;

class Program
{
    public static void Main(string[] args)
    {
        Console.Write("Please enter a image path:");
        var imagePath = Console.ReadLine();

        if (string.IsNullOrEmpty(imagePath))
        {
            Console.WriteLine("Image path cannot be empty.");
            return;
        }

        var tesseractPath = Path.Combine(AppContext.BaseDirectory, "testdata");

        if (!Directory.Exists(tesseractPath))
        {
            Console.WriteLine($"Tesseract data directory not found at {tesseractPath}");
            return;
        }

        try
        {
            // English
            //using (var engine = new TesseractEngine(tesseractPath, "eng", EngineMode.Default))
            
            // Turkish
            using (var engine = new TesseractEngine(tesseractPath, "tur", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine("Recognized text:");
                        Console.WriteLine(text);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");

        }

    }
}