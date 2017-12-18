using System;

namespace Web.Infrastructure {

    public static class Crypt {

        public static string GetToken() {
            return Guid.NewGuid().ToString("N");
        }
    }
}