public interface ICropReminderRepository : IRepository<CropReminder>
{
    Task<IEnumerable<CropReminder>> GetUpcomingRemindersAsync(string userId, int days = 7);
    Task<IEnumerable<CropReminder>> GetOverdueRemindersAsync(string userId);
    Task<IEnumerable<CropReminder>> GetTodayRemindersAsync(string userId);
}