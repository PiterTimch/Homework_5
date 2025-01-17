using ServerNovaPost.Services;
using System.Text;

namespace ServerNovaPost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            NovaPostService novaPostService = new NovaPostService();
            novaPostService.SeedAreas();

            Console.WriteLine("Фініш!");
        }
    }
}
