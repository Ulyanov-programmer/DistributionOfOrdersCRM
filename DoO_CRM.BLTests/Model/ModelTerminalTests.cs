using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using DoO_CRM.BL.Model;
using System.Linq;
using DoO_CRM.BL;

namespace DoO_CRM.BLTests.Model
{
    [TestClass()]
    public class ModelTerminalTests
    {
        [TestMethod()]
        public void SaveOrderTests()
        {
            //Arrange
            var context = new DoO_CRMContext();
            var terminal = new Terminal(1);

            /*
                В следствии специфики работы внешних ключей, 
                клиент для заказа должен быть сохранён в БД до того, как будет сохранён с заказом.
            */
            var client = ClientController.Registration($"Пользователь {Guid.NewGuid()}", context, 50000);

            //Перед выполнением нижестоящего метода, следует убедиться, что в БД в таблице Products есть элементы.
            Cart cart = new Cart(context.Products.First(), 3, context, client);

            Order actualOrder = new Order(client, cart);

            Order expectedOrder = new Order(terminal,
                                            actualOrder.Number,
                                            DateTime.Now,
                                            ClientController.GetSumCostOfSells(cart),
                                            false,
                                            client);

            //Act
            if (terminal.Enqueue(actualOrder) == false)
            {
                Assert.Fail("Заказ не был добавлен в очередь!");
            }

            terminal.Dequeue(false, terminal.TerminalId, context);


            //Assert
            Assert.IsTrue(expectedOrder.Equals(actualOrder), "Ожидаемый заказ не равен заказу актуальному, с.м Equals.");

        }

        [TestMethod()]
        public void SaveSellsTests()
        {
            //Arrange
            var context = new DoO_CRMContext();
            var client = ClientController.Registration($"Пользователь {Guid.NewGuid()}", context, 50000);

            //Перед выполнением нижестоящих методов, следует убедиться, что в БД в таблице Products есть элементы.
            var firstProduct = context.Products.First();
            var secondProduct = context.Products.First(prod => prod.Id == 1 || prod.Id == 2);

            var expendedSells = new List<Sell>()
            {
                new Sell(firstProduct, firstProduct.Id, 1, client.Id),
                new Sell(secondProduct, secondProduct.Id, 1, client.Id)
            };

            Cart cart = new Cart(client);
            cart.AddProduct(expendedSells);

            if (ClientController.GetSumCostOfSells(cart) <= firstProduct.Cost)
            { Assert.Fail("Суммарная стоимость корзины меньше или равна стоимости первого продукта из БД!"); }

            //Act
            var savedSells_fromCart = ClientController.ApplySells(cart, context);

            /*  
                Поскольку элементы в коллекциях сохранённых из корзины и БД различаются по сортировке, 
                их нужно привести в один вид сортировки, не забыв взять именно последние элемнты.
                Нет, TakeLast не работает с context.
            */
            var savedSells_fromDb = context.Sells.OrderByDescending(sell => sell.SellId)
                                                 .Take(expendedSells.Count)
                                                 .OrderBy(sell => sell.SellId)
                                                 .ToList();

            //Assert

            savedSells_fromCart.SequenceEqual(savedSells_fromDb);
        }
    }
}
