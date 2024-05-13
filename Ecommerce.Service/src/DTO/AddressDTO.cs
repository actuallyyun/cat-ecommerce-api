namespace Ecommerce.Service.src.DTO
{
    public class AddressCreateDto
    {
        public Guid UserId { get; set; }
        public string AddressLine { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AddressUpdateDto
    {
        public string? AddressLine { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class AddressReadDto
    {
        public Guid Id {get;}
        public Guid UserId { get; }
        public string AddressLine { get; }
        public string PostalCode { get; }
        public string Country { get; }
        public string PhoneNumber { get; }
    }
}
