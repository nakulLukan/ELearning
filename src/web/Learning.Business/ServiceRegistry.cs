using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Business.Services.Core;
using Learning.Business.Services.ExamNotification;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Business;

public static class ServiceRegistry
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<SubjectManager>();
        services.AddScoped<IExamNotificationManager, ExamNotificationManager>();
    }
}
