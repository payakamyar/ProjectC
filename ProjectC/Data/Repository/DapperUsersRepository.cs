using Dapper;
using Microsoft.Data.SqlClient;
using ProjectC.Contracts;
using ProjectC.Data.Context;
using ProjectC.Data.Models;

namespace ProjectC.Data.Repository
{
	public class DapperUsersRepository : IUsersRepository
	{
		private readonly ProjectContext _context;
        public DapperUsersRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task<User?> AddUser(User user)
		{
			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			int affected = await connection
				.ExecuteAsync("EXEC sp_add_user @user_name,@password,@email,@first_name,@last_name,@phone,@role_id"
				, new
				{
					user_name = user.UserName,
					password = user.Password,
					email = user.Email,
					first_name = user.FirstName,
					last_name = user.LastName,
					phone = user.Phone,
					role_id = user.RoleId
				});
			return affected > 0 ? user : null;
		}

		public async Task<bool> ChangePassword(int id, string newPassword)
		{
			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			int affected = await connection.ExecuteAsync("EXEC sp_change_password @Id,@password"
				, new
				{
					Id = id,
					password = newPassword
				});
			return affected > 0;
		}

		public async Task<bool> DeleteUser(int id)
		{
			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			int affected = await connection.ExecuteAsync("EXEC sp_delete_user @Id"
				, new
				{
					Id = id
				});
			return affected > 0;
		}

		public async Task<User?> EditUser(User user)
		{
			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			int affected = await connection.
								 ExecuteAsync("EXEC sp_edit_user @Id,@user_name,@email,@first_name,@last_name,@phone,@role_id"
				,new { Id = user.Id, user_name = user.UserName, email = user.Email, first_name = user.FirstName,
						last_name = user.LastName, phone = user.Phone, role_id = user.RoleId});
			return affected > 0 ? user : null;
		}

		public async Task<int> GetAllUsersCount()
		{
			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			return await connection.ExecuteScalarAsync<int>("SELECT dbo.get_count();");
		}

		public async Task<User?> GetUserById(int id)
		{

			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			IEnumerable<dynamic> q = await connection.QueryAsync("SELECT * FROM dbo.get_user(@Id);", new { Id = id });
			return q.Select(row =>
								new User
								{
									Id = row.id,
									UserName = row.user_name,
									Password = "",
									Email = row.email,
									FirstName = row.first_name,
									LastName = row.last_name,
									Phone = row.phone,
									RoleId = row.role_id,
									Role = new Role { Id = row.role_id, RoleName = row.role_name }
								}).FirstOrDefault();
		}

		public async Task<IEnumerable<User>> GetUsersPagination(int page, int itemsPerPage)
		{
			string connectionString = Environment.GetEnvironmentVariable("ConnectionStringDefault");
			using SqlConnection connection = new SqlConnection(connectionString);
			IEnumerable<dynamic> q = await connection.QueryAsync("SELECT * FROM dbo.get_users_pagination(@Page, @ItemsPerPage);", 
				new { Page = page, ItemsPerPage = itemsPerPage});
			return q.Select(row =>
			new User
			{
				Id = row.id,
				UserName = row.user_name,
				Password = "",
				Email = row.email,
				FirstName = row.first_name,
				LastName = row.last_name,
				Phone = row.phone,
				RoleId = row.role_id,
				Role = new Role { Id = row.role_id, RoleName = row.role_name }
			});
		}

		public async Task<User?> GetUserByUserName(string userName)
		{
			using SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("ConnectionStringDefault"));
			IEnumerable<dynamic> q = await connection.QueryAsync("SELECT * FROM dbo.get_user_by_username(@user_name);", 
									new { user_name = userName });
			return q.Select(row =>
								new User
								{
									Id = row.id,
									UserName = row.user_name,
									Password = "",
									Email = row.email,
									FirstName = row.first_name,
									LastName = row.last_name,
									Phone = row.phone,
									RoleId = row.role_id,
									Role = new Role { Id = row.role_id, RoleName = row.role_name }
								}).FirstOrDefault();
		}
	}
}
