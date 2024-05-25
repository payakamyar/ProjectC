using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectC.Contracts;
using ProjectC.Data.Context;
using ProjectC.Data.Models;
using ProjectC.Utils;

namespace ProjectC.Data.Repository
{
    public class EfUsersRepository: IUsersRepository
    {
        private readonly ProjectContext _context;
        public EfUsersRepository(ProjectContext projectContext) 
        { 
            _context = projectContext;
        }


		public async Task<IEnumerable<User>> GetUsersPagination(int page = 1, int itemsPerPage = 10)
		{
			return await _context.Users.Include(p => p.Role).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
		}


		public async Task<User?> GetUserById(int id)
		{
			return await _context.Users.Include(p => p.Role).Where(p => p.Id == id).FirstOrDefaultAsync();
		}


		public async Task<int> GetAllUsersCount() => await _context.Users.CountAsync();


		public async Task<bool> ChangePassword(int id, string newPassword)
		{
			User? user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
			if(user == null)
			{
				return false;
			}
			user.Password = newPassword;
			_context.Users.Entry(user).CurrentValues.SetValues(user);
			await _context.SaveChangesAsync();
			return true;
		}


		public async Task<User?> AddUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}


		public async Task<User?> EditUser(User user)
		{
			User? userFound = await GetUserById(user.Id);
            if (userFound is null)
            {
                return null;
            }
			user.Password = userFound.Password;
            _context.Users.Entry(userFound).CurrentValues.SetValues(user);
			await _context.SaveChangesAsync();
			return user;
		}


		public async Task<bool> DeleteUser(int id)
		{
			User? userFound = await GetUserById(id);
			
			if (userFound is null)
				return false;

			_context.Users.Remove(userFound);
			await _context.SaveChangesAsync();
			return true;
		}
		public async Task<User?> GetUserByUserName(string userName)
		{
			return await _context.Users.Include(p => p.Role).Where(p => p.UserName == userName).FirstOrDefaultAsync();
		}
	}
}
