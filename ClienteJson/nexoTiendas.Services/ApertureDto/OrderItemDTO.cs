using nexoTiendas.ApertureDto.Validators;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace nexoTiendas.ApertureDto
{
    public class OrderItemDto
    {
        [Required]
        [DisplayName("ProductCode*")]
        public string ProductCode { get; set; }

        public string SKUCode { get; set; }

        public string VariantCode { get; set; }

        [Required]
        [DisplayName("Description*")]
        public string Description { get; set; }

        public string VariantDescription { get; set; }

        [MinValueStrict(0, "{0} must be greater than zero")]
        [Required]
        [DisplayName("Quantity*")]
        public decimal Quantity { get; set; }

        [MinValueStrict(0, "{0} must be greater than zero")]
        [Required]
        [DisplayName("UnitPrice*")]
        public decimal UnitPrice { get; set; }

        [MinValue(0, "{0} must be greater than or equal to zero")]
        [MaxValueStrict(100, "{0} must be less than 100")]
        public decimal DiscountPercentage { get; set; }
    }
}