using Learning.Domain.Content;
using Learning.Domain.Core;
using Learning.Domain.Identity;
using Learning.Domain.Master;
using Learning.Domain.Notification;
using Learning.Domain.Subscription;
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

    public DbSet<SubjectSubscriptionDetail> SubjectSubscriptionDetails { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }

    public DbSet<ExamNotification> ExamNotifications { get; set; }

    public void ClearChanges();

    public void Set<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property, TProperty value);

    Task<int> SaveAsync(CancellationToken cancellationToken);
}
