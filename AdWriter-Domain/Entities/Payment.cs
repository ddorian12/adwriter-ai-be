using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int GenerationPackId { get; set; }

        public string StripeSessionId { get; set; } = string.Empty;
        public bool IsPaid { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        public User User { get; set; }
        public GenerationPack Pack { get; set; }
    }
}
