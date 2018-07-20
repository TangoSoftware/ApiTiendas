using System.ComponentModel.DataAnnotations;
using static nexoTiendas.ApertureDto.Validators.ValidatorsHelper;

namespace nexoTiendas.ApertureDto.Validators
{
    public class MaxValueAttribute : ValidationAttribute
    {
        private readonly double MaxValue;

        public MaxValueAttribute(double maxValue, string errorMessage)
        {
            MaxValue = maxValue;
            ErrorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return GetDecimal(value) <= (decimal)MaxValue;
            }
            else
            {
                return true;
            }
        }
    }
}
