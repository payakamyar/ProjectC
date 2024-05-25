using ProjectC.Contracts;
using ProjectC.Data.Models;
using ProjectC.Data.Repository;
using ProjectC.Utils;
using System.Collections;
using System.Security.Claims;

namespace ProjectC.Services
{

	public class AccountDataManager
	{
		private IUsersRepository usersRepository;
		private RoleRepository roleRepository;
		private HttpContext httpContext;
		private int currentUserId;
		public AccountDataManager(IUsersRepository usersRepository, RoleRepository roleRepository, IHttpContextAccessor contextAccessor)
        {
            this.usersRepository = usersRepository;
			this.roleRepository = roleRepository;
			httpContext = contextAccessor.HttpContext!;
			ClaimsIdentity identity = httpContext.User.Identity as ClaimsIdentity;
			int.TryParse(identity.FindFirst("ID").Value, out currentUserId);
		}

		public async Task<IEnumerable<User>> GetUsers(int page)
		{
			return await usersRepository.GetUsersPagination(page);
		}

		public async Task<int> GetUsersPageNumber(int userPerPage = 10)
		{
			int count = await usersRepository.GetAllUsersCount();
			if (count > 0)
			{
				return (int)Math.Ceiling(((double)count) / userPerPage);
			}
			return 0;
		}

		public async Task<bool> ChangePassword(int userId, string newPassword)
		{
			User? user = await GetUserById(userId);
            if (user is not null)
            {
				string hashedPassword = PasswordUtility.GetPasswordHash(newPassword);
				return await usersRepository.ChangePassword(userId, hashedPassword);
            }
			return false;
		}

		public async Task<User?> GetUserById(int id)
		{
			return await usersRepository.GetUserById(id);
		}

		public async Task<User?> GetUserByUserName(string userName)
		{
			return await usersRepository.GetUserByUserName(userName);
		}

		public async Task<User> AddUser(User user)
		{
			user.Password = PasswordUtility.GetPasswordHash(user.Password);
			return await usersRepository.AddUser(user);
		}


		public async Task<User?> EditUser(User user)
		{
			return await usersRepository.EditUser(user);
		}


		public async Task<bool> DeleteUser(int id)
		{
			if (id == currentUserId)
				return false;

			return await usersRepository.DeleteUser(id);
		}


		public async Task<IEnumerable<Role>> GetRoles()
		{
			return await roleRepository.GetRoles();
		}

		public async Task<Role> AddRole(Role role)
		{
			return await roleRepository.AddRole(role);
		}


		public async Task<Role?> EditRole(Role role)
		{
			return await roleRepository.EditRole(role);
		}


		public async Task<bool> DeleteRole(Role role)
		{
			if (role.RoleName == "Admin" || role.RoleName == "admin")
				return false;

			return await roleRepository.DeleteRole(role);
		}
	}
}
