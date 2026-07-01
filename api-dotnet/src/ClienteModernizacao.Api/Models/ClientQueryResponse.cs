namespace ClienteModernizacao.Api.Models
{
    // espelho do resp-consulta.cpy
    public class ClientQueryResponse
    {
        public string Status { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}