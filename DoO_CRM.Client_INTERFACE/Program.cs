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
            #region PreliminaryData

            var context = new DoO_CRMContext();
            Client client = null;
            Cart cart = new Cart();

            #endregion

            #region Authorization

            while (client is null)
            {
                Console.Clear();
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
                        else
                        {
                            Console.WriteLine("Пользователя с таким именем не существует!");
                            Thread.Sleep(6000);
                        }
                        break;

                    case ConsoleKey.R:

                        Console.Write("\nВведите свой будущий никнейм: ");
                        string newNickname = Console.ReadLine();
                        var newClientFromDB = ClientController.Registration(newNickname, context);

                        if (newClientFromDB != null)
                        {
                            Console.WriteLine($"Теперь вы зарегистрированы, {newNickname}!");
                            client = newClientFromDB;
                            cart.Client = client;
                        }
                        else
                        {
                            Console.WriteLine("Произошла ошибка при регистрации, возможно, вы ввели некорректные значения!");
                            Thread.Sleep(6000);
                        }
                        break;
                }
            }
            #endregion

            Thread.Sleep(6000);

            List<Product> productsFromDB = ProductController.GetTop10Products(false, context);
            bool listIsNotNull = productsFromDB.Count > 0;

            while (true)
            {
                Console.Clear();
                if (listIsNotNull)
                {
                    Console.WriteLine("Вам доступны следующие товары: \n");
                    foreach (var product in productsFromDB)
                    {
                        Console.WriteLine($"Название: {product.Name}");
                        Console.WriteLine($"Стоимость: {product.Cost}");
                        Console.WriteLine($"Количество: {product.Count} \n");
                    }

                    Console.WriteLine("Что бы пополнить корзину, нажмите B (англ).");
                    Console.WriteLine("Что бы пополнить отправить заказ, нажмите O (англ).");
                    Console.WriteLine("Что бы просмотреть корзину, нажмите S.");
                }
                Console.WriteLine("Что бы пополнить баланс, нажмите U.");
                Console.WriteLine($"Ваш баланс - {client.Balance}");

                switch (Console.ReadKey().Key)
                {
                    #region UpBalance (key U)
                    case ConsoleKey.U:
                        Console.Write("\nСумма пополнения (введите): ");
                        decimal sum = int.Parse(Console.ReadLine());

                        if (ClientController.UpBalance(client, sum, context) != -1)
                        {
                            Console.WriteLine($"Пополнение прошло успешно! Теперь ваш баланс составляет: {client.Balance}");
                        }
                        else
                        {
                            Console.WriteLine("Пополнение не было совершено, произошла ошибка!");
                            Console.WriteLine("Возможно, вы ввели некорректные значения.");
                        }

                        Thread.Sleep(6000);
                        break;
                    #endregion

                    #region AddingProductInCart (key B)
                    case ConsoleKey.B:
                        Console.Write("\nНазвание продукта, который собираетесь добавить в корзину (регистр не учитывается): ");
                        string name = Console.ReadLine().ToLower();

                        var findedProduct = productsFromDB.FirstOrDefault(prod => prod.Name.ToLower() == name);

                        if (findedProduct is null)
                        {
                            Console.WriteLine("Во время выполнения операции произошла ошибка, продукта с таким именем не существует!");
                        }
                        else if (cart.AddProduct(findedProduct, 1, context, client.Id))
                        {
                            Console.WriteLine("Добавление продукта произошло успешно!");
                        }
                        else
                        {
                            Console.WriteLine("Во время выполнения операции произошла неизвестная ошибка!");
                        }

                        Thread.Sleep(6000);
                        break;
                    #endregion

                    #region SendOrder (key O)
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
                            Console.WriteLine("Заказ не был отправлен!");
                            Console.WriteLine("Обратите внимание, скорее всего, ваш предыдущий заказ выполняется,");
                            Console.WriteLine(" или сервер недоступен.");
                        }

                        Thread.Sleep(6000);
                        break;
                    #endregion

                    #region ShowProductsInCart (key S)
                    case ConsoleKey.S:
                        Console.Clear();

                        List<string> data = new List<string>();
                        data = ProductController.ShowProductsInCart(cart, context);

                        if (data.Count > 0)
                        {
                            foreach (var strig in data)
                            {
                                Console.WriteLine(strig);
                            }
                            Console.WriteLine($"\nСуммарная стоимость корзины: {ClientController.GetSumCostOfSells(cart)}");

                            Console.WriteLine("Введите что угодно и/или нажмите Enter, что-бы продолжить.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Ваша корзина пуста.");
                            Thread.Sleep(6000);
                        }
                        break;
                    #endregion
                }
            }
        }
    }
}
