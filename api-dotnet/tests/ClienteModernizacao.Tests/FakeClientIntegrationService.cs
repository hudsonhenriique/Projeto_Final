using ClienteModernizacao.Api.Models;
using ClienteModernizacao.Api.Services;

namespace ClienteModernizacao.Tests
{
    public class FakeClientIntegrationService : IClientIntegrationService
    {
        public ClientQueryResponse ConsultarCliente(ClientQueryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientId) || request.ClientId == "999999")
            {
                return new ClientQueryResponse
                {
                    Status = "1",
                    Message = "NOT FOUND"
                };
            }

            return new ClientQueryResponse
            {
                Status = "0",
                ClientId = request.ClientId.PadLeft(6, '0'),
                Name = "HUDSON HENRIQUE",
                Phone = "31999998888",
                Email = "hudson@email.com",
                Message = "OK"
            };
        }

        public ClientUpdateResponse UpdateClient(ClientUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientId) || string.IsNullOrWhiteSpace(request.NewPhone) || string.IsNullOrWhiteSpace(request.NewEmail) || request.ClientId == "999999")
            {
                return new ClientUpdateResponse
                {
                    Status = "1",
                    Message = "CLIENTE NAO ENCONTRADO NO BANCO DE DADOS"
                };
            }

            return new ClientUpdateResponse
            {
                Status = "0",
                Message = "CLIENTE ATUALIZADO COM SUCESSO"
            };
        }
    }
}
