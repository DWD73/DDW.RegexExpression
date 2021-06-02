using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp7
{
    class Program
    {
        public static string pathPageUrl { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Программа для поиска картинок на странице ресурса");

            pathPageUrl = "http://sivtrans.ru/";
                     
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
           
            List<Image> images = new List<Image>();

            foreach (var path in listPath)
            {
                try
                {
                    using (var stream = new WebClient().OpenRead(path))
                    {

                        images.Add(Bitmap.FromStream(stream));
                        
                    }
                }
                catch (WebException e)
                {
                    if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
            }

            SaveImageToFile(images);
            
        }

        private static void SaveImageToFile(List<Image> images)
        {                    
            string pathFolder = CreateFolder($"{Directory.GetCurrentDirectory()}");

            pathFolder = pathFolder + @"\\";

            for(int j = 0; j < images.Count; j++)
            {              
                images[j].Save(pathFolder + j + ".jpg");              
            }

        }

        private static string CreateFolder(string pathFolder)
        {
            pathFolder = pathFolder + @"\\" + "FileFromInet";

            DirectoryInfo dirInfo = new DirectoryInfo(pathFolder);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            
            return dirInfo.FullName;
        }
    }
}
