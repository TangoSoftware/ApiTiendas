using nexoTiendas.ApertureDto.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nexoTiendas.ApertureDto
{
    public class OrderDto
    {
        #region Constructor
        public OrderDto()
        {
            OrderID = "";
            OrderNumber = "";
        }
        #endregion

        [Required]
        public DateTime Date { get; set; }

        [MinValueStrict(0, "{0} must be greater than zero")]
        [Required]
        public decimal Total { get; set; }

        [MinValue(0, "{0} must be greater than or equal to zero")]
        public decimal TotalDiscount { get; set; }

        [MinValue(0, "{0} must be greater than or equal to zero")]
        public decimal PaidTotal { get; set; }

        [MinValue(0, "{0} must be greater than or equal to zero")]
        public decimal FinancialSurcharge { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string OrderID { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string OrderNumber { get; set; }

        [Required]
        public CustomerDto Customer { get; set; }

        public bool CancelOrder { get; set; }

        [Required]
        public IList<OrderItemDto> OrderItems { get; set; }

        public ShippingDto Shipping { get; set; }

        public CashPaymentDto CashPayment { get; set; }

        public IList<PaymentDto> Payments { get; set; }
    }
}
