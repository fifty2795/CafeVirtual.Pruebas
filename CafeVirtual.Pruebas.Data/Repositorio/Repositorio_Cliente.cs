using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Utilidades.Utilidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using CafeVirtual.Pruebas.Utilidades.Interfaces;

namespace CafeVirtual.Pruebas.Data.Repositorio
{
    public class Repositorio_Cliente : ICliente
    {
        private readonly MvcContext _dbContext;
        private readonly ILogService _logService;

        public Repositorio_Cliente(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task<bool> EliminarClienteVenta(int idCliente)
        {
            try
            {
                var parametro = new SqlParameter("@IdCliente", idCliente);
                await _dbContext.Database.ExecuteSqlRawAsync("EXEC [sp_EliminarClienteVenta] @IdCliente", parametro);
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en EliminarClienteVenta", ex);
                return false;
            }
        }
    }
}
