using System.ComponentModel.DataAnnotations;
using Learning.Shared.Constants;

namespace Learning.Web.Client.Dto.ContactUs;

public class ContactUsModel
{
    [Required(ErrorMessage = "'Name' is required.")]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "'Contact Number' is required.")]
    [Phone]
    [MaxLength(10, ErrorMessage = "'Contact Number' should be exactly number of length 10.")]
    [MinLength(10, ErrorMessage = "'Contact Number' should be exactly number of length 10.")]
    public string ContactNumber { get; set; } = string.Empty;


    [Required(ErrorMessage = "'City' is required.")]
    [RegularExpression(RegexExpConst.GeneralPlace, ErrorMessage = RegexExpConst.GeneralPlaceMessage)]
    [MaxLength(50, ErrorMessage = "'City' cannot be greater than 50 characters")]
    public string City { get; set; } = string.Empty;

    [RegularExpression(RegexExpConst.GeneralEmailAddress, ErrorMessage = RegexExpConst.GeneralEmailAddressMessage)]
    [MaxLength(200)]
    public string? EmailAddress { get; set; }
}