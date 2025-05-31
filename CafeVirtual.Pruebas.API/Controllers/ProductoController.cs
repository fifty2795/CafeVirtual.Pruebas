using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CafeVirtual.Pruebas.API.Models;
using CafeVirtual.Pruebas.API.Services.DTO;
using CafeVirtual.Pruebas.API.Services.Interfaces;
using CafeVirtual.Pruebas.Data.Models;

namespace CafeVirtual.Pruebas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly IMapper _mapper;        

        public ProductoController(IProductoService productoService, IMapper mapper)
        {
            _productoService = productoService;
            _mapper = mapper;            
        }

        [HttpGet("obtenerProducto")]
        public async Task<IActionResult> ObtenerProducto([FromQuery] FiltroProductoViewModel filtro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _productoService.ObtenerProducto(filtro.Busqueda);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("obtenerProductoById")]
        public async Task<IActionResult> ObtenerProductoById([FromQuery] int? idProducto)
        {
            if (!idProducto.HasValue)
                return BadRequest("El parámetro idProducto es obligatorio.");

            if (idProducto <= 0)
                return BadRequest("El parámetro idProducto debe ser mayor que cero.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productoService.ObtenerProductoById(idProducto.Value);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("agregarProducto")]
        public async Task<IActionResult> AgregarProducto([FromBody] ProductoViewModel productoViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = _mapper.Map<TblProducto>(productoViewModel);

            var result = await _productoService.AgregarProducto(producto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("editarProducto")]
        public async Task<IActionResult> EditarProducto([FromBody] ProductoViewModel productoViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var producto = _mapper.Map<TblProducto>(productoViewModel);

            var result = await _productoService.EditarProducto(producto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("eliminarProducto")]
        public async Task<IActionResult> EliminarProducto([FromQuery] int idProducto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);            

            var result = await _productoService.EliminarProducto(idProducto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }        
    }
}
