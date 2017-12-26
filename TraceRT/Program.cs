using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TraceRT
{
    class Program
    {
        static string _log;
        static void Main(string[] args)
        {
            string[] url = new string[3];
            url[0] = "ya.ru";
            url[1] = "vk.com";
            url[2] = "youtube.com";
            Console.WriteLine("Starting Trace!");
            try
            {
                while (true)
                {
                    for(int i=0; i<3; i++)
                    {
                        StartLog(url[i]);
                        
                    }
                    System.Threading.Thread.Sleep(180000);
                }
            }
            catch
            {
                Console.WriteLine("Error");
                Console.ReadKey();
            }


           
        }

        static void StartLog(string adr)
        {
            _log = TraceRoute.Traceroute(adr);
            using (FileStream fstream = new FileStream(@"note.txt", FileMode.Append))
            {
                // преобразуем строку в байты
                byte[] array = Encoding.Default.GetBytes(_log);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);

            }
        }

        
    }
}
