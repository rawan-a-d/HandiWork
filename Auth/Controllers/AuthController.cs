using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Controllers
{
	[Route("api/[controller]")] // api/auth
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ILogger<AuthController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;

		public AuthController(
				UserManager<IdentityUser> userManager,
				RoleManager<IdentityRole> roleManager,
				ILogger<AuthController> logger,
				IConfiguration configuration,
				IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_logger = logger;
			_configuration = configuration;
			_mapper = mapper;
		}


		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] UserCreateDto userCreateDto)
		{
			var existingUser = await _userManager.FindByEmailAsync(userCreateDto.Email);

			// if user exists
			if (existingUser != null)
			{
				return BadRequest(new AuthResult()
				{
					Errors = new List<string> {
						// give more generic error message
						"Email already in use"
					},
					Success = false
				});
			}

			// create user
			//var newUser = new IdentityUser() { Email = user.Email, UserName = user.Email };
			var newUser = _mapper.Map<IdentityUser>(userCreateDto);

			var isCreated = await _userManager.CreateAsync(newUser, userCreateDto.Password);

			if (isCreated.Succeeded)
			{
				// add the user to a role
				await _userManager.AddToRoleAsync(newUser, "AppUser");

				// generate token
				var authResult = await GenerateJwtToken(newUser);

				return Ok(authResult);
			}

			return BadRequest(new AuthResult()
			{
				Errors = isCreated.Errors.Select(x => x.Description).ToList(),
				Success = false
			});
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
		{
			var existingUser = await _userManager.FindByEmailAsync(userLoginDto.Email);

			if (existingUser == null)
			{
				return BadRequest(new AuthResult()
				{
					Errors = new List<string> {
						// give more generic error message
						"Invalid login request"
					},
					Success = false
				});
			}

			var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userLoginDto.Password);

			if (!isCorrect)
			{
				return BadRequest(new AuthResult()
				{
					Errors = new List<string> {
						// give more generic error message
						"Invalid login request"
					},
					Success = false
				});
			}

			var authResult = await GenerateJwtToken(existingUser);

			return Ok(authResult);
		}

		private async Task<AuthResult> GenerateJwtToken(IdentityUser user)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();

			// get security key
			var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig")["Secret"]);

			var claims = await GetAllValidClaims(user);

			// security token descriptor (contains claims)
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(6),
				// sigining credentials (type of algorithm used to encrypt token)
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			// security token
			var token = jwtTokenHandler.CreateToken(tokenDescriptor);
			var jwtToken = jwtTokenHandler.WriteToken(token);

			return new AuthResult()
			{
				Token = jwtToken,
				Success = true,
			};
		}

		// Get all the valid claims for the user
		private async Task<List<Claim>> GetAllValidClaims(IdentityUser user)
		{
			// generic list of claims
			var claims = new List<Claim> {
				new Claim("Id", user.Id),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				// unique id used to generate a new token
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			// get claims that were assigned to user
			var userClaims = await _userManager.GetClaimsAsync(user);
			claims.AddRange(userClaims);

			// get user roles and add it to the claims
			var userRoles = await _userManager.GetRolesAsync(user);

			foreach (var userRole in userRoles)
			{
				var role = await _roleManager.FindByNameAsync(userRole);

				if (role != null)
				{
					claims.Add(new Claim(ClaimTypes.Role, userRole));

					// get claims assigned to each role
					var roleClaims = await _roleManager.GetClaimsAsync(role);

					foreach (var roleClaim in roleClaims)
					{
						claims.Add(roleClaim);
					}
				}
			}

			return claims;
		}
	}
}