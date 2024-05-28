using AutoMapper;
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
    public class CityController : ControllerBase
    {
        protected readonly ICityRepository _repository;
        protected readonly IMapper _mapper;
        protected ResponseApi _response;

        public CityController(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _response = new();
        }


        /// <summary>
        /// Obtiene valores de todos los tipos de clientes
        /// </summary>
        /// <response code="200"> Si se obtiene el listado</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _repository.GetCities();
            if (cities.IsNullOrEmpty())
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existen registros guardados");
                return BadRequest(_response);
            }

            return Ok(cities);
        }

        /// <summary>
        /// Obtiene el valor pasando el id por parametro
        /// </summary>
        /// <param name="id">Id</param>
        /// <response code="200"> Si se obtiene el registro</response>
        /// <response code="400">Si no encuentra la ruta</response> 
        /// <response code="403">Si la llamada no esta autenticada</response>
        /// <response code="404">Si no existe el registro</response>
        [HttpGet("{id:int}", Name = "GetCityById")]
        public async Task<IActionResult> GetCityById(int id)
        {
            var data = await _repository.GetCity(id);

            if (data == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese id");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = data;
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
        [HttpGet("{name}", Name = "GetCityByName")]
        public async Task<IActionResult> GetCityByName(string name)
        {
            var city = await _repository.GetCity(name);
            if (city == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add("No existe un registro con ese nombre");
                return BadRequest(_response);
            }
            var cityDto = _mapper.Map<CityDto>(city);

            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = cityDto;
            return Ok(_response);
        }



        /// <summary>
        /// Añade un registro
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Retorna el registro creado</returns>
        /// <response code="201">Se ha creado correctamente un nuevo registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCityDto model)
        {

            if (!ModelState.IsValid || model == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }


            try
            {
                var city = _mapper.Map<City>(model);

                await _repository.SaveCity(city);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se guardo el registro correctamente");
                _response.Result = city;

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
        /// <param name="model">CityDto</param>
        /// <returns>Retorna el registro acutlizado</returns>
        /// <response code="200">Se ha actualizado correctamente el registro</response>
        /// <response code="400">Si la solicitud es incorrecta</response> 
        /// <response code="401">No tiene autorizacion para realizar la solicitud</response>
        /// <response code="500">Se ha producido un error interno en el servidor</response>
        // [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CityDto model)
        {
            if (!ModelState.IsValid)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.Messages.Add("Los datos ingresados no son correctos o son nulos");
                return BadRequest(_response);
            }

            var exist = await _repository.ExistCity(model.Id);
            if (!exist)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {model.Id}");
                return BadRequest(_response);
            }

            try
            {
                var city = _mapper.Map<City>(model);

                await _repository.UpdateCity(city);

                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                _response.Messages.Add("se actualizo el registro correctamente");
                _response.Result = city;

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
        [HttpDelete("{id:int}", Name = "DeleteCity")]
        public async Task<IActionResult> Delete(int id)
        {

            var city = await _repository.ExistCity(id);
            if (!city)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                _response.Messages.Add($"No existe un registro con el id {id}");
                return BadRequest(_response);
            }


            var deleted = await _repository.DeleteCity(id);
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
