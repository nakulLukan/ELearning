using Learning.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Learning.Business.Dto.Master.Lookup;

public class AddLookupValueDto
{
    [Required]
    [RegularExpression(RegexExpConst.LookupValue, ErrorMessage = RegexExpConst.LookupValueMessage)]
    [MaxLength(50)]
    public string DisplayValue { get; set; }

    [RegularExpression(RegexExpConst.LookupValueCode, ErrorMessage = RegexExpConst.LookupValueCodeMessage)]
    [MaxLength(10)]
    public string? InternalName { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public int? LookupMasterId { get; set; }
}
