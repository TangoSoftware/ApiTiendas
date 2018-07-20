using System.ComponentModel.DataAnnotations;
using static nexoTiendas.ApertureDto.Validators.ValidatorsHelper;

namespace nexoTiendas.ApertureDto.Validators
{
    public class MinValueStrictAttribute : ValidationAttribute
    {
        private readonly double MinValue;

        public MinValueStrictAttribute(double minValue, string errorMessage)
        {
            MinValue = minValue;
            ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return GetDecimal(value) > (decimal)MinValue;
            }
            else
            {
                return true;
            }
        }
    }
}
