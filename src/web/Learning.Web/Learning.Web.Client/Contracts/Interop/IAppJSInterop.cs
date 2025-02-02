namespace Learning.Web.Client.Contracts.Interop;

public interface IAppJSInterop
{
	public Task OpenDocumentInNewTab(string url);
	public Task CopyTextToClipboard(string text);
	public Task ScrollToTopOnNavigation();
	public Task ScrollToBottomOnNavigation();
	public Task ScrollToTarget(string targetId);
	public Task<bool> IsMobile();
	public Task InitRazorpayPopup(string appName, string rzrpayOrderId, string? userName, string? userEmail, string phoneNumber, string internalOrderId);
}
