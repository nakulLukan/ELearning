using Learning.Web.Client.Contracts.Interop;
using Microsoft.JSInterop;

namespace Learning.Web.Client.Impl.Interop;

public class AppJSInterop : IAppJSInterop
{
	private readonly IJSRuntime _jsRuntime;

	public AppJSInterop(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	public async Task CopyTextToClipboard(string text)
	{
		await _jsRuntime.InvokeVoidAsync("copyToClipboard", text);
	}

	public async Task<bool> IsMobile()
	{
		return await _jsRuntime.InvokeAsync<bool>("isMobile");
	}

	public Task OpenDocumentInNewTab(string url)
	{
		throw new NotImplementedException();
	}

	public async Task ScrollToTopOnNavigation()
	{
		await _jsRuntime.InvokeVoidAsync("scrollToTopOnNavigation");
	}

	public async Task ScrollToBottomOnNavigation()
	{
		await _jsRuntime.InvokeVoidAsync("scrollToBottomOnNavigation");
	}
	public async Task ScrollToTarget(string targetId)
	{
		await _jsRuntime.InvokeVoidAsync("scrollToTarget", targetId);
	}

	public async Task InitRazorpayPopup(string appName, string rzrpayOrderId, string? userName, string? userEmail, string phoneNumber, string internalOrderId)
	{
		await _jsRuntime.InvokeVoidAsync("initRazorpayPopup", appName, rzrpayOrderId, userName, userEmail, phoneNumber, internalOrderId);
	}

}
