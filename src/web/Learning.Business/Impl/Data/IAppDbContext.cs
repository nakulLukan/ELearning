using Learning.Domain.Content;
using Learning.Domain.Core;
using Learning.Domain.Identity;
using Learning.Domain.Master;
using Learning.Domain.Notification;
using Learning.Domain.Quiz;
using Learning.Domain.Subscription;
using Learning.Domain.Subscription.Offer;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Learning.Business.Impl.Data;

public interface IAppDbContext
{
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<ApplicationUserOtherDetail> ApplicationUserOtherDetails { get; set; }
    public DbSet<ContactUsRequest> ContactUsRequests { get; set; }

    public DbSet<LookupMaster> LookupMasters { get; set; }
    public DbSet<LookupValue> LookupValues { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<ClassDivision> Classes { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Video> Videos { get; set; }

    #region Subscriptions
    public DbSet<SubjectSubscriptionDetail> SubjectSubscriptionDetails { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }
    public DbSet<CouponCode> CouponCodes { get; set; }
    #endregion

    #region Quiz
    public DbSet<QuizConfiguration> QuizConfigurations { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<QuizQuestionAnswer> QuizQuestionAnswers { get; set; }
    #endregion

    #region ExamNotifications
    public DbSet<ExamNotification> ExamNotifications { get; set; }
    public DbSet<ModelExamConfiguration> ModelExamConfigurations { get; set; }
    public DbSet<ModelExamPackage> ModelExamPackages { get; set; }
    public DbSet<ModelExamQuestionConfiguration> ModelExamQuestionConfigurations { get; set; }
    public DbSet<ModelExamAnswerConfiguration> ModelExamAnswerConfigurations { get; set; }
    public DbSet<ModelExamResult> ModelExamResults { get; set; }
    public DbSet<ModelExamResultDetail> ModelExamResultDetails { get; set; }
    public DbSet<ModelExamPurchaseHistory> ModelExamPurchaseHistory { get; set; }
    #endregion

    public void ClearChanges();

    public void Set<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property, TProperty value);

    Task<int> SaveAsync(CancellationToken cancellationToken);
}
