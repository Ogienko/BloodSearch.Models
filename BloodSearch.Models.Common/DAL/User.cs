﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodSearch.Models.Common.DAL {

    [Table("users")]
    public class User {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordRestoreHash { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}