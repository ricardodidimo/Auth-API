using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Data;
using api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> SelectUsersAsync()
        {
            return _context.users.Select(u => new User(){
                UserId = u.UserId,
                username = u.username
            }).ToListAsync();
        }  

        public Task<User> SelectUserByNameAsync(string username)
        {
            return _context.users
                .Where(u => u.normalized_username.Equals(username))
                .FirstOrDefaultAsync();
        }
        
        public Task<User> SelectUserByIdAsync(int id)
        {
           return _context.users.Where(u => u.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<User> InsertUserAsync(User userInput)
        {
            User added =  _context.users.Add(userInput).Entity;
            await _context.SaveChangesAsync();
            return added;
        }
        public async Task<User> UpdateUserAsync(User input)
        {
            User userUpdated = _context.users.Update(input).Entity;
            await _context.SaveChangesAsync();
            return userUpdated;
        }
        public async Task<User> DeleteUserAsync(User toDelete)
        {
            _context.users.Remove(toDelete);
            await _context.SaveChangesAsync();

            return toDelete;
        }
    }
}