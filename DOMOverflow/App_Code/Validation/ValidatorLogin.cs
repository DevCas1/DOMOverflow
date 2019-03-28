using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.WebPages;

namespace DOMOverflow {
    public class ValidatorLogin : ValidatorDynamicErrorBase {
        private string username, password;

        public ValidatorLogin(string username, string password) : base("Error message not set.") {
            this.username = username;
            this.password = password;
        }

        protected override bool IsValid(HttpContextBase ctx, string value) {
            string error;
            bool success = DBManager.LoginUser(username, password, ctx.Session, out error);

            UpdateErrorMessage(error);

            return success;
        }
    }
}