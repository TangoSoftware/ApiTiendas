using nexoTiendas.ApertureDto.Validators;
using System.ComponentModel.DataAnnotations;

namespace nexoTiendas.ApertureDto
{
    public class ShippingDto
    {
        #region Constructor
        public ShippingDto()
        {
            ShippingID = 0;
            Street = "";
            HouseNumber = "";
            Floor = "";
            Apartment = "";
            City = "";
            ProvinceCode = "";
            PostalCode = "";
            PhoneNumber1 = "";
            PhoneNumber2 = "";
            DeliversMonday = "";
            DeliversTuesday = "";
            DeliversWednesday = "";
            DeliversThursday = "";
            DeliversFriday = "";
            DeliversSaturday = "";
            DeliversSunday = "";
            DeliveryHours = "";
        }
        #endregion

        [Required]
        [MinValueStrict(0, "{0} must be greater than zero")]
        public long ShippingID { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string Floor { get; set; }

        public string Apartment { get; set; }

        public string City { get; set; }

        public string ProvinceCode { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public decimal ShippingCost { get; set; }

        public string DeliversMonday { get; set; }

        public string DeliversTuesday { get; set; }

        public string DeliversWednesday { get; set; }

        public string DeliversThursday { get; set; }

        public string DeliversFriday { get; set; }

        public string DeliversSaturday { get; set; }

        public string DeliversSunday { get; set; }

        public string DeliveryHours { get; set; }
    }
}
