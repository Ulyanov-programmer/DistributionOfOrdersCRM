using DoO_CRM.BL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoO_CRM.BL.Model
{
    /// <summary>
    /// Виртуальная сущность корзины, хранит список объектов Sell и данные о клиенте.
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// Создаёт объект Cart с первым экземпляром Sell в списке и данными о клиенте.
        /// </summary>
        /// <param name="product"> Данные о продукте, необходимые для создания экземпляра Sell. </param>
        /// <param name="countOfProducts"> Количество добавляемых продуктов одного типа. (не работает) </param>
        /// <param name="context"> Экземпляр класса контекста, необходимый для подключения к БД. </param>
        /// <param name="optionalClient"> (опционально) Данные о клиенте, владельце этой корзины. </param>
        public Cart(Product product, int countOfProducts, DoO_CRMContext context, Client optionalClient = null)
        {
            Client = optionalClient;
            if (optionalClient is null)
            {
                AddProduct(product, countOfProducts, context);
            }
            else
            {
                AddProduct(product, countOfProducts, context, optionalClient.Id);
            }
        }

        /// <summary>
        /// Создаёт объект Cart с данными о клиенте.
        /// </summary>
        /// <param name="optionalClient"> (опционально) Данные о клиенте, владельце этой корзины. </param>
        public Cart(Client optionalClient = null)
        {
            Client = optionalClient;
        }

        #region params

        /// <summary>
        /// Данные о клиенте, владельце этой корзины.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Список экземпляров Sell этой корзины.
        /// </summary>
        public List<Sell> Sells = new List<Sell>();
        #endregion

        #region methods

        /// <summary>
        /// Добавляет в список Sell новый экземпляр для этого объекта Cart.
        /// </summary>
        /// <param name="product"> Данные о продукте, необходимые для создания экземпляра Sell. </param>
        /// <param name="countOfProducts"> Количество добавляемых продуктов одного типа. (не работает) </param>
        /// <param name="context"> Экземпляр класса контекста, необходимый для подключения к БД. </param>
        /// <param name="optionalClientId"> (опционально) Идентификатор клиента в БД, владельца этой корзины. </param>
        /// <returns> Если добавление было совершено, возвращает true. Иначе - false. </returns>
        public bool AddProduct(Product product, int countOfProducts, DoO_CRMContext context, int? optionalClientId = null)
        {
            if (product != null && countOfProducts > 0 && context != null)
            {
                var productFromDB = context.Products.FirstOrDefault(prod => prod.Id == product.Id);

                if (productFromDB != default)
                {
                    Sell newSell = new Sell(productFromDB, product.Id, countOfProducts, optionalClientId);

                    Sells.Add(newSell);
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Добавляет в список Sell новые экземпляры для этого объекта Cart.
        /// Не желательно использовать где угодно, кроме тестов.
        /// </summary>
        /// <param name="inputSells"> Список входных экземпляров Sell. </param>
        /// <returns> Если добавление было совершено, возвращает true. </returns>
        public bool AddProduct(List<Sell> inputSells)
        {
            Sells.AddRange(inputSells);
            return true;
        }

        /// <summary>
        /// Добавляет в список Sell новые экземпляры для этого объекта Cart.
        /// </summary>
        /// <param name="inputProducts"> Список входных экземпляров Product. </param>
        /// <param name="optionalClientId"> (опционально) Идентификатор клиента в БД, владельца этой корзины. </param>
        /// <returns> Если добавление было совершено, возвращает true. Иначе - false. </returns>
        public bool AddProduct(List<Product> inputProducts, int? optionalClientId = null)
        {
            if (inputProducts != null && inputProducts.Count > 0)
            {
                foreach (var product in inputProducts)
                {
                    if (product != null)
                    {
                        Sell newSell = new Sell(product, product.Id, 1, optionalClientId);

                        Sells.Add(newSell);
                    }
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}
