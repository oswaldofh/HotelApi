using AutoMapper;
using HotelApi.Common.Enums;
using HotelApi.Common.Response;
using HotelApi.Domain.DTOs;
using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    //[AllowAnonymous]
    public class HotelController : ControllerBase
    {
        protected readonly IHotelRepository _repository;
        protected readonly IMapper _mapper;
        private readonly IHotelTypeRepository _hotelType;
        private readonly ICityRepository _cityRepository;
        protected ResponseApi _response;

        public HotelController(
            IHotelRepository repository,
            IMapper mapper, 
            IHotelTypeRepository hotelType,
            ICityRepository cityRepository
        )
        {
            _repository = repository;
            _mapper = mapper;
            _hotelType = hotelType;
            _cityRepository = cityRepository;
            _response = new();
        }


        /// <summary>
        /// Obtiene valores de todos los registros
        /// </summary>
        /// <response code="200"> Si se obtiene el listado</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repository.GetAll();
            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existen registros guardados");
                return BadRequest(_response);
            }
            var hotels = new List<HotelDto>();
            foreach (var list in data)
            {
                hotels.Add(_mapper.Map<HotelDto>(list));
            }

            return Ok(hotels);
        }

        /// <summary>
        /// Obtiene un registro pasando el id por parametro
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("{id:int}", Name = "GetHotelById")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var data = await _repository.GetById(id);


            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese id");
                return BadRequest(_response);
            }
            var hotel = _mapper.Map<HotelDto>(data);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = hotel;
            return Ok(_response);
        }

        /// <summary>
        /// Obtiene un registro pasando el nombre por parametro
        /// </summary>
        /// <param name="name">Nombre</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("{name}", Name = "GetHotelByName")]
        public async Task<IActionResult> GetHotelByName(string name)
        {
            var data = await _repository.GetByName(name);
            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese nombre");
                return BadRequest(_response);
            }

            var hotel = _mapper.Map<HotelDto>(data);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = hotel;
            return Ok(_response);
        }


        /// <summary>
        /// Obtienen los hoteles de una ciudad pasando el id de la ciudad por parametro
        /// </summary>
        /// <param name="cityId">cityId</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("GetHotelInCity/{cityId:int}")]
        public async Task<IActionResult> GetHotelInCity(int cityId)
        {
            var data = await _repository.GetHotelsInCity(cityId);

            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existen hoteles en la ciudad {cityId}");
                return BadRequest(_response);
            }

            var hotels = new List<HotelDto>();
            foreach (var list in data)
            {
                hotels.Add(_mapper.Map<HotelDto>(list));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = hotels;
            return Ok(_response);
        }


        /// <summary>
        /// Obtienen los hoteles de una ciudad pasando el id del tipo  de hotel por parametro
        /// </summary>
        /// <param name="typeId">typeId</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("GetHotelInType/{typeId:int}")]
        public async Task<IActionResult> GetHotelInType(int typeId)
        {
            var data = await _repository.GetHotelsInType(typeId);

            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existen hoteles del tipo {typeId}");
                return BadRequest(_response);
            }

            var hotels = new List<HotelDto>();
            foreach (var list in data)
            {
                hotels.Add(_mapper.Map<HotelDto>(list));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = hotels;
            return Ok(_response);
        }

        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">CreateRoomTypeDto</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHotelDto model)
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }
            var type = await _hotelType.Exist(model.HotelTypeId);
            if (!type)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un tipo de hotel con el id {model.HotelTypeId}");
                return BadRequest(_response);
            }

            var city = await _cityRepository.ExistCity(model.CityId);
            if (!city)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe una ciudad con el id {model.CityId}");
                return BadRequest(_response);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(model);
                hotel.HotelState = HotelState.Abierto;

                await _repository.Save(hotel);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se guardo el registro correctamente");

                var hotelDto = _mapper.Map<HotelDto>(hotel);
                _response.Result = hotelDto;

                return Ok(_response);

            }
            catch (DbUpdateException e)
                when (e.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add($"Ya existe un registro con esos parametros");
                return BadRequest(_response);
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add(e.Message);
                return BadRequest(_response);
            }
        }

        /// <summary>
        /// Actualiza un registro
        /// </summary>
        /// <param name="model">RoomTypeDto</param>
        /// <returns>Retorna el registro acutlizado</returns>
        /// <response code="200">Se ha actualizado correctamente el registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] HotelDto model)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var exist = await _repository.Exist(model.Id);
            if (!exist)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {model.Id}");
                return BadRequest(_response);
            }

            var type = await _hotelType.Exist(model.HotelTypeId);
            if (!type)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un tipo de hotel con el id {model.HotelTypeId}");
                return BadRequest(_response);
            }

            var city = await _cityRepository.ExistCity(model.CityId);
            if (!city)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe una ciudad con el id {model.CityId}");
                return BadRequest(_response);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(model);

                await _repository.Update(hotel);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se actualizo el registro correctamente");
                var hotelDto = _mapper.Map<HotelDto>(hotel);
                _response.Result = hotelDto;

                return Ok(_response);

            }
            catch (DbUpdateException e)
                when (e.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add($"Ya existe un registro con esos parametros");
                return BadRequest(_response);
            }
            catch (Exception e)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add(e.Message);
                return BadRequest(_response);
            }
        }

        /// <summary>
        /// Borra un registro pasando el id por parametro 
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="204">Si se elimina el registro</response>
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="404">Si no existe el registro</response>
        /// <response code="500">Se ha producido un error interno en el servido</response> 
        [HttpDelete("{id:int}", Name = "DeleteHotel")]
        public async Task<IActionResult> Delete(int id)
        {

            var data = await _repository.Exist(id);
            if (!data)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {id}");
                return BadRequest(_response);
            }


            var deleted = await _repository.Delete(id);
            if (!deleted)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.Messages.Add($"Algo salio mal eliminando el registro {id}");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _response.Messages.Add("Se elimino el registro correctamente");
            return Ok(_response);
        }
    }
}
