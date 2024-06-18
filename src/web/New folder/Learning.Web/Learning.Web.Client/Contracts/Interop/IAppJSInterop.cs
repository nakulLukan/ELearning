namespace Learning.Web.Client.Contracts.Interop;

public interface IAppJSInterop
{
    public Task OpenDocumentInNewTab(string url);
}
