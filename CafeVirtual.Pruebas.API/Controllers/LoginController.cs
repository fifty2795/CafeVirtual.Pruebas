using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CafeVirtual.Pruebas.API.Interfaces;
using CafeVirtual.Pruebas.API.Models;
using CafeVirtual.Pruebas.API.Services.DTO;

namespace CafeVirtual.Pruebas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;

        public LoginController(ILoginService loginService, IMapper mapper)
        {
            _loginService = loginService;
            _mapper = mapper;
        }       

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginDto = _mapper.Map<LoginDTO>(model);

            var result = await _loginService.Login(loginDto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
