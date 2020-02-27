using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Eduraise.Models;

namespace Eduraise.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : Controller
	{

		public EduraiseContext DataBase;
		private readonly EduraiseContext _context;

		public AccountController(EduraiseContext context)
		{
			_context = context;
		}

		[HttpPost("/token")]
		public IActionResult Token([FromForm] Admins admin)
		{
			var identity = GetIdentity(admin.AdminEmail, admin.AdminPassword);
			if (identity == null)
			{
				return BadRequest(new { errorText = "Invalid username or password." });
			}

			var now = DateTime.UtcNow;

			var jwt = new JwtSecurityToken(
				issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				notBefore: now,
				claims: identity.Claims,
				expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
				signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = new
			{
				access_token = encodedJwt,
				username = identity.Name,
				role = identity.RoleClaimType
			};

			return Json(response);
		}

		private ClaimsIdentity GetIdentity(string username, string password)
		{
			var person = new object();
			person = null;
			string role = null;
			if (_context.Admins.FirstOrDefault(up => up.AdminEmail.Contains(username)) != null)
			{
				person = _context.Admins.FirstOrDefault(x => x.AdminEmail == username && x.AdminPassword == password);
				role = "Admin";
			}
			else if (_context.Teachers.FirstOrDefault(up => up.TeacherEmail.Contains(username)) != null)
			{
				person = _context.Teachers.FirstOrDefault(x => x.TeacherEmail == username && x.TeacherPassword == password);
				role = "Teacher";
			}
			else if (_context.Students.FirstOrDefault(up => up.StudentEmail.Contains(username)) != null)
			{
				person = _context.Students.FirstOrDefault(x => x.StudentEmail == username && x.StudentPassword == password);
				role = "Student";
			}

			if (person != null)
			{
				var claims = new List<Claim> { new Claim(ClaimsIdentity.DefaultNameClaimType, username) };

				ClaimsIdentity claimsIdentity =
					new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, role);
				return claimsIdentity;
			}

			return null;
		}
	}
}