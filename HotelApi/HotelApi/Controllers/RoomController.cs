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

namespace HotelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoomController : ControllerBase
    {
        protected readonly IRoomRepository _repository;
        protected readonly IMapper _mapper;
        private readonly IRoomTypeRepository _typeRepository;
        private readonly IHotelRepository _hotelRepository;
        protected ResponseApi _response;

        public RoomController(
            IRoomRepository repository,
            IMapper mapper,
            IRoomTypeRepository typeRepository,
            IHotelRepository hotelRepository
        )
        {
            _repository = repository;
            _mapper = mapper;
            _typeRepository = typeRepository;
            _hotelRepository = hotelRepository;
            _response = new();
        }


        /// <summary>
        /// Obtiene todos los registros
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
            var rooms = new List<InformationRoomDto>();
            foreach (var list in data)
            {
                rooms.Add(_mapper.Map<InformationRoomDto>(list));
            }

            return Ok(rooms);
        }

        /// <summary>
        /// Obtiene el valor pasando el id por parametro
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Retorna una raza</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("{id:int}", Name = "GetRoomById")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var data = await _repository.GetById(id);

            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese id");
                return BadRequest(_response);
            }
            var room = _mapper.Map<InformationRoomDto>(data);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = room;
            return Ok(_response);
        }

        /// <summary>
        /// Obtiene un registro pasando el nombre por parametro
        /// </summary>
        /// <param name="number">Nombre</param>
        /// <returns>Retorna un registro</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("GetRoomByNumber/{number}")]
        public async Task<IActionResult> GetRoomByNumber(string number)
        {
            var data = await _repository.GetByName(number);
            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese nombre");
                return BadRequest(_response);
            }
            var room = _mapper.Map<InformationRoomDto>(data);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = room;
            return Ok(_response);
        }



        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">CreateRoomDto</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto model)
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var hotel = await _hotelRepository.Exist(model.HotelId);
            if (!hotel)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un hotel con el id {model.HotelId}");
                return BadRequest(_response);
            }
            var type = await _typeRepository.Exist(model.RoomTypeId);
            if (!type)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un tipo de habitación con el id {model.RoomTypeId}");
                return BadRequest(_response);
            }

            try
            {
                var room = _mapper.Map<Room>(model);
                room.RoomStatus = RoomStatus.Disponible;

                await _repository.Save(room);

                var roomDto = _mapper.Map<InformationRoomDto>(room);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se guardo el registro correctamente");
                _response.Result = roomDto;

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
        /// <param name="model">RoomDto</param>
        /// <returns>Retorna el registro acutlizado</returns>
        /// <response code="200">Se ha actualizado correctamente el registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RoomDto model)
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

            var hotel = await _hotelRepository.Exist(model.HotelId);
            if (!hotel)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un hotel con el id {model.HotelId}");
                return BadRequest(_response);
            }
            var type = await _typeRepository.Exist(model.RoomTypeId);
            if (!type)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un tipo de habitación con el id {model.RoomTypeId}");
                return BadRequest(_response);
            }

            try
            {

                var room = _mapper.Map<Room>(model);

                await _repository.Update(room);

                var roomDto = _mapper.Map<InformationRoomDto>(room);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se actualizo el registro correctamente");
                _response.Result = roomDto;

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
        [HttpDelete("{id:int}", Name = "DeleteRoom")]
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
