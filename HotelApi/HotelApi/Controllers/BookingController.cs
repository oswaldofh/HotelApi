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
    //[AllowAnonymous]
    public class BookingController : ControllerBase
    {

        protected readonly IBookingRepository _repository;
        protected readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly IMailRepository _mailRepository;
        protected ResponseApi _response;

        public BookingController(
            IBookingRepository repository,
            IMapper mapper,
            IRoomRepository roomRepository,
            IMailRepository mailRepository

        )
        {
            _repository = repository;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _mailRepository = mailRepository;
            _response = new();
        }


        /// <summary>
        /// Obtiene todos los registros
        /// </summary>
        /// <response code="200"> Si se obtiene el listado</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        //[AllowAnonymous] //PARA QUE LO PUEDAN VER SIN AUTORIZACIÓN O SE PUEDE DEJAR EN BLANCO SIN ESTE CAMPO
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
            var bookings = new List<InformationBookingDto>();
            foreach (var list in data)
            {
                bookings.Add(_mapper.Map<InformationBookingDto>(list));
            }

            return Ok(bookings);
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
        [HttpGet("{id:int}", Name = "GetBookingById")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var data = await _repository.GetById(id);

            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese id");
                return BadRequest(_response);
            }
            var booking = _mapper.Map<InformationBookingDto>(data);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = booking;
            return Ok(_response);
        }


        /// <summary>
        /// Obtiene un registro pasando el documento por parametro
        /// </summary>
        /// <param name="document">Documento</param>
        /// <returns>Retorna un registro</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        //[HttpGet("{document}", Name = "GetBookingByDocument")]
        [HttpGet("GetBookingByDocument/{document}")]
        public async Task<IActionResult> GetBookingByDocument(string document)
        {
            var data = await _repository.GetByDocument(document);
            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese documento");
                return BadRequest(_response);
            }

            var hotel = _mapper.Map<InformationBookingDto>(data);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = hotel;
            return Ok(_response);
        }


        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">CreateBookingDto</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto model) //FromForm, FromBody
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var room = await _roomRepository.GetById(model.RoomId);
            if (room == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe una habitación con el id {model.RoomId}");
                return BadRequest(_response);
            }


            try
            {
                var iva = room.Iva / 100;
                var valorIva = iva * room.Price;

                var booking = _mapper.Map<Booking>(model);
                booking.BookingStatus = BookingStatus.Abierta;
                booking.TotalValue = (decimal)(room.Price + valorIva);
                booking.ValueIva = (decimal)valorIva;
                await _repository.Save(booking);
                /*
                ResponseEmail response = _mailRepository.SendMail(
                    $"{booking.FullName}",
                    booking.Email,
                    "HotelApi - Información de reserva",
                    $"<h1>HotelApi - Informacion de la reserva</h1>" +
                    $"Saludos {model.FullName } Se ha realizado la reserva {booking.Id} en el hotel {room.Hotel.Name}" +
                    $"<hr/> <br/> <p>Habitación : {room.Number} </p>" +
                    $"<br/> <p>Valor de iva : {booking.ValueIva} </p>" +
                    $"<br/> <p>Valor : {room.Price} </p>" +
                    $"<br/> <p>Valor total : {booking.TotalValue} </p>"
                );*/



                var bookingDto = _mapper.Map<InformationBookingDto>(booking);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("Se creo la reserva correctamente al correo le llegara informacion al respecto");
                _response.Result = bookingDto;

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
        /// <param name="model">BookingDto</param>
        /// <returns>Retorna el registro acutlizado</returns>
        /// <response code="200">Se ha actualizado correctamente el registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        // [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BookingDto model)
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
            var room = await _roomRepository.GetById(model.RoomId);
            if (room == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un hotel con el id {model.RoomId}");
                return BadRequest(_response);
            }


            try
            {
                var iva = room.Iva / 100;
                var valorIva = iva * room.Price;

                var booking = _mapper.Map<Booking>(model);
                booking.TotalValue = (decimal)(room.Price + valorIva);
                booking.ValueIva = (decimal)valorIva;

                await _repository.Update(booking);

                var bookingDto = _mapper.Map<InformationBookingDto>(booking);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se actualizo el registro correctament");
                _response.Result = bookingDto;

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
        //[Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeleteBooking")]
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
