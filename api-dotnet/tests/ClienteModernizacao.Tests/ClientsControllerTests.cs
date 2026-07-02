using Microsoft.AspNetCore.Mvc;
using ClienteModernizacao.Api.Controllers;
using ClienteModernizacao.Api.Models;
using ClienteModernizacao.Api.Services;
using Xunit;

namespace ClienteModernizacao.Tests
{
    public class ClientsControllerTests
    {
        private readonly ClientsController _controller;

        public ClientsControllerTests()
        {
            var integrationService = new FakeClientIntegrationService();
            _controller = new ClientsController(integrationService);
        }

        [Fact]
        public void Consult_ExistingClient_ReturnsOkResult()
        {
            string clientId = "000001";

            var result = _controller.Consultar(clientId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ClientQueryResponse>(okResult.Value);
            
            Assert.Equal("0", response.Status);
            Assert.Equal("HUDSON HENRIQUE", response.Name);
        }

        [Fact]
        public void Consult_NonExistingClient_ReturnsNotFound()
        {
            string clientId = "999999";

            var result = _controller.Consultar(clientId);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ClientQueryResponse>(notFoundResult.Value);
            
            Assert.Equal("1", response.Status);
            Assert.Contains("NOT FOUND", response.Message.ToUpper());
        }

        [Fact]
        public void Update_ValidClient_ReturnsOkResult()
        {
            var updateRequest = new ClientUpdateRequest
            {
                ClientId = "000002",
                NewPhone = "31999998888",
                NewEmail = "marilaine.nova@email.com"
            };

            var result = _controller.Atualizar(updateRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ClientUpdateResponse>(okResult.Value);
            
            Assert.Equal("0", response.Status);
            Assert.Contains("SUCESSO", response.Message.ToUpper());
        }

        [Fact]
        public void Update_NonExistingClient_ReturnsBadRequest()
        {
            var updateRequest = new ClientUpdateRequest
            {
                ClientId = "999999",
                NewPhone = "31999998888",
                NewEmail = "cliente.inexistente@email.com"
            };

            var result = _controller.Atualizar(updateRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ClientUpdateResponse>(badRequestResult.Value);

            Assert.Equal("1", response.Status);
            Assert.Contains("NAO ENCONTRADO", response.Message.ToUpper());
        }

        [Fact]
        public void Consult_EmptyClientId_ReturnsNotFound()
        {
            var result = _controller.Consultar(string.Empty);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ClientQueryResponse>(notFoundResult.Value);

            Assert.Equal("1", response.Status);
            Assert.Contains("NOT FOUND", response.Message.ToUpper());
        }

        [Fact]
        public void Consult_NullClientId_ReturnsNotFound()
        {
            var result = _controller.Consultar(null!);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ClientQueryResponse>(notFoundResult.Value);

            Assert.Equal("1", response.Status);
            Assert.Contains("NOT FOUND", response.Message.ToUpper());
        }

        [Fact]
        public void Update_EmptyFields_ReturnsBadRequest()
        {
            var updateRequest = new ClientUpdateRequest
            {
                ClientId = "000003",
                NewPhone = string.Empty,
                NewEmail = string.Empty
            };

            var result = _controller.Atualizar(updateRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ClientUpdateResponse>(badRequestResult.Value);

            Assert.Equal("1", response.Status);
            Assert.Contains("NAO ENCONTRADO", response.Message.ToUpper());
        }
    }
}