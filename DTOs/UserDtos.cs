using AgriculturalTech.API.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace AgriculturalTech.API.DTOs
{
    public class SubscriptionDto
    {
        public bool Status { get; set; }
        public DateTime CurrentPeriodStart { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
        public bool IsCancelAtPeriodEnd { get; set; }
    }

    public class SubscriptionRequestDto
    {
        [Required]
        public SubscriptionPlanType PlanType { get; set; }
    }

    public class UpdatePreferredLanguageDto
    {
        [Required]
        public string PreferredLanguage { get; set; }
    }
}
