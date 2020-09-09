using DoO_CRM.BL.Controller;
using DoO_CRM.BL.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace DoO_CRM.INTERFACE
{
    public class Program
    {
        private static void Main()
        {
            var context = new DoO_CRMContext();
            var terminal = new Terminal
            {
                TerminalId = 1
            };
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            const int port = 8080;

            Console.WriteLine("Идёт создание точки подключения...");

            #region CreateServer
            TcpListener server = default;

            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Не удалось создать точку подключения, данные об ошибке: {ex.Message}");
            }
            Console.WriteLine("Точка подключения создана и находится в состоянии работы!");

            #endregion

            Console.WriteLine("Добро пожаловать, сотрудник!");

            try
            {
                while (true)
                {
                    var result = terminal.WaitingOfOrderAsync(server, terminal);
                    if (result.Result >= 1)
                    {
                        Console.WriteLine($"На текущий момент в очереди {result.Result} заказов.");
                        Console.WriteLine("\n Вы подтвердите первый заказ? Y - подтверждение, N - отказ.");
                        var key = Console.ReadKey().Key;

                        if (key == ConsoleKey.Y)
                        {
                            terminal.Dequeue(true, terminal.TerminalId, context);
                        }
                        else if (key == ConsoleKey.N)
                        {
                            terminal.Dequeue(false, terminal.TerminalId, context);
                        }
                    }
                    continue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка! \n" + ex.Message);
                throw;
            }







        }
    }
}
