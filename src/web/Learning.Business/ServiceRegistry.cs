using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Business.Contracts.Services.Master;
using Learning.Business.Services.Core;
using Learning.Business.Services.ExamNotification;
using Learning.Business.Services.Master;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Business;

public static class ServiceRegistry
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<SubjectManager>();
        services.AddScoped<IExamNotificationManager, ExamNotificationManager>();
        services.AddScoped<IAppMasterSettingManager, AppMasterSettingManager>();
    }
}
