using Learning.Core.PageModels.AccountManagement;

namespace Learning.App.Pages.AccountManagement;

public partial class LoginPage : ContentPageBase
{
	public LoginPage(LoginPageModel pageModel)
	{
		InitializeComponent();
		BindingContext = pageModel;
	}
}