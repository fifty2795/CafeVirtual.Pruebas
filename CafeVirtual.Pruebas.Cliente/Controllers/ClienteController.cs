using Microsoft.AspNetCore.Mvc;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CafeVirtual.Pruebas.Data.UnitOfWork;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Business.DTO;

namespace CafeVirtual.Pruebas.Cliente.Controllers
{
    //[Authorize(Roles = "Administrador,Usuario")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _cliente;

        public ClienteController(IClienteService cliente)
        {            
            _cliente = cliente;
        }

        public async Task<IActionResult> Index(string? txtBusqueda, int pageNumber)
        {
            int pageSize = 5;

            ViewBag.Busqueda = txtBusqueda;

            if (string.IsNullOrEmpty(txtBusqueda))
            {
                txtBusqueda = string.Empty;
            }

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            var response = await _cliente.ObtenerClientes(txtBusqueda, pageNumber, pageSize);

            return View(response);            
        }

        public IActionResult AgregarCliente()
        {
            var response = new ResponseViewModel<TblCliente>();

            response.Data = new TblCliente();

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarCliente(TblCliente cliente)
        {
            var response = await _cliente.AgregarCliente(cliente);

            return Json(response);
        }

        public async Task<IActionResult> EditarCliente(int id)
        {
            var response = await _cliente.ObtenerClienteById(id);

            ViewBag.IdCliente = id;

            return View(response);
        }

        [HttpPost]

        public async Task<IActionResult> EditarCliente(TblCliente cliente)
        {
            var response = await _cliente.ActualizarCliente(cliente);

            return Json(response);
        }

        public async Task<IActionResult> EliminarCliente(int id)
        {
            var response = await _cliente.ObtenerClienteById(id);            

            ViewBag.IdCliente = id;

            return View(response);
        }

        public async Task<IActionResult> EliminarClienteConfirm(int idCliente)
        {
            var response = await _cliente.EliminarClienteVenta(idCliente);

            return Json(response);
        }        
    }
}
