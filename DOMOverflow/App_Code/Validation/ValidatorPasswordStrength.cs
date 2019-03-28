using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace DOMOverflow {
    public class ValidatorPasswordStrength : ValidatorDynamicErrorBase {
        public const string AllowedSpecialChars = "";

        private int min, max;
        private bool lower, upper, number, special;

        public ValidatorPasswordStrength(int minLength, int maxLength, bool reqLower, bool reqUpper, bool reqNumber, bool reqSpecial) : base("No error message set.") {
            this.min     = minLength;
            this.max     = maxLength;
            this.lower   = reqLower;
            this.upper   = reqUpper;
            this.number  = reqNumber;
            this.special = reqSpecial;
        }

        protected override bool IsValid(HttpContextBase ctx, string value) {
            List<string> issues = new List<string>();

            if (value.Length < min || value.Length > max) issues.Add("Het wachtwoord moet tussen de " + min + " en " + max + " karakters lang zijn.");
            if (lower   && !value.ContainsAny("abcdefghijklmnopqrstuvwxyz"))            issues.Add("Het wachtwoord moet minimaal 1 kleine letter bevatten.");
            if (upper   && !value.ContainsAny("ABCDEFGHIJKLMNOPQRSTUVWXYZ"))            issues.Add("Het wachtwoord moet minimaal 1 hoofdletter bevatten.");
            if (number  && !value.ContainsAny("0123456789"))                            issues.Add("Het wachtwoord moet minimaal 1 getal bevatten.");
            if (special && !value.ContainsAny("~`!@#$%^&*(){}[];:\"',.<>/?\\|-_=+"))    issues.Add("Het wachtwoord moet minimaal 1 speciaal karakter (een van: ~`!@#$%^&*(){}[];:\"',.<>/?\\|-_=+ ) bevatten.");

            UpdateErrorMessage(string.Join("\n", issues));
            return issues.Count == 0;
        }
    }
}