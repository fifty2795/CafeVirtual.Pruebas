using AutoMapper;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using CafeVirtual.Pruebas.API.Services.DTO;
using CafeVirtual.Pruebas.API.Services.Interfaces;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;
using CafeVirtual.Pruebas.API.Utilidades.Response;
using CafeVirtual.Pruebas.Data.Models;

namespace CafeVirtual.Pruebas.API.Services.Services
{
    public class ProductoService: IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<TblProducto> _repositorio;
        private readonly ILogService _logService;

        public ProductoService(IUnitOfWork unitOfWork, IMapper mapper, ILogService logService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _repositorio = _unitOfWork.Repository<TblProducto>();
            _logService = logService;
        }

        public async Task<ResponseViewModel<List<ProductoDTO>>> ObtenerProducto(string? busqueda)
        {
            try
            {
                var producto = await _unitOfWork.RepositorioProducto.ObtenerProducto(busqueda);

                if (producto == null)
                {
                    return ResponseHelper.CrearRespuestaError<List<ProductoDTO>>("No se encontro ningun producto.", 401);
                }

                var productoEntity = _mapper.Map<List<ProductoDTO>>(producto);

                return ResponseHelper.CrearRespuestaExito(productoEntity, "Producto obtenido exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerProducto", ex);
                return ResponseHelper.CrearRespuestaError<List<ProductoDTO>>("Error al obtener el producto.", 500);
            }
        }

        public async Task<ResponseViewModel<ProductoDTO>> ObtenerProductoById(int idProducto)
        {
            try
            {
                if (idProducto == 0)
                {
                    return ResponseHelper.CrearRespuestaError<ProductoDTO>("Ingrese un Id de Producto.", 400);
                }                

                var producto = await _repositorio.ObtenerByIdAsync(p => p.IdProducto == idProducto, p => p.IdProveedorNavigation);

                if (producto == null)
                {
                    return ResponseHelper.CrearRespuestaError<ProductoDTO>("No se encontro ningun producto.", 401);
                }

                var productoEntity = _mapper.Map<ProductoDTO>(producto);

                return ResponseHelper.CrearRespuestaExito(productoEntity, "Producto obtenido exitosamente.");                
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerProductoById", ex);
                return ResponseHelper.CrearRespuestaError<ProductoDTO>("Error al obtener el producto.", 500);
            }
        }

        public async Task<ResponseViewModel<ProductoDTO>> AgregarProducto(TblProducto producto)
        {
            try
            {
                if (producto == null)
                {
                    return ResponseHelper.CrearRespuestaError<ProductoDTO>("Favor de ingresar la informacion del producto.", 401);
                }

                producto.FechaCreacion = DateTime.Now;

                await _repositorio.AgregarAsync(producto);

                await _unitOfWork.SaveChangesAsync();

                var productoAgregado = await _repositorio.ObtenerByIdAsync(p => p.IdProducto == producto.IdProducto, p => p.IdProveedorNavigation);

                var productoEntity = _mapper.Map<ProductoDTO>(productoAgregado);

                return ResponseHelper.CrearRespuestaExito(productoEntity, "Producto agregado exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en AgregarProducto", ex);
                return ResponseHelper.CrearRespuestaError<ProductoDTO>("Error al agregar el producto.", 500);
            }
        }

        public async Task<ResponseViewModel<ProductoDTO>> EditarProducto(TblProducto producto)
        {
            try
            {
                if (producto == null)
                {
                    return ResponseHelper.CrearRespuestaError<ProductoDTO>("Favor de ingresar la informacion del producto.", 401);
                }

                await _unitOfWork.RepositorioProducto.ActualizarProducto(producto);                

                var productoActualizado = await _repositorio.ObtenerByIdAsync(x => x.IdProducto == producto.IdProducto, x => x.IdProveedorNavigation);

                var productoEntity = _mapper.Map<ProductoDTO>(productoActualizado);                

                return ResponseHelper.CrearRespuestaExito(productoEntity, "Producto actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en EditarProducto", ex);
                return ResponseHelper.CrearRespuestaError<ProductoDTO>("Error al actualizar el producto.", 500);
            }
        }

        public async Task<ResponseViewModel<bool>> EliminarProducto(int idProducto)
        {
            try
            {
                var productoEliminado = await _unitOfWork.RepositorioProducto.EliminarProducto(idProducto);

                if (!productoEliminado)
                {
                    return ResponseHelper.CrearRespuestaError<bool>("Error al eliminar el producto.", 400);
                }                

                return ResponseHelper.CrearRespuestaExito(true, "Producto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en EliminarProducto", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Error al eliminar el producto.", 500);
            }
        }
    }
}
