using DoO_CRM.BL.Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DoO_CRM.BL.Model;
using System.Linq;

namespace DoO_CRM.BL.Controller.Tests
{
    [TestClass()]
    public class ClientControllerTests
    {
        [TestMethod()]
        public void RegistrationTest()
        {
            //Arrange
            var context = new DoO_CRMContext();
            var client = new Client($"Пользователь {Guid.NewGuid()}", 0);
            ClientController.Registration(client, context);

            //Act
            var outputClient = ClientController.GetRegistered(client.Name, context);

            //Assert
            Assert.IsTrue(client.Equals(outputClient), "Объекты Client из теста и из БД не равны!");
        }

        [TestMethod()]
        public void UpBalanceTest()
        {
            //Arrange
            var context = new DoO_CRMContext();
            var client = new Client($"Пользователь {Guid.NewGuid()}", 0);
            decimal newBalanceOfClient = new Random().Next(0, 5000000);

            ClientController.Registration(client, context);

            //Act
            decimal resultOfMethod = ClientController.UpBalance(client, newBalanceOfClient, context);
            if (resultOfMethod is -1)
            {
                Assert.Fail("Метод увеличения баланса вернул недопустимое значение, скорее всего, он не был корректно выполнен!");
            }

            decimal balanceOf_ClietFromDb = context.Clients.First(clt => clt.Name == client.Name &&
                                                                     clt.Balance == newBalanceOfClient)
                                                           .Balance;

            //Assert
            Assert.AreEqual(newBalanceOfClient, balanceOf_ClietFromDb, "Баланс пользователя в БД не был изменён!");
        }
    }

    [TestClass()]
    public class ProductControllerTests
    {
        [TestMethod()]
        public void ShowProductsInCartTest()
        {
            //Arrange
            var context = new DoO_CRMContext();
            var products = new List<Product>();

            //Adding products in list with products.
            for (int id = 0; products.Count < 3; id++)
            {
                if (context.Products.Find(id) != null)
                {
                    Product newProduct = context.Products.Find(id);
                    products.Add(newProduct);
                }
            }
            if (products.Count <= 1)
            { Assert.Fail("Лист продуктов не был заполнен достаточным количеством экземпляров!"); }

            //Create expectedData for the method SequenceEqual.
            List<string> expectedData = new List<string>();
            foreach (var product in products)
            {
                expectedData.Add($"{product.Name}, стоимость: {product.Cost}");
            }

            //Adding products in Cart.
            Cart cart = new Cart();
            cart.AddProduct(products);


            //Act
            List<string> result = ProductController.ShowProductsInCart(cart, context);


            //Assert
            Assert.IsTrue(expectedData.SequenceEqual(result), "Результат выполнения метода и ожидаемые данные не равны!");
        }
    }
}