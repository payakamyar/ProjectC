using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectC.Data.DTO;
using ProjectC.Data.Models;
using ProjectC.Services;
using System.Security.Claims;

namespace ProjectC.Controllers
{

	[Authorize]
	public class ProfileController : Controller
	{

		private readonly AccountDataManager _accountManager;
		User currentUser;

        public ProfileController(AccountDataManager accountManager)
        {
            _accountManager = accountManager;
		}


		[HttpGet("/Profile")]
        public async Task<IActionResult> Index()
		{
			await CheckCurrentUser();

			if (currentUser is not null)
			{
				return View(currentUser);
			}

			return NotFound();
		}



		public async Task<IActionResult> EditProfile()
		{
			await CheckCurrentUser();

			if (currentUser is not null)
			{
				return View(currentUser);
			}

			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> EditUserPost(User user)
		{
			await CheckCurrentUser();

			user.Id = currentUser.Id;
			user.UserName = currentUser.UserName;
			user.RoleId = currentUser.RoleId;

			User? editedUser = await _accountManager.EditUser(user);
			if (editedUser is not null)
			{
				return RedirectToAction(nameof(Index));
			}
			return View(nameof(EditProfile), user);
		}

		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			await CheckCurrentUser();
			ViewBag.Id = currentUser.Id;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePasswordPost(UserPasswordChangeDTO user)
		{
			await CheckCurrentUser();
			user.Id = currentUser.Id;

			if (await _accountManager.ChangePassword(user.Id, user.Password))
				return RedirectToAction("Index", "Home");

			return View(nameof(ChangePassword));
		}

		private async Task CheckCurrentUser()
		{
			ClaimsIdentity identity = User.Identity as ClaimsIdentity;
			int.TryParse(identity.FindFirst("ID").Value, out int currentUserId);
			currentUser = await _accountManager.GetUserById(currentUserId);
		}
	}
}
