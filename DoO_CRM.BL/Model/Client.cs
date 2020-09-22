using DoO_CRM.BL.Model;
using System.Collections.Generic;

namespace DoO_CRM.BL
{
    /// <summary>
    /// Класс клиента, содержащего все необходимые данные, такие как баланс,
    /// имя и список совершённых заказов (но последнее может не работать).
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Создаёт новый экземпляр класса Client на основе входных параметров.
        /// </summary>
        /// <param name="name"> Имя объекта Client. </param>
        /// <param name="optionalBalance"> (опционально) Баланс объекта Client. </param>
        public Client(string name, decimal optionalBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(name) is false && optionalBalance >= 0)
            {
                Name = name;
                Balance = optionalBalance;
            }
        }

        public Client() { }

        #region params

        /// <summary>
        /// Идентификатор клиента в БД.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Баланс пользователя.
        /// </summary>
        public decimal Balance { get; set; }


        /// <summary>
        /// Список совершённых заказов пользователя.
        /// </summary>
        public List<Order> Orders { get; set; }
        #endregion

        #region overrides

        /// <summary>
        /// Возвращает Name объекта Client.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Сравнивает объект Client с другим.
        /// </summary>
        /// <param name="otherClient"> Экземпляр другого класса Client. </param>
        /// <returns> True - если они равны, и false, если не равны. </returns>
        public bool Equals(Client otherClient)
        {
            if (Name == otherClient.Name &&
                Balance == otherClient.Balance)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
