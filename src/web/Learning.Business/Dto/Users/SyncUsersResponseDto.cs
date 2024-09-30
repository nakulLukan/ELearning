namespace Learning.Business.Dto.Users;

public class SyncUsersResponseDto
{
    public int UpdatedCount { get; set; }
    public int CreatedCount { get; set; }
    public SyncUsersResponseDto(int createdCount, int updatedCount)
    {
        UpdatedCount = updatedCount;
        CreatedCount = createdCount;
    }
}
