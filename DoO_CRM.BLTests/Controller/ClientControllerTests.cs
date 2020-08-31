using Microsoft.VisualStudio.TestTools.UnitTesting;
using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using DoO_CRM.BL.Model;
using System.Linq;

namespace DoO_CRM.BL.Controller.Tests
{
    [TestClass()]
    public class ProductControllerTests
    {
        [TestMethod()]
        public void ShowProductsTest()
        {
            //Arrange
            var rnd = new Random();
            var products = new List<Product>();

            Product product1 = new Product("product1",
                                            rnd.Next(0, 10000),
                                            rnd.Next(1, 50));

            Product product2 = new Product("product2",
                                            rnd.Next(0, 10000),
                                            rnd.Next(1, 50));

            Product product3 = new Product("product3",
                                            rnd.Next(0, 10000),
                                            rnd.Next(1, 50));

            for (int i = 0; i < 5; i++)
            {
                products.Add(product1);
            }
            for (int i = 0; i < 3; i++)
            {
                products.Add(product2);
            }
            products.Add(product3);
            

            List<string> expectedData = new List<string>()
            {
                $"{product1.Name}, количество - 5 шт.",
                $"{product2.Name}, количество - 3 шт.",
                $"{product3.Name}, количество - 1 шт."
            };

            Cart cart = new Cart(null);
            cart.AddProduct(products);

            //Act
            List<string> result = ProductController.ShowProducts(cart, false);

            //Assert
            bool isEqual = expectedData.SequenceEqual(result);
            Assert.IsTrue(isEqual);
        }
    }
}