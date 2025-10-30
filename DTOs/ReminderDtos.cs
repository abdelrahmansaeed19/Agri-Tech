using System.ComponentModel.DataAnnotations;

public class CreateCropReminderDto
{
    public int? UserPlantId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    [Required, MaxLength(50)]
    public string ReminderType { get; set; }

    [Required]
    public DateTime ReminderDate { get; set; }

    public bool IsRecurring { get; set; }
    public int? RecurringIntervalDays { get; set; }

    [MaxLength(20)]
    public string Priority { get; set; }
}

public class UpdateCropReminderDto
{
    [MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    public DateTime? ReminderDate { get; set; }

    [MaxLength(20)]
    public string Priority { get; set; }

    public bool? IsCompleted { get; set; }
}

public class CropReminderDto
{
    public int Id { get; set; }
    public int? UserPlantId { get; set; }
    public string PlantName { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ReminderType { get; set; }
    public DateTime ReminderDate { get; set; }
    public bool IsRecurring { get; set; }
    public int? RecurringIntervalDays { get; set; }
    public string Priority { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOverdue { get; set; }
    public int DaysUntilDue { get; set; }
}