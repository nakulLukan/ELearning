using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Core.PageModels.Startup;

public partial class SplashPageModel : PageModelBase
{
    public SplashPageModel(ILogger<PageModelBase> logger) : base(logger)
    {
    }
}
