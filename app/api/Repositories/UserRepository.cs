using api.Models.Data;
using api.Models.Entities;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public User InsertUser(User userInput)
        {
            User added = _context.users.Add(userInput).Entity;
            _context.SaveChanges();
            return added;
        }
    }
}