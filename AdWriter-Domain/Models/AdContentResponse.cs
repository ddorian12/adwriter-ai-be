using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Domain.Models
{
  public class AdContentResponse
  {
    public List<string> Titles { get; set; } = new();
    public List<string> Descriptions { get; set; } = new();
    public List<string> Hashtags { get; set; } = new();
  }
}
