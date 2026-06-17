namespace ECommerceAPI.DTOs.ShippingAddress
{
    public class CreateShippingAddressDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
    }
}
