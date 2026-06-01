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
}
