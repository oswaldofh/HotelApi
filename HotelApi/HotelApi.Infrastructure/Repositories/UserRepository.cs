using AutoMapper;
using HotelApi.Common.Enums;
using HotelApi.Common.Response;
using HotelApi.Domain.DTOs;
using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using HotelApi.Domain.Tokens;
using HotelApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace HotelApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ICreateToken _token;
        private readonly IMapper _mapper;

        public UserRepository(
           DataContext context,
           UserManager<User> userManager,
           RoleManager<IdentityRole> roleManager,
           SignInManager<User> signInManager,
           IConfiguration configuration,
           ICreateToken token,
            IMapper mapper

        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _token = token;
            _mapper = mapper;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> AddUserAsync(CreateUserDto model)
        {
            User user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Document = model.Document,
                Email = model.Email,
                UserName = model.Email,
                UserType = UserType.User
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }


            User newUser = await GetUserAsync(user.UserName);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());

            return user;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> GetUserDocumentAsync(string document)
        {
            var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Document == document);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {

            return await _context.Users.ToListAsync();
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<LoginResponse> LoginAsync(LoginUserDto model)
        {
            //TODO: modificar el valor (FALSE) HACE REFERENCIA AL CANTIDAD DE INTENTOS DE LOGUE Y BLOQUEA EL USERS
            SignInResult result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);

            LoginResponse loginResponse = new LoginResponse();

            if (result.Succeeded)
            {
                User? user = await _userManager.FindByNameAsync(model.Username);


                JwtSecurityToken token = await _token.GenerateToken(user);

                loginResponse.Result = _mapper.Map<UserDto>(user);
                loginResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
                loginResponse.IsSuccess = true;
                loginResponse.Expiration = token.ValidTo;

                return loginResponse;
            }

            else if (result.IsLockedOut) //PARA SABER SI EL USUARIO ESTA BLOQUEADO
            {
                loginResponse.IsSuccess = false;
                loginResponse.ErrorMessages = "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos";
                return loginResponse;
            }
            else if (result.IsNotAllowed) //QUE NO SE HA CONFIRMADO
            {
                loginResponse.IsSuccess = false;
                loginResponse.ErrorMessages = "El usuario no ha sido habilitado, debes seguir las instrucciones del correo enviado para poder habilitarle en el sistema.";
                return loginResponse;
            }
            else
            {
                loginResponse.IsSuccess = false;
                loginResponse.ErrorMessages = "Email o contraseña incorrectos";
                return loginResponse;
            }

        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
