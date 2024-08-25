namespace Learning.Shared.Dto.DataCollection.ContactUsRequest;

public class AddContactInformationCommandRequestDto
{
    public required string Name { get; set; }
    public required string CountryCode { get; set; }
    public required string ContactNumber { get; set; }
    public string? EmailAddress { get; set; }
}
