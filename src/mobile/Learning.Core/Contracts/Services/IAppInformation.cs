
using Learning.Core.Enums;

namespace Learning.Core.Contracts.Services;

public interface IAppInformation
{
    string GetModel();
    string GetPlatformString();
    DevicePlatformEnum GetPlatform();
    DeviceIdiomEnum GetDeviceIdiom();
}
