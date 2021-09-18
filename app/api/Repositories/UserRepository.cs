using System.Collections.Generic;
using System.Linq;
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
        public User SelectUserByName(string username)
        {
            return _context.users
                .Where(u => u.normalized_username.Equals(username))
                .FirstOrDefault();
        }

        public List<User> SelectUsers()
        {
            return _context.users.Select(u => new User(){
                UserId = u.UserId,
                username = u.username
            }).ToList();
        }  
        
        public User SelectUserById(int id)
        {
           return _context.users.Where(u => u.UserId == id).FirstOrDefault();
        }

        public User UpdateUser(User input)
        {
            User userUpdated = _context.users.Update(input).Entity;
            _context.SaveChanges();
            return userUpdated;
        }
        public User DeleteUser(User toDelete)
        {
            _context.users.Remove(toDelete);
            _context.SaveChanges();

            return toDelete;
        }
    }
}