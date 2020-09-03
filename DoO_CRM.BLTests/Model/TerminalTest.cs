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
        public void TerminalTests()
        {
            //Arrange
            var context = new DoO_CRMContext();

            /*
                В следствии специфики работы внешних ключей, 
                клиент для заказа должен быть сохранён в БД до того, как будет сохранён с заказом.
            */
            var client = ClientController.Registration($"Пользователь {Guid.NewGuid()}", 50000, context);

            Terminal terminal = new Terminal
            {
                TerminalId = 1
            };

            Cart cart = new Cart(client, context.Products.First(), 3, context);
            Order actualOrder = new Order(terminal.TerminalId, client, cart);

            Order expectedOrder = new Order(terminal.TerminalId,
                                            actualOrder.Number,
                                            DateTime.Now,
                                            ClientController.GetSumCostOfSells(cart),
                                            false,
                                            client,
                                            cart);

            //Act
            if (terminal.Enqueue(actualOrder) == false)
            {
                Assert.Fail();
            }

            var savedOrder = terminal.Dequeue(false, terminal.TerminalId, context);
            

            //Assert
            Assert.IsTrue(expectedOrder.Equals(savedOrder), "Ожидаемый заказ не равен заказу актуальному, с.м отладку Equals.");

        }
    }
}
