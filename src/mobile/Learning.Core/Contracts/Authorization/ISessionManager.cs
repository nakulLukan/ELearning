using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Core.Contracts.Authorization;

public interface ISessionManager
{
    public Task<string> Login();
}
