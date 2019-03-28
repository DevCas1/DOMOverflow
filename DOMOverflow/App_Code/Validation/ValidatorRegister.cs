using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace DOMOverflow {
    public class ValidatorRegister : ValidatorDynamicErrorBase {
        private string username, password, email;

        public ValidatorRegister(string username, string password, string email) : base("Error message not set.") {
            this.username = username;
            this.password = password;
            this.email = email;
        }

        protected override bool IsValid(HttpContextBase ctx, string value) {
            string error;
            bool success = DBManager.RegisterUser(username, password, email, ctx.Session, out error);

            UpdateErrorMessage(error);

            return success;
        }
    }
}