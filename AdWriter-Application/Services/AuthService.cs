using AdWriter_Application.Interfaces.Repositories;
using AdWriter_Application.Interfaces.Services;
using AdWriter_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtGenerator _jwt;

        public AuthService(IUserRepository userRepo, IJwtGenerator jwt)
        {
            _userRepo = userRepo;
            _jwt = jwt;
        }

        public async Task<string> RegisterAsync(string email, string password)
        {
            if (await _userRepo.GetByEmailAsync(email) != null)
                throw new Exception("Email already registered.");

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Email = email,
                PasswordHash = hash,
                RemainingGenerations = 5
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return _jwt.GenerateToken(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepo.GetByEmailAsync(email)
                ?? throw new Exception("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            return _jwt.GenerateToken(user);
        }
    }
}
