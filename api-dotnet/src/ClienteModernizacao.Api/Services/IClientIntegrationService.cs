using ClienteModernizacao.Api.Models;

namespace ClienteModernizacao.Api.Services
{
    public interface IClientIntegrationService
    {
        ClientQueryResponse ConsultarCliente(ClientQueryRequest request);
        ClientUpdateResponse UpdateClient(ClientUpdateRequest request);
    }
}
