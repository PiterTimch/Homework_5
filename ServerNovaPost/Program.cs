using ServerNovaPost.Services;
using System.Text;

namespace ServerNovaPost
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Завантаження...");

            try
            {
                NovaPostService novaPostService = new NovaPostService();
                await novaPostService.SeedAreasAsync();

                Console.WriteLine("Фініш!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
