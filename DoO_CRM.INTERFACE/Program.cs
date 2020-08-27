using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DoO_CRM.INTERFACE
{
    class Program
    {
        static void Main()
        {
            byte cassId = 0;
            Console.WriteLine("Добро пожаловать, сотрудник!");
            Console.WriteLine("Идёт создание точки подключения...");

            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            TcpListener server = default;

            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось создать точку подключения, данные об ошибке: {ex.Message}");
            }
            Console.WriteLine("Точка подключения создана и находится в состоянии работы!");

            while (true)
            {
                Console.WriteLine("Ожидание подключения...");

                TcpClient client = server.AcceptTcpClient();

                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                NetworkStream stream = client.GetStream();

                byte[] data = new byte[512];
                int bytesOfData = stream.Read(data, 0, data.Length);
                string orderData = Encoding.UTF8.GetString(data, 0, bytesOfData);

            }








        }
    }
}
