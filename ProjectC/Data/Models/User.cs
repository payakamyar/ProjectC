using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectC.Data.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "username cannot be empty")]
	[MaxLength(32)]
	//[Remote(action:"IsUsernameUnique", controller:"Account", areaName:"Admin", ErrorMessage = "Username must be unique.")]
	public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "password cannot be empty")]
	[MinLength(4)]
	public string Password { get; set; } = null!;

	[MaxLength(50)]
	[EmailAddress]
	public string? Email { get; set; }

	[MaxLength(30)]
	public string? FirstName { get; set; }

	[MaxLength(30)]
	public string? LastName { get; set; }

    [MinLength(11)]
    [MaxLength(11)]
	[RegularExpression("^[0-9]*$", ErrorMessage = "Enter only digits in Phone Field.")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "role cannot be empty")]
    public int RoleId { get; set; }

	[ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; } = null!;
}
