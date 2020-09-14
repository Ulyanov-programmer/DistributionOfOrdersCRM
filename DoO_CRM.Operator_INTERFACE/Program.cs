using DoO_CRM.BL.Controller;
using DoO_CRM.BL.Model;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

            IPAddress localAddr = IPAddress.Parse(ClientController.IpAddress);

            Console.WriteLine("Идёт создание точки подключения...");

            #region CreateServer

            TcpListener server = default;

            try
            {
                server = new TcpListener(localAddr, ClientController.Port);
                server.Start();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Не удалось создать точку подключения, данные об ошибке: {ex.Message}");
            }

            Console.WriteLine("Точка подключения создана и находится в состоянии работы!");
            #endregion

            Console.WriteLine("Добро пожаловать, сотрудник!");
            Thread.Sleep(3000);

            try
            {
                while (true)
                {
                   _ = terminal.WaitingOfOrderAsync(server, terminal);
                    if (terminal.ActualLenghtOfQueue > 0)
                    {
                        Console.WriteLine($"На текущий момент в очереди {terminal.ActualLenghtOfQueue} заказов.");
                        Console.WriteLine("\n Вы подтвердите первый заказ? Y - подтверждение, N - отказ.");
                        var key = Console.ReadKey().Key;
                        string outputMessage = "";

                        if (key == ConsoleKey.Y)
                        {
                            outputMessage = terminal.Dequeue(true, terminal.TerminalId, context);
                        }
                        else if (key == ConsoleKey.N)
                        {
                            outputMessage = terminal.Dequeue(false, terminal.TerminalId, context);
                        }
                        Console.WriteLine(outputMessage);
                        Console.WriteLine("Ожидание следующего заказа.\n");
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
