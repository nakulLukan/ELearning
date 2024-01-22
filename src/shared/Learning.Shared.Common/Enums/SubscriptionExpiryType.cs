namespace Learning.Shared.Common.Enums;

public enum SubscriptionExpiryType
{
    /// <summary>
    /// Expires every year at a particular date
    /// </summary>
    Yearly = 1,

    /// <summary>
    /// Expires after x days of subscription
    /// </summary>
    RelativeExpiry = 2,

    /// <summary>
    /// Expires exactly on defined date.
    /// </summary>
    AbsoluteExpiry = 3,

    /// <summary>
    /// Never expires
    /// </summary>
    Never = 4
}
