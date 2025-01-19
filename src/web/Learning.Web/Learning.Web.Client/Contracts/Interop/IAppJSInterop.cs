namespace Learning.Web.Client.Contracts.Interop;

public interface IAppJSInterop
{
    public Task OpenDocumentInNewTab(string url);
    public Task CopyTextToClipboard(string text);
    public Task ScrollToTopOnNavigation();
    public Task ScrollToBottomOnNavigation();
    public Task<bool> IsMobile();
}
