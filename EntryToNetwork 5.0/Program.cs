using System;
using System.Net;
using System.Text;

namespace EntryToNetwork_5._0
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //using(HttpClient client = new HttpClient())
            //{
            //    try
            //    {
            //        HttpResponseMessage response = await client.GetAsync("https://example.com");
            //
            //        response.EnsureSuccessStatusCode();
            //
            //
            //        string responseBody = await response.Content.ReadAsStringAsync();
            //
            //        Console.WriteLine(responseBody);
            //    }
            //    catch (HttpRequestException ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();

            Console.WriteLine("Cервер запущено на http://localhost:8080/");

            while(true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                HttpListenerRequest request = context.Request;


                HttpListenerResponse response = context.Response;

                Console.WriteLine($"Отримано запит: {request.Url}");

                //string responseString = "<h1>Hello, HTTP!</h1>";
                //byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                string filePath;

                switch (request.Url.LocalPath)
                {
                    case "/":
                        filePath = "index.html";
                        break;
                    case "/about":
                        filePath = "about.html";
                        break;
                    case "/contact":
                        filePath = "contact.html";
                        break;
                    default:
                        filePath = null; // Шлях не знайдено
                        break;
                }

                //string filePath = "C:\\Users\\OLXza_fu90\\source\\repos\\EntryToNetwork 5.0\\EntryToNetwork 5.0\\index.html";


                byte[] buffer;


                if (File.Exists(filePath))
                {
                    buffer = await File.ReadAllBytesAsync(filePath);
                    response.ContentType = "text/html; charset=utf-8";
                }
                else
                {
                    string errorMsg = "<h1>Помилка 404: Файл не знайдено</h1>";
                    buffer = Encoding.UTF8.GetBytes(errorMsg);
                    response.ContentType = "text/html; charset=utf-8";
                }

                response.ContentLength64 = buffer.Length;
                //response.ContentType = "text/html; charset=utf-8";


                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                response.Close();
            }
        }
    }
}
