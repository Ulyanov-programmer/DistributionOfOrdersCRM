using DoO_CRM.BL;
using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;

namespace DoO_CRM.ClientINTERFACE
{
    class Program
    {
        static void Main(string[] args)
        {
            DoO_CRMContext context = new DoO_CRMContext();
            Client client = null;
            Console.WriteLine("Добро пожаловать, клиент! \n");
            Console.WriteLine("Если вы зарегистрированы, нажмите клавишу A.");
            Console.WriteLine("Если вы не желаете продолжить без регистрации или авторизации, нажмите любую другую кнопку.");
            Console.WriteLine("Если вы желаете зарегистрироваться, нажмите клавишу R.");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.A:
                    Console.Clear();
                    Console.Write("Введите свой никнейм: ");
                    string nickname = Console.ReadLine();

                    var clientFromDB = ClientController.GetRegistered(nickname, context);

                    if (clientFromDB != null)
                    {
                        Console.WriteLine($"Привет, {nickname}!");
                        client = clientFromDB;
                    }
                    break;

                case ConsoleKey.R:
                    Console.Clear();
                    Console.Write("Введите свой будущий никнейм: ");
                    string newNickname = Console.ReadLine();
                    var newClientFromDB = ClientController.Registration(newNickname, context);

                    if (newClientFromDB != null)
                    {
                        Console.WriteLine($"Вы зарегистрированы, {newNickname}!");
                        client = newClientFromDB;
                    }
                    break;
            }

            List<Product> productsFromDB = ProductController.GetTop100Products(false, context);

            if (productsFromDB.Count > 0)
            {
                Console.WriteLine("Вам доступны следующие товары: \n");
                foreach (var product in productsFromDB)
                {
                    Console.WriteLine($"Название: {product.Name}");
                    Console.WriteLine($"Стоимость: {product.Cost}");
                    Console.WriteLine($"Количество: {product.Count} \n");
                }
            }
            while (true)
            {
                Console.WriteLine("Что-бы пополнить баланс, нажмите U.");
                Console.WriteLine("Что-бы заполнить заказ на покупку, нажмите B.");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.U:


                        break;

                    case ConsoleKey.B:
                        break;

                }
            }
        }
    }
}
