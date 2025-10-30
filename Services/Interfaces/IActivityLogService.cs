public interface IActivityLogService
{
    Task LogActivityAsync(string userId, string action, string entityType, int? entityId, string details);
}