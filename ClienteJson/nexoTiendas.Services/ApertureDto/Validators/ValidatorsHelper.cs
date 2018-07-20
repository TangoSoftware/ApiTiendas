namespace nexoTiendas.ApertureDto.Validators
{
    public static class ValidatorsHelper
    {
        public static decimal GetDecimal(object value)
        {
            if (value is int)
            {
                return (decimal)((int)value);
            }
            if (value is long)
            {
                return (decimal)((long)value);
            }
            return (decimal)value;
        }
    }
}
