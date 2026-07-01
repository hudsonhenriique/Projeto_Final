using Microsoft.AspNetCore.Mvc;
using ClienteModernizacao.Api.Models;
using ClienteModernizacao.Api.Services;

namespace ClienteModernizacao.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ClientsController : ControllerBase
    {
        private readonly FileIntegrationService _integrationService;

        // O C# injeta o servico automaticamente aqui
        public ClientsController(FileIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [HttpGet("{id}")]
        public IActionResult Consultar(string id)
        {
            var request = new ClientQueryRequest { ClientId = id };
            var response = _integrationService.ConsultarCliente(request);

            if (response.Status == "1")
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut]
        public IActionResult Atualizar([FromBody] ClientUpdateRequest request)
        {
            var response = _integrationService.UpdateClient(request);

            if (response.Status != "0")
                return BadRequest(response);

            return Ok(response);
        }
    }
}