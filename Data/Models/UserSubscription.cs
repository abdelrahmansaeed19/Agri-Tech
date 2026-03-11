using AgriculturalTech.API.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriculturalTech.API.Data.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }

        // Foreign Key to your ApplicationUser (Identity)
        public string UserId { get; set; }

        // Stripe Identifiers
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }

        public enSubscriptionStatus SubscriptionStatus { get; set; }

        public DateTime CurrentPeriodStart { get; set; }

        public DateTime CurrentPeriodEnd { get; set; }

        // True if the user canceled, but still has access until the end of the month
        public bool CancelAtPeriodEnd { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
