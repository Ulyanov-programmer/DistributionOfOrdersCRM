using System;

namespace DoO_CRM.BL
{
    /// <summary>
    /// Класс-контейнер, содержащий данные об объекте Product сохранённом в базе данных.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Создаёт новый экземпляр Product на основе входных параметров. 
        /// </summary>
        /// <param name="name"> Имя нового экземпляра Product. </param>
        /// <param name="cost"> Стоимость нового экземпляра Product. </param>
        /// <param name="count"> Количество этого товара на складе. </param>
        public Product(string name, decimal cost, int count)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Cost = cost;
            Count = count;
        }
        public Product() { }

        #region params

        public int Id { get; set; }

        /// <summary>
        /// Строковое представление имени этого экземпляра Product.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Стоимость этого экземпляра Product.
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Количество этого товара на складе.
        /// </summary>
        public int Count { get; set; }

        #endregion

        #region overrides

        /// <summary>
        /// Возвращает поле Name этого экземпляра Product.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Сравнивает этот экземпляр Product с другим.
        /// </summary>
        /// <param name="otherProduct"> Другой экземпляр Product. </param>
        /// <returns> True - если сравниваемые объекты равны по всем сравниваемым параметрам, иначе - false. </returns>
        public bool Equals(Product otherProduct)
        {
            if (Name == otherProduct.Name &&
                Cost == otherProduct.Cost)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
