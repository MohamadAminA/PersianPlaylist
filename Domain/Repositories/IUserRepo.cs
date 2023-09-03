using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepo
    {
        /// <summary>
        /// Get and Find a User by Id From Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User</returns>
        public Task<User?> Get(Guid id);
        /// <summary>
        /// Get and Find a User by Mobile Number From Database
        /// </summary>
        /// <param name="mobile">long mobile number</param>
        /// <returns>User</returns>
        public Task<User?> Get(long mobile);

        /// <summary>
        /// Update Existing User in Database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task Update(User user);
        /// <summary>
        /// Delete a User from Database By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task Delete(Guid id);

        /// <summary>
        /// Get All users by Pagination and filter
        /// </summary>
        /// <param name="take">number of users to take</param>
        /// <param name="skip">number of users for skip</param>
        /// <param name="searchString">search a phrase in name of users</param>
        /// <returns></returns>
        public Task<IEnumerable<User>> GetAll(int take = 20,int skip = 0,string searchString = "");

        /// <summary>
        /// Add new User to Database
        /// </summary>
        /// <param name="user">user model to add</param>
        /// <returns></returns>
        public Task Add(User user);
    }
}
