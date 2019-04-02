using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.WebPages;

public abstract class ValidatorDynamicErrorBase : RequestFieldValidatorBase {
    public ValidatorDynamicErrorBase(string error) : base(error) {}

    protected void UpdateErrorMessage(string msg) {
        // There is no real reason why the error message can't be changed after object construction,
        // other than that RequestFieldValidatorBase doesn't have a method to do so.
        FieldInfo field = typeof(RequestFieldValidatorBase).GetField("_errorMessage", BindingFlags.Instance | BindingFlags.NonPublic);
        field.SetValue(this, msg);
    }
}