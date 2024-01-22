namespace Learning.Domain.Master;

public class LookupValue : DomainBase
{
    public int Id { get; set; }
    public string? InternalName { get; set; }
    public string DisplayValue { get; set; }
    public bool IsActive { get; set; }
    public int LookupMasterId { get; set; }
    public int Order { get; set; }

    public LookupMaster LookupMaster { get; set; }
}
