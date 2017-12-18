using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodSearch.Models.Common.DAL {

    [Table("users")]
    public class User {

        public int Id { get; set; }

        /// <summary>
        /// Почта 256 символов
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Хэш пароля 256 символов
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Ip 45 символов
        /// </summary>
        public string RegisterFromIp { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// История авторизации
        /// </summary>
        public List<AuthToken> AuthTokens { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }
    }
}