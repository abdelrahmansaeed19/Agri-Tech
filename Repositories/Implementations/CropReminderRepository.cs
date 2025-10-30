using AgriculturalTech.API.Data;
using Microsoft.EntityFrameworkCore;

public class CropReminderRepository : Repository<CropReminder>, ICropReminderRepository
{
    public CropReminderRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<CropReminder>> GetUpcomingRemindersAsync(string userId, int days = 7)
    {
        var today = DateTime.UtcNow.Date;
        var endDate = today.AddDays(days);

        return await _dbSet
            .Include(r => r.UserPlant)
                .ThenInclude(p => p.CropType)
            .Where(r => r.UserId == userId
                && r.IsActive
                && !r.IsCompleted
                && r.ReminderDate >= today
                && r.ReminderDate <= endDate)
            .OrderBy(r => r.ReminderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CropReminder>> GetOverdueRemindersAsync(string userId)
    {
        var today = DateTime.UtcNow.Date;

        return await _dbSet
            .Include(r => r.UserPlant)
                .ThenInclude(p => p.CropType)
            .Where(r => r.UserId == userId
                && r.IsActive
                && !r.IsCompleted
                && r.ReminderDate < today)
            .OrderBy(r => r.ReminderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<CropReminder>> GetTodayRemindersAsync(string userId)
    {
        var today = DateTime.UtcNow.Date;

        return await _dbSet
            .Include(r => r.UserPlant)
                .ThenInclude(p => p.CropType)
            .Where(r => r.UserId == userId
                && r.IsActive
                && !r.IsCompleted
                && r.ReminderDate.Date == today)
            .OrderBy(r => r.Priority)
            .ToListAsync();
    }
}