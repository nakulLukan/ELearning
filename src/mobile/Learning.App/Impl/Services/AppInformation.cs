

using Learning.Core.Contracts.Services;
using Learning.Core.Enums;

namespace Learning.App.Impl.Services;

public class AppInformation : IAppInformation
{
    private readonly IAppInfo _appInfo;

    public AppInformation(IAppInfo appInfo)
    {
        _appInfo = appInfo;
    }

    public string GetModel()
    {
        return DeviceInfo.Current.Model;
    }

    public DevicePlatformEnum GetPlatform()
    {
        var devicePlatform = DeviceInfo.Current.Platform;
        if (devicePlatform == DevicePlatform.Android)
        {
            return DevicePlatformEnum.Android;
        }
        else if (devicePlatform == DevicePlatform.iOS)
        {
            return DevicePlatformEnum.IOS;
        }
        else if (devicePlatform == DevicePlatform.WinUI)
        {
            return DevicePlatformEnum.WinUI;
        }
        else if (devicePlatform == DevicePlatform.MacCatalyst)
        {
            return DevicePlatformEnum.Mac;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public DeviceIdiomEnum GetDeviceIdiom()
    {
        var devicePlatform = DeviceInfo.Idiom;
        if (devicePlatform == DeviceIdiom.Phone)
        {
            return DeviceIdiomEnum.Phone;
        }
        else if (devicePlatform == DeviceIdiom.Tablet)
        {
            return DeviceIdiomEnum.Tablet;
        }
        else if (devicePlatform == DeviceIdiom.Desktop)
        {
            return DeviceIdiomEnum.Desktop;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public string GetPlatformString()
    {
        return DeviceInfo.Current.Platform.ToString();
    }
}
