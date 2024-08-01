using Learning.Business.Contracts.Services.ExamNotification;
using Learning.Business.Contracts.Services.Quiz;
using Learning.Business.Services.Core;
using Learning.Business.Services.ExamNotification;
using Learning.Business.Services.Quiz;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Business;

public static class ServiceRegistry
{
    public static void RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<SubjectManager>();
        services.AddScoped<IExamNotificationManager, ExamNotificationManager>();
        services.AddTransient<IQuizManager, QuizManager>();
    }
}
