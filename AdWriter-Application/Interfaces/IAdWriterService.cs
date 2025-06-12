using AdWriter_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Application.Interfaces
{
    public interface IAdWriterService
    {
      Task<AdContentResponse> GenerateAdContentAsync2(string description, string language);
      Task<string> GenerateAdContentAsync(string description, string language);
    }
}
