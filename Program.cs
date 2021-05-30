using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {

            GetRegex();

            GetHtmlPage("https://sm.news/the-national-interest-takticheskoe-yadernoe-oruzhie-vs-rf-pugaet-nato-60798/");

            //Encoding unicode = Encoding.Unicode;
            //Encoding utf16 = Encoding.BigEndianUnicode;
            //Encoding utf8 = Encoding.UTF8;

            //string myText = "apple!";

            //byte[] unicodeByte = unicode.GetBytes(myText);
            //byte[] utf16Byte = Encoding.Convert(unicode, utf16, unicodeByte);
            //byte[] utf8Byte = Encoding.Convert(unicode, utf8, unicodeByte);
            //NewMethod(unicodeByte);
            //NewMethod(utf16Byte);
            //NewMethod(utf8Byte);

            //PrintLine();

            //Console.ReadLine();

            //var swUtf8 = new StreamWriter("text.txt", true, Encoding.UTF8);
            //swUtf8.WriteLine("Hello word!");
            //swUtf8.Close();

            //var swUtf32 = new StreamWriter("text.txt", true, Encoding.UTF32);
            //swUtf32.WriteLine("Hello word!");
            //swUtf32.Close();

            Console.ReadLine();
        }

        private static void NewMethod(byte[] unicodeByte)
        {
            foreach (byte b in unicodeByte)
                Console.Write($"{b}:");
            PrintLine();
        }

        private static void PrintLine()
        {
            Console.WriteLine(Environment.NewLine);
        }



        private static void GetRegex()
        {
            const string pattern = @"^\d*\D+\d+$";
            var regex = new Regex(pattern);

            string input = "";

            while (true)
            {
                //string input = Console.ReadKey().KeyChar.ToString();
                input = Console.ReadLine();

                if (input == " ")
                    break;


                Console.WriteLine($"{(regex.IsMatch(input) ? "\tСоответствуе шаблону" : "\tНе соответствует шаблону")}");

            }




        }

        private static void GetHtmlPage(string pagelink)
        {


            if (pagelink == "" || pagelink == " ")
            {

            }
            else
            {
                //Отправляем запрос,где textBox1 - строка с адресом

                System.Net.WebRequest reqGET = System.Net.WebRequest.Create(pagelink);
                System.Net.WebResponse resp = reqGET.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                //Получаем ответ в переменную sr и считываем его до конца
                System.IO.StreamReader sr = new System.IO.StreamReader(stream, Encoding.Default);
                string s = sr.ReadToEnd();

                //Выводим всю лабуду в richTextBox1
                var myFile = new StreamWriter("text.txt");
                myFile.WriteLine(s);
                myFile.Close();
            }


        }


    }


}
