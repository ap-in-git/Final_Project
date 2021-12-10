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
            decimal number;
            if (!decimal.TryParse((string)value, out number))
            {
                return new ValidationResult(false, "Please input numeric only");
            }
                
            
            if (value.ToString().Length != 16)
            {
                return new ValidationResult(false, "Please input your 16 digit Credit Card Number");
            }

            return ValidationResult.ValidResult;
        }
    }
}

