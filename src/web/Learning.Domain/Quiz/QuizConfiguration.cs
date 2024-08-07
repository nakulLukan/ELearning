﻿namespace Learning.Domain.Quiz;

public class QuizConfiguration : DomainBase
{
    public required int Id { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsDefault { get; set; }
    public required float PassPercentage { get; set; }

    /// <summary>
    /// Version number can be used to update a quiz. Any user who has attended a quiz with different version
    /// can reappear this quiz.
    /// </summary>
    public required int VersionNumber { get; set; }

    /// <summary>
    /// Discount offered when scored 100%. Based on the score which must be greater than <see cref="PassPercentage"/>, this value reduces from the given value.
    /// </summary>
    public required int DiscountPercentage { get; set; }

    public List<QuizQuestion>? Questions { get; set; }
}
