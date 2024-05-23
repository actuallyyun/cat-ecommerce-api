namespace Ecommerce.Service.src.DTO
{
    public class AddressCreateDto
    {
        public string FirstName{get;set;}
        public string LastName{get;set;}
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }

    }

    public class AddressUpdateDto
    {
         public string? FirstName{get;set;}
        public string? LastName{get;set;}
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class AddressReadDto
    {
        public Guid Id {get;set;}
        public Guid UserId { get; set;}
         public string FirstName{get;set;}
        public string LastName{get;set;}
        public string AddressLine { get;set; }
        public string PostalCode { get; set;}
        public string Country { get;set; }
        public string PhoneNumber { get;set; }
    }
}
