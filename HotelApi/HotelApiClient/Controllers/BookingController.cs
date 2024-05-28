using AutoMapper;
using HotelApi.Common.Enums;
using HotelApi.Common.Response;
using HotelApi.Domain.DTOs;
using HotelApi.Domain.Entities;
using HotelApi.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace HotelApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        /// Obtiene todos los registros de las reservas
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
                _response.Messages.Add("No existen reservas guardados");
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
        /// Crea una reserva en un hotel
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
                );



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
        /// Obtienen los hoteles disponible pasando un fecha por parametro
        /// </summary>
        /// <param name="date">City</param>
        /// <returns>Retorna una raza</returns>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        //[HttpGet("{cityId:int}", Name = "GetHotelInCity")]
        [HttpGet("GetBookingInDate/{date}")]
        public async Task<IActionResult> GetBookingInDate(DateTime date)
        {
            var data = await _repository.GetByDate(date);

            if (data.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existen reservas en la fecha {date}");
                return BadRequest(_response);
            }

            var rooms = new List<InformationBookingDto>();
            foreach (var list in data)
            {
                rooms.Add(_mapper.Map<InformationBookingDto>(list));
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = rooms;
            return Ok(_response);
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
                _response.Messages.Add("se actualizo la reserva correctament");
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

    }
}
