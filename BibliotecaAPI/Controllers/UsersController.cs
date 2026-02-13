using AutoMapper;
using BibliotecaAPI.Data;
using BibliotecaAPI.DTOs.Users;
using BibliotecaAPI.Entities;
using BibliotecaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController: ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly IUsersService usersService;
        private readonly AplicationDbContext context;
        private readonly IMapper mapper;

        public UsersController(UserManager<User> userManager, 
            IConfiguration configuration, SignInManager<User> signInManager, IUsersService usersService,
            AplicationDbContext context, IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.usersService = usersService;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy ="isadmin")]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            var users = await context.Users.ToListAsync();
            var usersDTO = mapper.Map<IEnumerable<UserDTO>>(users);
            return usersDTO;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(UserCredentialsDTO userCredentialsDTO)
        {
            var user = new User
            {
                UserName = userCredentialsDTO.Email,
                Email = userCredentialsDTO.Email,
            };

            var result = await userManager.CreateAsync(user, userCredentialsDTO.Password!);

            if (result.Succeeded)
            {
                var authResponse = await BuildToken(userCredentialsDTO);
                return authResponse;
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }
                return ValidationProblem();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(UserCredentialsDTO userCredentials)
        {
            var user = await userManager.FindByEmailAsync(userCredentials.Email);

            if (user is null)
            {
                return ReturnIncorrectLogin();
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, userCredentials.Password!,
                lockoutOnFailure: false);

            if (result.Succeeded) return await BuildToken(userCredentials);
            else return ReturnIncorrectLogin();
        }

        [HttpGet("reload")]
        [Authorize]
        public async Task<ActionResult<AuthResponseDTO>> ReloadToken()
        {
            var user = await usersService.GetUser();

            if (user is null) return NotFound();

            var userCredentialsDTO = new UserCredentialsDTO { Email = user.Email!};

            var response = await BuildToken(userCredentialsDTO);

            return response;
        }

        [HttpPost("add-admin")]
        [Authorize(Policy = "isadmin")]
        public async Task<ActionResult> AddAdmin(EditClaimDTO editClaimDTO)
        {
            var user = await userManager.FindByEmailAsync(editClaimDTO.Email);

            if (user is null) return NotFound();

            await userManager.AddClaimAsync(user, new Claim("isadmin", "true"));
            return NoContent();
        }

        [HttpPost("remove-admin")]
        [Authorize(Policy = "isadmin")]
        public async Task<ActionResult> RemoveAdmin(EditClaimDTO editClaimDTO)
        {
            var user = await userManager.FindByEmailAsync(editClaimDTO.Email);

            if (user is null) return NotFound();

            await userManager.RemoveClaimAsync(user, new Claim("isadmin", "true"));
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Put(UpdateUserDTO updateUserDTO)
        {
            var user = await usersService.GetUser();

            if (user is null) return NotFound();

            user.Birthday = updateUserDTO.Birthday;

            await userManager.UpdateAsync(user);

            return NoContent();
        }

        private ActionResult ReturnIncorrectLogin()
        {
            ModelState.AddModelError(string.Empty, "Incorrect Login");
            return ValidationProblem();
        }
        private async Task<AuthResponseDTO> BuildToken(UserCredentialsDTO userCredentialsDTO)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialsDTO.Email)
            };

            var user = await userManager.FindByEmailAsync(userCredentialsDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddYears(1);

            var tokenSecurity = new JwtSecurityToken(
                issuer: null, audience: null, claims: claims, expires: expires, signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSecurity);

            return new AuthResponseDTO
            {
                Token = token,
                Expires = expires
            };
        }
    }
}
