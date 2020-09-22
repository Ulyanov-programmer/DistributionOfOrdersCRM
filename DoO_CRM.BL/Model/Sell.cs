using System;

namespace DoO_CRM.BL.Model
{
    /// <summary>
    /// Класс-контейнер, содержащий данные о покупке определённого товара,
    /// такие как: идентификатор покупателя, данные о продукте, их количество.
    /// </summary>
    public class Sell
    {
        /// <summary>
        /// Создаёт объект Sell на основе входных аргументов.
        /// </summary>
        /// <param name="product"> Экземпляр Product, используемый для хранения данных о нём в этом Sell. </param>
        /// <param name="productId"> Идентификатор сохраняемого экземпляра Product. </param>
        /// <param name="countOfProduct"> Указатель на "количество" сохраняемых экземпляров Product.
        ///                               (используется только для БД)                              </param>
        /// <param name="clientId"> Идентификатор клиента, которому будет принадлежать данный экземпляр Sell. </param>
        public Sell(Product product, int productId, int countOfProduct, int? clientId)
        {
            Product = product;
            ProductId = productId;
            CountOfProduct = countOfProduct;
            ClientId = clientId;
        }
        public Sell() { }


        public int SellId { get; set; }

        /// <summary>
        /// Указатель на "количество" объектов Product в экземпляре Sell.
        /// </summary>
        public int CountOfProduct { get; set; }

        #region References

        public int? ClientId { get; set; }
        public int ProductId { get; set; }
        public Client Client { get; set; }
        public Product Product { get; set; }

        /// <summary>
        /// Содержит данные о заказе, к которому принадлежит этот экземпляр Sell.
        /// </summary>
        public Order Order { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// Возвращает данные о Sell в виде интерполированной строки.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return @$"Продукт ""{Product.Name}"", количество - {CountOfProduct} шт.";
        }

        /// <summary>
        /// Сравнивает этот экземпляр Sell с другим.
        /// </summary>
        /// <param name="otherSell"> Другой экземпляр Sell. </param>
        /// <returns> True - если сравниваемые объекты равны по всем сравниваемым параметрам, иначе - false. </returns>
        public bool Equals(Sell otherSell)
        {
            if (ClientId == otherSell.ClientId &&
                ProductId == otherSell.ProductId)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
