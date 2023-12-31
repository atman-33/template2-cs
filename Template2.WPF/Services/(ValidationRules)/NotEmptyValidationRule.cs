using System.Globalization;
using System.Windows.Controls;

namespace Template2.WPF.Services
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "入力必須です。") : ValidationResult.ValidResult;
        }
    }
}
