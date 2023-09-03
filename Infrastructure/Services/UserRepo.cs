using Domain.Entities.Users;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class UserRepo : IUserRepo
    {
        private readonly ApplicationContext context;

        public UserRepo(ApplicationContext _context)
        {
            context = _context;
        }

        public async Task Add(User user)
        {
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();
            user.LastEditDate = DateTime.UtcNow;
            user.RegisterDate = DateTime.UtcNow;
            
            await context.Users.AddAsync(user);
        }

        public async Task Delete(Guid id)
        {
            var user = await Get(id);
            if (user == null)
                throw new Exception($"User Not founded {id}");
            context.Users.Remove(user);
        }

        public async Task<User?> Get(Guid id)
        {
            var response = await context.Users.FindAsync(id);
            return response;
        }

        public async Task<User?> Get(long mobile)
        {
            var response = await context.Users.FirstAsync(f=>f.Mobile == mobile);
            return response;
        }

        public async Task<IEnumerable<User>> GetAll(int take = 20, int skip = 0, string searchString = "")
        {
            IQueryable<User> query = context.Users;
            if (!string.IsNullOrEmpty(searchString))
                /*query = */query.Where(w => w.Name != null && w.Name.Contains(searchString));
                var response = await query.Skip(skip).Take(take).ToListAsync();
            return response;
        }

        public Task Update(User user)
        {
            user.LastEditDate = DateTime.UtcNow;
            context.Users.Update(user);
            return Task.CompletedTask;
        }
    }
}
