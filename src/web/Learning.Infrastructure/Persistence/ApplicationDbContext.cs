
using Learning.Business.Impl.Data;
using Learning.Domain.Content;
using Learning.Domain.Core;
using Learning.Domain.Identity;
using Learning.Domain.Master;
using Learning.Domain.Notification;
using Learning.Domain.Subscription;
using Learning.Infrastructure.Data.Seeder.MigrationSeeder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
namespace Learning.Infrasture.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(), t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
            )
        );

        modelBuilder.SeedData();
    }

    public Task<int> SaveAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }
    public void ClearChanges()
    {
        this.ChangeTracker.Clear();
    }

    public void Set<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property, TProperty value)
    {
        if (property.Body is MemberExpression memberExpression)
        {
            string propertyName = memberExpression.Member.Name;
            entity.GetType().GetProperty(propertyName).SetValue(entity, value, null);
            Entry(entity).Property(propertyName).IsModified = true;
        }
    }

    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<ApplicationUserOtherDetail> ApplicationUserOtherDetails { get; set; }
    public DbSet<ContactUsRequest> ContactUsRequests { get; set; }

    public DbSet<LookupMaster> LookupMasters { get; set; }
    public DbSet<LookupValue> LookupValues { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<ClassDivision> Classes { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public DbSet<Video> Videos { get; set; }

    public DbSet<SubjectSubscriptionDetail> SubjectSubscriptionDetails { get; set; }
    public DbSet<UserSubscription> UserSubscriptions { get; set; }

    public DbSet<ExamNotification> ExamNotifications { get; set; }
}
