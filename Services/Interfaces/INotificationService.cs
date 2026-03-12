namespace AgriculturalTech.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string fcmToken, string title, string body);
        Task SendReminderNotificationAsync(string userId, CropReminder reminder);
        Task SendAlertNotificationAsync(string userId, string title, string message);
        Task<List<Notification>> GetUnreadNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
    }
}