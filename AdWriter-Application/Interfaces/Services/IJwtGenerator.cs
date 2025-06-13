using AdWriter_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Application.Interfaces.Services
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}
