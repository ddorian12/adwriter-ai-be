using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Domain.Entities
{
    public class GenerationPack
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Generations { get; set; }
        public decimal PriceRon { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

}
