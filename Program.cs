using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        public static string pathPageUrl { get; private set; }

        static void Main(string[] args)
        {

            Console.WriteLine("Программа для поиска картинок на странице ресурса\nУкажите адрес странички.");
            pathPageUrl = Console.ReadLine();

            if (pathPageUrl?.Length == 0)
            {
                //pathPage = "http://www.contoso.com/default.html";
                pathPageUrl = "http://sivtrans.ru";
            }

            var pageContent = GetPageContent(pathPageUrl);

            Console.WriteLine("Загрузка контента... Пожалуйста подождите.");

            while (true)
            {
                if (pageContent.IsCompleted)
                {
                    break;
                }
            }

            var result = pageContent.Result;

            Console.WriteLine($"Контент загружен.\nНачать поиск картинок? {ConsoleKey.Y} / {ConsoleKey.N}");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                GetImagePath(result);
            }
            else
            {
                Environment.Exit(0);
            }

            

            Console.ReadLine();

        }

        static async Task<string> GetPageContent(string requestUri)
        {
            WebRequest request = WebRequest.Create(requestUri);
            request.Credentials = CredentialCache.DefaultCredentials;
            using (HttpWebResponse webResponse = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = webResponse.GetResponseStream())
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return await streamReader.ReadToEndAsync();
            }

        }

        private static void GetImagePath(string pageContent)
        {
            List<string> pathCollectionImage = new List<string>();
            string pathNew = "";

            Console.WriteLine(Environment.NewLine);

            string pattern = @"((\<img\s+src=""(?<Full>http(s?).+?)"")|(\<img\s+src=""(?<Short>.+?)""))";
           
            MatchCollection matches = Regex.Matches(pageContent, pattern);

            Console.WriteLine("{0} совпадений найдено\n", matches.Count);

            foreach (Match match in matches)
            {
                pathNew = SubPath(match.Value);
                pathNew = pathPageUrl + "/" + pathNew;
                pathCollectionImage.Add(pathNew);

                Console.WriteLine($"\t{pathNew}");
            }


            GetImageFromUrl(pathCollectionImage);

        }

        private static string SubPath(string path)
        {
            int start = path.IndexOf('"');
            int finish = path.LastIndexOf('"');
            return (path.Substring(++start, finish - start));
        }

        private static void GetImageFromUrl(List<string> listPath)
        {
            foreach(var path in listPath)
            {
                GetImageFromUrl(path);
            }
        }
     
        private static void GetImageFromUrl(string url)
        {                    
           
            List<Image> images = new List<Image>();                 

            try
            {
                using (var stream = new WebClient().OpenRead(url))
                {
                    
                    images.Add(Bitmap.FromStream(stream));

                }
            }
            catch(WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine(e.Message);
                }

            }           
            
        }

        private static void SaveImageToFile(List<Image> images)
        {
            int i = 0;
            string pathFolder = @"C:\Users\User\Desktop\GFile\";
            try
            {
                foreach(Image image in images)
                if (image != null)
                {                   
                    image.Save($"{pathFolder} + img + {i++}");                   
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
