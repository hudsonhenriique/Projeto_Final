namespace ClienteModernizacao.Api.Models
{
    public class ClientUpdateRequest
    {
        public string ClientId { get; set; } = string.Empty;
        public string NewPhone { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
    }
}