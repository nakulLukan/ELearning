namespace Learning.Shared.Dto.DataCollection.ContactUsRequest;

public class GetAllContactUsQueryResponseDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required string ContactNumber { get; set; }
    public required string City { get; set; }
    public required string? EmailAddress { get; set; }
    public required DateTimeOffset RequestedOn { get; set; }
}
