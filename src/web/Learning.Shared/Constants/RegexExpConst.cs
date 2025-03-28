﻿using System.Diagnostics.CodeAnalysis;

namespace Learning.Shared.Constants;

public static class RegexExpConst
{
    #region General

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralName = "^[A-Za-z ,]+$";
    public const string GeneralNameMessage = "Invalid input. Only alphabets allowed.";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralDescription = "^[A-Za-z 0-9.,\n']+$";
    public const string GeneralDescriptionMessage = "Please enter a value containing only letters, numbers, comma, newline, apostrophe and dots.";
    
    public const string GeneralInValidUrlMessage = "Please enter a valid URL";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralEmailAddress = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string GeneralEmailAddressMessage = "Please enter a valid email address";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralPlace = "^[A-Za-z 0-9']+$";
    public const string GeneralPlaceMessage = "Please only alpha numeric values";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralPhoneNumber = "^(?:\\+91)?[6-9]\\d{9}$";
    public const string GeneralPhoneNumberMessage = "Please enter valid mobile number";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralPassword = "(?=.*[A-Za-z])(?=.*\\d)(?=.*[!@#$%^&*()-+=])[A-Za-z\\d!@#$%^&*()-+=]{8,}$";
    public const string GeneralPasswordMessage = "Please enter a valid password";
    public const string GeneralPasswordPolicyMessage = "Password should contain at least 1 character, 1 number and 1 special character.";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string GeneralOtp = "^[0-9]{6}$";
    public const string GeneralOtpMessage = "OTP should be 6 digit number";

    #endregion General


    #region Master
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string LookupValue = "^[A-Za-z 0-9]+$";
    public const string LookupValueMessage = "Please enter a value containing only letters, space and numbers.";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string LookupValueCode = "^[a-z-]+$";
    public const string LookupValueCodeMessage = "Please enter a value containing only lowercase letters and hyphens.";
    #endregion

    #region Courses
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string CourseCode = "^[a-z-]+$";
    public const string CourseCodeMessage = "Please enter a value containing only lowercase letters and hyphens.";
    
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string CourseName = "^[A-Za-z ]+$";
    public const string CourseNameMessage = "Please enter a value containing only letters and spaces.";
    #endregion

    #region Classes
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string ClassCode = "^[a-z0-9-]+$";
    public const string ClassCodeMessage = "Please enter a value containing only lowercase letters, numbers, and hyphens.";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string ClassName = "^[A-Za-z0-9 ]+$";
    public const string ClassNameMessage = "Please enter a value containing only letters (uppercase or lowercase), numbers, and spaces.";
    #endregion

    #region Subject
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string SubjectCode = "^[a-z0-9-]+$";
    public const string SubjectCodeMessage = "Please enter a value containing only lowercase letters, numbers, and hyphens.";

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string SubjectName = "^[A-Za-z0-9 ]+$";
    public const string SubjectNameMessage = "Please enter a value containing only letters (uppercase or lowercase), numbers, and spaces.";
    #endregion
    
    #region Lesson
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string ChapterName = "^[a-zA-Z0-9- ]+$";
    public const string ChapterNameMessage = "Please enter a value containing only lowercase letters, numbers, spaces, and hyphens.";
    
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string LessonName = "^[a-zA-Z0-9- ]+$";
    public const string LessonNameMessage = "Please enter a value containing only lowercase letters, numbers, spaces, and hyphens.";
    
    [StringSyntax(StringSyntaxAttribute.Regex)] public const string LessonCode = "^[a-z-]+$";
    public const string LessonCodeMessage = "Please enter a value containing only lowercase letters and hyphens.";

    #endregion

    #region Notifications

    [StringSyntax(StringSyntaxAttribute.Regex)] public const string ExamNotificationTitle = "^[A-Za-z 0-9]+$";
    public const string ExamNotificationTitleMessage = "Please enter a value containing only letters, space and numbers.";
    
    public const string ExamNotificationImagePathMessage = "The image is not a valid url";

    public const string GovtLinkUrl = "Please enter a value containing only letters, space and numbers.";

    #endregion
}
