using nexoTiendas.ApertureDto.Validators;
using System.ComponentModel.DataAnnotations;

namespace nexoTiendas.ApertureDto
{
    public class CashPaymentDto
    {
        #region Constructor
        public CashPaymentDto()
        {
            PaymentID = 0;
            PaymentMethod = "";
            PaymentTotal = 0;
        }
        #endregion

        [Required]
        [MinValueStrict(0, "{0} must be greater than zero")]
        public long PaymentID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string PaymentMethod { get; set; }

        [Required]
        [MinValueStrict(0, "{0} must be greater than zero")]
        public decimal PaymentTotal { get; set; }
    }
}
