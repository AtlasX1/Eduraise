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
			var optionsBuilder = new DbContextOptionsBuilder<EduraiseContext>();
			var options = optionsBuilder
				.UseSqlServer(@"Data Source=DESKTOP-6BABV49;Initial Catalog=dbo_CMS;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
				.Options;
			_context = new EduraiseContext(options);
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
				username = identity.Name
			};

			return Json(response);
		}

		private ClaimsIdentity GetIdentity(string username, string password)
		{
		
			
			var person = _context.Admins.FirstOrDefault(x => x.AdminEmail == username && x.AdminPassword==password);

			if (person != null)
			{
				var claims = new List<Claim>
				{
				new Claim(ClaimsIdentity.DefaultNameClaimType, person.AdminEmail)

				};
				ClaimsIdentity claimsIdentity =
				new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
				return claimsIdentity;
			}


			return null;
		}
	}
}