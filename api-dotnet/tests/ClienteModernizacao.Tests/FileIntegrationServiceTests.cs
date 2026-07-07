using System;
using System.IO;
using ClienteModernizacao.Api.Models;
using ClienteModernizacao.Api.Services;
using Xunit;

namespace ClienteModernizacao.Tests
{
    public class FileIntegrationServiceTests
    {
        [Fact]
        public void ConsultarCliente_ReadsExpectedResponseFromCobolFiles()
        {
            string tempRoot = CreateTemporaryCobolStructure();
            try
            {
                var service = new TestableFileIntegrationService(tempRoot);
                string clientId = "000123";
                var request = new ClientQueryRequest { ClientId = clientId };

                string responseLine = "0" + clientId.PadLeft(6, '0') + "JOÃO SILVA".PadRight(30) + "1234567890".PadRight(15) + "email@teste.com".PadRight(40) + "OK".PadRight(50);
                File.WriteAllText(Path.Combine(tempRoot, "cobol", "data", "resp-consulta.txt"), responseLine);

                var response = service.ConsultarCliente(request);

                Assert.Equal("0", response.Status);
                Assert.Equal(clientId, response.ClientId);
                Assert.Equal("JOÃO SILVA", response.Name.Trim());
                Assert.Equal("1234567890", response.Phone.Trim());
                Assert.Equal("email@teste.com", response.Email.Trim());
                Assert.Equal("OK", response.Message.Trim());
            }
            finally
            {
                Directory.Delete(tempRoot, true);
            }
        }

        [Fact]
        public void UpdateClient_ReadsExpectedResponseFromCobolFiles()
        {
            string tempRoot = CreateTemporaryCobolStructure();
            try
            {
                var service = new TestableFileIntegrationService(tempRoot);
                var request = new ClientUpdateRequest
                {
                    ClientId = "000123",
                    NewPhone = "9876543210",
                    NewEmail = "novo@teste.com"
                };

                string responseLine = "0" + "SUCESSO".PadRight(50);
                File.WriteAllText(Path.Combine(tempRoot, "cobol", "data", "resp-atualizacao.txt"), responseLine);

                var response = service.UpdateClient(request);

                Assert.Equal("0", response.Status);
                Assert.Equal("SUCESSO", response.Message.Trim());
            }
            finally
            {
                Directory.Delete(tempRoot, true);
            }
        }

        [Fact]
        public void ConsultarCliente_ThrowsWhenResponseFileMissing()
        {
            string tempRoot = CreateTemporaryCobolStructure();
            try
            {
                var service = new TestableFileIntegrationService(tempRoot);
                var request = new ClientQueryRequest { ClientId = "000123" };

                var exception = Assert.Throws<FileNotFoundException>(() => service.ConsultarCliente(request));
                Assert.Contains("resp-consulta.txt", exception.FileName);
            }
            finally
            {
                Directory.Delete(tempRoot, true);
            }
        }

        private static string CreateTemporaryCobolStructure()
        {
            string tempRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(Path.Combine(tempRoot, "cobol", "data"));
            Directory.CreateDirectory(Path.Combine(tempRoot, "cobol", "src"));
            return tempRoot;
        }

        private class TestableFileIntegrationService : FileIntegrationService
        {
            public TestableFileIntegrationService(string repositoryRoot)
                : base(repositoryRoot)
            {
            }

            protected override void ExecutarCobol(string executavel)
            {
                // No-op during tests
            }
        }
    }
}
