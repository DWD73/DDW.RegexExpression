using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Программа для поиска картинок на странице ресурса\nУкажите адрес странички.");

            string pathPage = Console.ReadLine();

            if (pathPage?.Length == 0)
            {
                pathPage = "http://www.contoso.com/default.html";
            }

            var pageContent = GetPageContent(pathPage);


            Console.WriteLine("Поиск картинок... Ждите.");

            while (true)
            {
                if (pageContent.IsCompleted)
                {
                    break;
                }
            }


            GetImage(pageContent.Result);

            //Console.WriteLine($"Контент загружен.\nНачать поиск картинок? {Command.Y} / {Command.N}");

            //var input = Console.ReadKey().KeyChar;

            //if (Convert.ToChar(Command.Y).Equals(Convert.ToChar(input)))
            //{
            //    GetImage(pageContent.Result);
            //}
            //else
            //{
            //    Environment.Exit(0);
            //}

            //Console.WriteLine(pageContent.Result);

            Console.ReadLine();




            //Console.ReadLine();
        }

        static async Task<string> GetPageContent(string requestUri)
        {

            WebRequest request = WebRequest.Create(requestUri);
            request.Credentials = CredentialCache.DefaultCredentials;
            using HttpWebResponse webResponse = (HttpWebResponse)await request.GetResponseAsync();
            using Stream stream = webResponse.GetResponseStream();
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return await streamReader.ReadToEndAsync();
            }

        }


        private static void GetImage(string pageContent)
        {
            string pattern = @"((\<img\s+src=""(?<Full>http(s?).+?)"")|(\<img\s+src=""(?<Short>.+?)""))";

                ///<img[^>]+src="([^">]+)"/gm
            //const string pattern = @"\d+";

            var regex = new Regex(pattern);

            MatchCollection matches = Regex.Matches(pageContent, pattern);



            foreach (Match match in matches)
            {


                Console.WriteLine(match.Value);


            }




        }
    }

    public enum Command
    {
        Y,
        N
    }


}
