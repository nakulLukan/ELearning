namespace Functions.Identity.Core.Contracts.ManageUsers;

public interface IIdentityUserManager
{
    public Task<string> AddUser(IDictionary<string, string> userAttributes);
}
