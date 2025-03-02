namespace Learning.Web.Contracts.Authentication;

public interface IUserManager
{
    public Task Login(string username, string password);
    public Task<long> SignUpUser(string username, string password, string name, string address);
    public Task<bool> ConfirmAccount(string username, int otp);
    public Task Logout();
}
