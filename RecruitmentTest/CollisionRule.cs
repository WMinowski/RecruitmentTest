using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace RecruitmentTest
{
    class CollisionRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((value as BindingGroup).Items.Count == 0)
                return ValidationResult.ValidResult;
            CustomerVM customer = ((value as BindingGroup).Items[0]) as CustomerVM;
            if (customer.Customer.HasCollisions == true)
            {
                return new ValidationResult(false,
                  "This object has unresolved collisions. Please, fix this issue!");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}
