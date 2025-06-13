using AdWriter_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Application.Interfaces.Services
{
    public interface IAdWriterService
    {
        Task<string> GenerateAdContentAsync(string description, string language);
    }
}
