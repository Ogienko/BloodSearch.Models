using System;

namespace BloodSearch.Models.Common.DAL {

    /// <summary>
    /// Авторизация токенов
    /// </summary>
    public class AuthToken {

        public int Id { get; set; }

        /// <summary>
        /// Токен авторизации
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Дата создания токена
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата окончания действия токена
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// С какого ip была провизведена авторизация
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// Номер пользователя, которому принадлежит токен
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Пользователь, которому принадлежит токен
        /// </summary>
        public User User { get; set; }
    }
}