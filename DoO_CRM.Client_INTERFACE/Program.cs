using DoO_CRM.BL;
using DoO_CRM.BL.Controller;
using DoO_CRM.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DoO_CRM.ClientINTERFACE
{
    class Program
    {
        static void Main()
        {
            var context = new DoO_CRMContext();
            Client client = null;
            Cart cart = new Cart(client);

            Console.WriteLine("Добро пожаловать, клиент! \n");
            Console.WriteLine("Зарегистрированы? Нажмите клавишу - A.");
            Console.WriteLine("Желаете зарегистрироваться? Нажмите клавишу - R.");
            Console.WriteLine("Желаете продолжить без регистрации или авторизации, нажмите любую другую кнопку.");

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.A:
                    Console.Write("\nВведите свой никнейм: ");
                    string nickname = Console.ReadLine();

                    var clientFromDB = ClientController.GetRegistered(nickname, context);

                    if (clientFromDB != null)
                    {
                        Console.WriteLine($"Привет, {nickname}!");
                        client = clientFromDB;
                        cart.Client = client;
                    }
                    break;

                case ConsoleKey.R:

                    Console.Write("\nВведите свой будущий никнейм: ");
                    string newNickname = Console.ReadLine();
                    var newClientFromDB = ClientController.Registration(newNickname, 0 , context);

                    if (newClientFromDB != null)
                    {
                        Console.WriteLine($"Теперь вы зарегистрированы, {newNickname}!");
                        client = newClientFromDB;
                        cart.Client = client;
                    }
                    break;
            }

            Thread.Sleep(6000);
            Console.Clear();

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
                Console.WriteLine("Что бы пополнить баланс, нажмите U.");
                Console.WriteLine("Что бы пополнить корзину, нажмите B.");
                Console.WriteLine("Вы готовы отправить заказ? Нажмите O (англ).");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.U:
                        Console.Write("\nСумма пополнения (введите): ");
                        decimal sum = int.Parse(Console.ReadLine());
                        if (ClientController.UpBalance(client, sum, context))
                        {
                            Console.WriteLine("Пополнение прошло успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Пополнение не было совершено, произошла ошибка.");
                        }
                        break;

                    case ConsoleKey.B:
                        Console.Write("\nНазвание продукта, который собираетесь добавить в корзину (регистр не учитывается): ");
                        string name = Console.ReadLine().ToLower();

                        var findedProduct = productsFromDB.FirstOrDefault(prod => prod.Name.ToLower() == name);

                        if (cart.AddProduct(findedProduct.Id, client.Id, 1, context))
                        {
                            Console.WriteLine("Добавление продукта произошло успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Во время выполнения операции произошла ошибка!");
                        }
                        break;
                    case ConsoleKey.O:
                        if (ClientController.SendOrder(client, cart))
                        {
                            if (ClientController.ApplySells(cart, context) != null)
                            {
                                Console.WriteLine("Заказ успешно сохранён в очереди, ожидайте подтверждения.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Заказ не был сохранён! Обратите внимание, скорее всего, ваш заказ выполняется.");
                        }
                        break;
                }
            }
        }
    }
}
