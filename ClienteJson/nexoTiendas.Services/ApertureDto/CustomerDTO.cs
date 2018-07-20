using nexoTiendas.ApertureDto.Validators;
using System.ComponentModel.DataAnnotations;

namespace nexoTiendas.ApertureDto
{
    public class CustomerDto
    {
        #region Constructor
        public CustomerDto()
        {
            CustomerID = 0;
            DocumentType = "";
            DocumentNumber = "";
            IVACategoryCode = "";
            User = "";
            Email = "";
            FirstName = "";
            LastName = "";
            BusinessName = "";
            Street = "";
            HouseNumber = "";
            Floor = "";
            Apartment = "";
            City = "";
            ProvinceCode = "";
            PostalCode = "";
            PhoneNumber1 = "";
            PhoneNumber2 = "";
            MobilePhoneNumber = "";
            BusinessAddress = "";
            Comments = "";
        }
        #endregion

        [Required]
        [MinValueStrict(0, "{0} must be greater than zero")]
        public long CustomerID { get; set; }

        public string DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string IVACategoryCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string User { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BusinessName { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string Floor { get; set; }

        public string Apartment { get; set; }

        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ProvinceCode { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string BusinessAddress { get; set; }

        public string Comments { get; set; }
    }
}
