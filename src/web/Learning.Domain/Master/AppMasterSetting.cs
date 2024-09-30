namespace Learning.Domain.Master;

public class AppMasterSetting : DomainBase
{
    public string Id { get; set; } = string.Empty;
    public string? Value { get; set; }
    public bool IsJsonValue { get; set; }
}
