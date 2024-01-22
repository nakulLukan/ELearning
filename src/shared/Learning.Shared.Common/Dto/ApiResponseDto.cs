namespace Learning.Shared.Common.Dto;

/// <summary>
/// API common response structure for scalar types
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponseDto<T>
{
    /// <summary>
    /// Response Value
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// API common response structure for scalar types
    /// </summary>
    /// <param name="data"></param>
    public ApiResponseDto(T data)
    {
        Data = data;
    }
}
