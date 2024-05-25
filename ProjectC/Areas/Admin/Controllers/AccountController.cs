using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Data.DTO;
using ProjectC.Data.Models;
using ProjectC.Services;
using System.ComponentModel;
using System.Security.Claims;

namespace ProjectC.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("{area}/{controller}/{action}")]
	[Authorize(Policy = "Admin-Role")]
	public class AccountController : Controller
	{

		private AccountDataManager accountManager;

        public AccountController(AccountDataManager accountManager)
        {
            this.accountManager = accountManager;
        }
		[HttpGet]
        public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> ManageUsers(int page = 1)
		{
			IEnumerable<User> users = await accountManager.GetUsers(page);
			ViewBag.Count = await accountManager.GetUsersPageNumber();
			return View(users);
		}


		[HttpGet]
		public async Task<IActionResult> AddUser()
		{
			ViewBag.Roles = await accountManager.GetRoles();
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> AddUserPost(User user)
		{
            if (await IsUsernameUnique(user.UserName) == false)
			{
				ViewBag.Roles = await accountManager.GetRoles();
				ViewBag.Error = "Username already exists";
				return View("AddUser");
            }
            if (await accountManager.AddUser(user) != null)
			{
				return RedirectToAction("ManageUsers","Account");
			}
			return View("AddUser");
		}


		[HttpGet]
		public async Task<IActionResult> EditUser(int id)
		{
			User? user = await accountManager.GetUserById(id);

			if(user is not null)
			{
				ViewBag.Roles = await accountManager.GetRoles();
				return View(user);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditUserPost(User user)
		{
			User? foundUser = await accountManager.GetUserById(user.Id);

			if (foundUser is not null)
			{
				if(foundUser.UserName != user.UserName && await IsUsernameUnique(user.UserName) == false)
				{
					ViewBag.Roles = await accountManager.GetRoles();
					ViewBag.Error = "Username already exists";
					return View(nameof(EditUser),foundUser);
				}
			}
			else
			{
				return NotFound();
			}

			User? editedUser = await accountManager.EditUser(user);
			if(editedUser is not null)
			{
				return RedirectToAction(nameof(ManageUsers));
			}
			return View(nameof(EditUser), user);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteUser(int id)
		{

			User? user = await accountManager.GetUserById(id);

			if (user is not null)
				return View(user);

			return NotFound();
		}

		[HttpGet]
		public async Task<IActionResult> DeleteUserAction([FromQuery] int id)
		{
			ClaimsIdentity identity = User.Identity as ClaimsIdentity;
			int.TryParse(identity.FindFirst("Id").Value, out int currentUserId);

			if (id == currentUserId)
				return BadRequest();

			if (await accountManager.DeleteUser(id))
			{
				return RedirectToAction(nameof(ManageUsers));
			}

			return NotFound();
		}

		[HttpGet]
		public IActionResult ChangePassword(int id)
		{
			ViewBag.Id = id;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePasswordPost(UserPasswordChangeDTO user)
		{
			if(await accountManager.ChangePassword(user.Id, user.Password))
				return RedirectToAction(nameof(ManageUsers));

			return View(nameof(ChangePassword));
		}

		private async Task<bool> IsUsernameUnique(string userName)
		{
			return await accountManager.GetUserByUserName(userName) is null;
		}
	}
}
