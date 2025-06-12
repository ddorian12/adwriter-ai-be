using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Domain.Models
{
    public class AdRequest
    {
        public string Description { get; set; } = string.Empty;
        public string Language { get; set; } = "ro";
    }
}
