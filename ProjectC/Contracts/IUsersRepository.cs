using ProjectC.Data.Models;

namespace ProjectC.Contracts
{
	public interface IUsersRepository
	{
		Task<IEnumerable<User>> GetUsersPagination(int page = 1, int itemsPerPage = 10);
		Task<User?> GetUserById(int id);
		Task<User?> GetUserByUserName(string userName);
		Task <int> GetAllUsersCount();
		Task<bool> ChangePassword(int id, string newPassword);
		Task<User?> AddUser(User user);
		Task<User?> EditUser(User user);
		Task<bool> DeleteUser(int id);

	}
}
