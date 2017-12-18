using BloodSearch.Models.Common;
using System;
using System.Linq;

namespace Web.Services {

    public class UserServices {

        public bool UserIsNotAuthorized(string token) {
            using (var db = new BloodSearchContext()) {
                var date = DateTime.UtcNow;
                return !db.AuthTokens.Any(x => x.Token == token && date < x.ExpiryDate);
            }
        }
    }
}