using System.ComponentModel.DataAnnotations;

namespace ProjectC.Data.DTO
{
	public class UserPasswordChangeDTO
	{
		public int Id { get; set; }


		[Required]
		[MinLength(4)]
		public string Password { get; set; }

		[Required]
		[MinLength(4)]
		[Compare(nameof(Password), ErrorMessage = "Password does not match with its confirmation.")]
		public string ConfirmPassword { get; set; }
	}
}
