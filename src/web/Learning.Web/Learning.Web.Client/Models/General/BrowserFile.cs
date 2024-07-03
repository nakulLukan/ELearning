using Microsoft.AspNetCore.Components.Forms;

namespace Learning.Web.Client.Models.General;

public class BrowserFile
{
    public string Name { get; private set; }
    public long Size { get; private set; }

    private IBrowserFile? _file;
    public IBrowserFile? File
    {
        get => _file; set
        {

            _file = value;
            Name = _file?.Name ?? string.Empty;
            Size = _file?.Size ?? 0;
        }
    }

    public BrowserFile(string name, long size)
    {
        Name = name;
        Size = size;
    }

    public BrowserFile()
    {
        Name = string.Empty;
    }
}
