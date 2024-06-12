using Microsoft.Extensions.Logging;

namespace Learning.Core.PageModels.Startup;

public partial class PlaygroundPageModel : PageModelBase
{
    public PlaygroundPageModel(ILogger<PlaygroundPageModel> logger) : base(logger)
    {
    }
}
