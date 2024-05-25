using Microsoft.EntityFrameworkCore;
using ProjectC.Data.Context;
using ProjectC.Data.Models;

namespace ProjectC.Data.Repository
{
    public class RoleRepository
    {


        private readonly ProjectContext _context;


        public RoleRepository(ProjectContext projectContext) 
        { 
            _context = projectContext;
        }

		public async Task<IEnumerable<Role>> GetRoles()
		{
			return await _context.Roles.ToListAsync();
		}

		public async Task<Role?> GetRoleById(int id)
		{
			return await _context.Roles.Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<Role> AddRole(Role role)
		{
			_context.Roles.Add(role);
			await _context.SaveChangesAsync();
			return role;
		}

		public async Task<Role?> EditRole(Role role)
		{
			Role? roleFound = await GetRoleById(role.Id);
            if (roleFound is null)
            {
                return null;
            }
            _context.Roles.Entry(roleFound).CurrentValues.SetValues(role);
			await _context.SaveChangesAsync();
			return role;
		}

		public async Task<bool> DeleteRole(Role role)
		{
			Role? userFound = await GetRoleById(role.Id);
			if (role is null)
				return false;
			_context.Roles.Remove(role);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
