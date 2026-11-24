namespace CalendarManagement.Domain.Entities
{
    public class Location
    {

        public const int NameMaxLength = 200;
        public const int CityMaxLength = 100;
        public const int AddressMaxLength = 300;


        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
