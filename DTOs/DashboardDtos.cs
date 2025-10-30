public class DashboardSummaryDto
{
    public int TotalPlants { get; set; }
    public int ActivePlants { get; set; }
    public int PlantsNeedingAttention { get; set; }
    public int UpcomingReminders { get; set; }
    public int UnreadAlerts { get; set; }
    public List<UserPlantDto> RecentPlants { get; set; }
    public List<CropReminderDto> TodayReminders { get; set; }
    public List<WeatherAlertDto> ActiveAlerts { get; set; }
    public LatestSensorDataDto LatestSensorData { get; set; }
}

public class PlantHealthSummaryDto
{
    public int Excellent { get; set; }
    public int Good { get; set; }
    public int Fair { get; set; }
    public int Poor { get; set; }
    public int Critical { get; set; }
}
