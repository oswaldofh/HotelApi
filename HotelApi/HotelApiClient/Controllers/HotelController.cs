using AutoMapper;
using HotelApi.Common.Response;
using HotelApi.Domain.DTOs;
using HotelApi.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace HotelApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        protected readonly IRoomRepository _repository;
        protected readonly IMapper _mapper;
        protected ResponseApi _response;

        public HotelController(
            IRoomRepository repository,
            IMapper mapper  
        )
        {
            _repository = repository;
            _mapper = mapper;
            _response = new();
        }


        /// <summary>
        /// Obtienen los hoteles de una ciudad pasando el nombre de la ciudad por parametro
        /// </summary>
        /// <param name="city">City</param>
        /// <returns>Retorna una raza</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        //[HttpGet("{cityId:int}", Name = "GetHotelInCity")]
        [HttpGet("GetHotelByNameCity/{city}")]
        public async Task<IActionResult> GetHotelInCity(string city)
        {
            var data = await _repository.GetByCity(city);

            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existen hoteles en la ciudad {city}");
                return BadRequest(_response);
            }
            
            var rooms = new List<InformationRoomDto>();
            foreach (var list in data)
            {
                rooms.Add(_mapper.Map<InformationRoomDto>(list));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = rooms;
            return Ok(_response);
        }

        /// <summary>
        /// Obtienen los hoteles de una ciudad pasando la cantidad de personas por parametro
        /// </summary>
        /// <param name="number">Cantidad personas</param>
        /// <returns>Retorna una raza</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("{number:int}", Name = "GetHotelNumber")]
        //[HttpGet("GetHotelNumber/{number:int}")]
        public async Task<IActionResult> GetHotelNumber(int number)
        {
            var data = await _repository.GetByNumberPerson(number);

            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existen habitaciones que alojen esa cantidad {number}");
                return BadRequest(_response);
            }

            var rooms = new List<InformationRoomDto>();
            foreach (var list in data)
            {
                rooms.Add(_mapper.Map<InformationRoomDto>(list));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = rooms;
            return Ok(_response);
        }

        /// <summary>
        /// Obtienen los hoteles disponible pasando un fecha por parametro
        /// </summary>
        /// <param name="date">City</param>
        /// <returns>Retorna una raza</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        //[HttpGet("{cityId:int}", Name = "GetHotelInCity")]
        [HttpGet("GetHotelInDate/{date}")]
        public async Task<IActionResult> GetHotelInDate(DateTime date)
        {
            var data = await _repository.GetByDate(date);

            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existen habitaciones disponibles en los hoteles en la fecha {date}");
                return BadRequest(_response);
            }

            var rooms = new List<InformationRoomDto>();
            foreach (var list in data)
            {
                rooms.Add(_mapper.Map<InformationRoomDto>(list));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = rooms;
            return Ok(_response);
        }
    }
}
