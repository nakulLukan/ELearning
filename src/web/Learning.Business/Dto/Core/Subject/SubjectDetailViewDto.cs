namespace Learning.Business.Dto.Core.Subject;

public class SubjectDetailViewDto
{
    public int SubjectId { get; set; }
    public string SubjectCode { get; set; }
    public string SubjectName { get; set; }
    public string ClassName { get; set; }
    public string CourseName { get; set; }

    public float DiscountedPrice { get; set; }
    public float Price { get; set; }
    public string ThumbnailUrl { get; set; }
    public string SubjectDescription { get; set; }
    public string CreatedOn { get; set; }

    public string Validity { get; set; }

    public bool IsInvalidSubscription { get; set; }
    public string InvalidSubscriptionMessage { get; set; }

    public string TotalDuration { get; set; }

    /// <summary>
    /// Date when this subject details was last updated.
    /// </summary>
    public string LastUpdatedOn { get; set; }

    /// <summary>
    /// Flag to indicate If a logged in user has purchased the course.
    /// </summary>
    public bool HasPurchased { get; set; }
}
