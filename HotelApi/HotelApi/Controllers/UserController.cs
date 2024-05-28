using AutoMapper;
using HotelApi.Common.Enums;
using HotelApi.Common.Response;
using HotelApi.Domain.DTOs;
using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        protected ResponseApi _response;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _response = new();
        }

        /// <summary>
        /// Logueo de usuario 
        /// </summary>
        /// <param name="loginUserDto">loginUserDto</param>
        /// <returns>Retorna el usuario logueado con el token</returns>
        /// <response code="200">Si el usuario se loguea correctamente</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            if (ModelState.IsValid)
            {
                LoginResponse loginResponse = await _userRepository.LoginAsync(loginUserDto); //FUNCION QUE LOGUEA AL USUARIO

                if (loginResponse.IsSuccess)
                {
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    _response.Result = loginResponse;
                    return Ok(_response);
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Messages.Add(loginResponse.ErrorMessages);
                    return BadRequest(_response);
                }
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son validos");
                return BadRequest(_response);
            }
        }

        //[Authorize(Roles = "admin")]
        [AllowAnonymous]
        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">createCityDto</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model) //FromBody, FromForm
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var existUser = await _userRepository.GetUserAsync(model.Email);
            if (existUser != null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add($"Ya existe un usuario con el correo {model.Email}");
                return BadRequest(_response);
            }

            User user = _mapper.Map<User>(model);
            user.UserName = model.Email;
            user.UserType = UserType.Admin;


            IdentityResult result = await _userRepository.AddUserAsync(user, model.Password);

            if (result != IdentityResult.Success)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add(result.Errors.FirstOrDefault().Description);
                return BadRequest(_response);
            }

            await _userRepository.CheckRoleAsync(user.UserType.ToString());
            await _userRepository.AddUserToRoleAsync(user, user.UserType.ToString());


            var userDto = _mapper.Map<UserDto>(user);


            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            _response.Messages.Add($"Usuario creado correctamente {user.Email}");
            _response.Result = userDto;
            return Ok(_response);

        }
    }
}
