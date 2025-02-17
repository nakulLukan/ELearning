namespace Learning.Domain.Identity;

public class ContactUsRequest : DomainBase
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string CountryCode { get; set; }
   
    /// <summary>
    /// Without country code
    /// </summary>
    public required string PhoneNumber { get; set; }
    public required string City { get; set; }
    public string? EmailAddress { get; set; }
}
