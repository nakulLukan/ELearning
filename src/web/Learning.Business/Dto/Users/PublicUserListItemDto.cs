namespace Learning.Business.Dto.Users;

public class PublicUserListItemDto
{
    public string Id { get; set; }
    public long Index { get; set; }
    public string FullName { get; set; }
    public string ContactNumber { get; set; }
    public bool IsContactNumberVerified { get; set; }
    public string EmailAddress { get; set; }
    public bool IsEmailAddressVerified { get; set; }
    public bool IsActive { get; set; }
    public string AccountCreatedOn { get; set; }
}
