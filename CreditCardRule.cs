using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Final_Project
{
    class CreditCardRule : ValidationRule
    {
        //Validate credit card to be numeric and 16 digits only
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = !decimal.TryParse((string)value, out decimal number)
                ? new ValidationResult(false,
                    "Please input numeric only")
                : ((string)value).Length != 16
                    ? new ValidationResult(false,
                        string.Format("Please input your 16 digit Credit Card Number"))
                    : ValidationResult.ValidResult;
            return result;
        }
    }
}

