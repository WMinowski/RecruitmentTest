using System.Globalization;
using System.Windows.Controls;

namespace RecruitmentTest
{
    class NoEmptyRule : ValidationRule
    {
        public NoEmptyRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (((string)value).Length==0)
            {
                return new ValidationResult(false,
                  "Empty field!");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
