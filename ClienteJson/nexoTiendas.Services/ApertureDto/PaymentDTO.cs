using nexoTiendas.ApertureDto.Validators;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace nexoTiendas.ApertureDto
{
    public class PaymentDto
    {
        #region Constructor
        public PaymentDto()
        {
            PaymentId = 0;
            AuthorizationCode = "";
            TransactionNumber = "";
        }
        #endregion
        [Required]
        [MinValueStrict(0, "{0} must be greater than zero")]
        [DisplayName("PaymentId*")]
        public long PaymentId { get; set; }

        [Required]
        [DisplayName("TransactionDate*")]
        public DateTime? TransactionDate { get; set; }

        public string AuthorizationCode { get; set; }

        public string TransactionNumber { get; set; }

        [MinValueStrict(0, "{0} must be greater than zero")]
        [Required]
        [DisplayName("Installments*")]
        public int Installments { get; set; }

        [MinValueStrict(0, "{0} must be greater than zero")]
        [Required]
        [DisplayName("InstallmentAmount*")]
        public decimal InstallmentAmount { get; set; }

        [MinValueStrict(0, "{0} must be greater than zero")]
        [Required]
        [DisplayName("Total*")]
        public decimal Total { get; set; }

        [Required]
        [DisplayName("CardCode*")]
        public string CardCode { get; set; }

        [Required]
        [DisplayName("CardPlanCode*")]
        public string CardPlanCode { get; set; }

        [Required]
        [DisplayName("VoucherNo*")]
        public long VoucherNo { get; set; }

        public string CardPromotionCode { get; set; }
    }
}