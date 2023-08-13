using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce_App.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Ecommerce_AppUser class

[NotMapped]
public class Ecommerce_AppUser : IdentityUser
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Image { get; set; } = string.Empty;
	public bool Status { get; set; }

}

