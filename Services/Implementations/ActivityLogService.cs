public class ActivityLogService : IActivityLogService
{
    private readonly IUnitOfWork _unitOfWork;

    public ActivityLogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task LogActivityAsync(string userId, string action, string entityType, int? entityId, string details)
    {
        var log = new ActivityLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Details = details
        };

        await _unitOfWork.ActivityLogs.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();
    }
}
