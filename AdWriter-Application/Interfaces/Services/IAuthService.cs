using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
