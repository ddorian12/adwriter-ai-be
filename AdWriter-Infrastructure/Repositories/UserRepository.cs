using AdWriter_Application.Interfaces.Repositories;
using AdWriter_Domain.Entities;
using AdWriter_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWriter_Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByEmailAsync(string email) =>
            _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public Task<User?> GetByIdAsync(Guid id) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public Task AddAsync(User user)
        {
            _context.Users.Add(user);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }

}
