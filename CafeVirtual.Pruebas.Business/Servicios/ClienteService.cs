using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Utilidades.Interfaces;

namespace CafeVirtual.Pruebas.Business.Servicios
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IRepository<TblCliente> _repositorio;
        private readonly ILogService _logService;

        public ClienteService(IUnitOfWork unitOfWork, ILogService logService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));            
            _repositorio = _unitOfWork.Repository<TblCliente>();
            _logService = logService;
        }

        public async Task<ResponseViewModel<List<TblCliente>>> ObtenerClientes()
        {
            try
            {
                //var clientes = _repositorio.ObtenerQueryable().Where(p => p.Activo).OrderBy(p => p.Nombre).ToListAsync();                

                var query = _repositorio.ObtenerQueryable();

                query = query.Where(x => x.Activo == true);

                query = query.OrderBy(x => x.Nombre);

                var clientes = await query.ToListAsync();

                return ResponseHelper.CrearRespuestaExito(clientes, "Clientes obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerClientes", ex);
                return ResponseHelper.CrearRespuestaError<List<TblCliente>>("Hubo un error al obtener los clientes. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<PaginatedList<TblCliente>>> ObtenerClientes(string? busqueda, int pageNumber, int pageSize)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                //query = query.Where(p => p.Activo);

                // Filtro por otros campos
                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    query = query.Where(p => p.Nombre.Contains(busqueda) || p.Descripcion.Contains(busqueda) || p.Rfc.Contains(busqueda));
                }

                query = query.OrderBy(c => c.Nombre);

                var clientesPaginados = await PaginatedList<TblCliente>.CreateAsync(query, pageNumber, pageSize);

                return ResponseHelper.CrearRespuestaExito(clientesPaginados, "Clientes obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerClientes con busqueda", ex);
                return ResponseHelper.CrearRespuestaError<PaginatedList<TblCliente>>("Error al obtener los clientes filtrados.");
            }
        }

        public async Task<ResponseViewModel<TblCliente>> ObtenerClienteById(int idCliente)
        {
            try
            {                
                var cliente = await _repositorio.ObtenerByIdAsync(idCliente);

                if (cliente == null)
                {
                    return new ResponseViewModel<TblCliente>
                    {
                        Success = false,
                        Message = "Cliente no encontrado."
                    };
                }

                return ResponseHelper.CrearRespuestaExito(cliente, "Cliente obtenido exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerClienteById", ex);
                return ResponseHelper.CrearRespuestaError<TblCliente>("Hubo un error al obtener el cliente. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblCliente>> AgregarCliente(TblCliente cliente)
        {
            if (cliente == null)
            {
                return new ResponseViewModel<TblCliente>
                {
                    Success = false,
                    Message = "El cliente no puede ser nulo."
                };
            }

            try
            {
                cliente.FechaCreacion = DateTime.Now;
                var nuevoCliente = await _repositorio.AgregarAsync(cliente);

                await _unitOfWork.SaveChangesAsync();

                return ResponseHelper.CrearRespuestaExito(nuevoCliente, "Cliente agregado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en AgregarCliente", ex);
                return ResponseHelper.CrearRespuestaError<TblCliente>("Hubo un error al agregar el cliente. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblCliente>> ActualizarCliente(TblCliente cliente)
        {
            if (cliente == null)
            {
                return new ResponseViewModel<TblCliente>
                {
                    Success = false,
                    Message = "El cliente no puede ser nulo."
                };
            }

            try
            {
                var clienteActualizado = _repositorio.Actualizar(cliente, nameof(TblCliente.FechaCreacion));

                await _unitOfWork.SaveChangesAsync();

                return ResponseHelper.CrearRespuestaExito(clienteActualizado, "Cliente actualizado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ActualizarCliente", ex);
                return ResponseHelper.CrearRespuestaError<TblCliente>("Hubo un error al actualizar el cliente. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblCliente>> EliminarCliente(int idCliente)
        {
            try
            {
                var clienteEliminado = await _repositorio.EliminarAsync(idCliente);                
                
                if (clienteEliminado == null)
                {
                    return new ResponseViewModel<TblCliente>
                    {
                        Success = false,
                        Message = "No se pudo eliminar el cliente, no existe."
                    };
                }

                await _unitOfWork.SaveChangesAsync();

                return ResponseHelper.CrearRespuestaExito(clienteEliminado, "Cliente eliminado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en EliminarCliente", ex);
                return ResponseHelper.CrearRespuestaError<TblCliente>("Hubo un error al eliminar el cliente. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<bool>> EliminarClienteVenta(int idCliente)
        {
            try
            {
                var clienteEliminado = await _unitOfWork.RepositorioCliente.EliminarClienteVenta(idCliente);

                if (!clienteEliminado)
                {
                    return new ResponseViewModel<bool>
                    {
                        Success = false,
                        Message = "Error al eliminar el cliente."
                    };
                }

                return ResponseHelper.CrearRespuestaExito(clienteEliminado, "Cliente eliminado con éxito.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en EliminarClienteVenta", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Error al eliminar el cliente.");
            }
        }
    }
}
