using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RemainingGenerations { get; set; } = 5;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<GeneratedItem> GeneratedItems { get; set; } = new List<GeneratedItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
