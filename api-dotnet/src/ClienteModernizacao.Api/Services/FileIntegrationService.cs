using System;
using System.Diagnostics;
using System.IO;
using ClienteModernizacao.Api.Models;

namespace ClienteModernizacao.Api.Services
{
    public class FileIntegrationService
    {
        // O C# roda dentro da pasta do projeto, entao voltamos 3 niveis (..\..\) para chegar na raiz do monorepo
        private readonly string _cobolDataFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "cobol", "data"));
        private readonly string _cobolSrcFolder = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "cobol", "src"));

        public ClientQueryResponse ConsultarCliente(ClientQueryRequest request)
        {
            // Operacao '0' (Consulta) + Codigo de 6 posicoes com zeros a esquerda
            string idFormatado = request.ClientId.PadLeft(6, '0');
            string linhaRequisicao = $"0{idFormatado}";

            string reqPath = Path.Combine(_cobolDataFolder, "req-consulta.txt");
            File.WriteAllText(reqPath, linhaRequisicao);

            ExecutarCobol("client-query.exe");

            string respPath = Path.Combine(_cobolDataFolder, "resp-consulta.txt");
            
            if (!File.Exists(respPath))
            {
                throw new Exception("O COBOL nao gerou o arquivo de resposta.");
            }

            string linhaResposta = File.ReadAllText(respPath).PadRight(142, ' ');

            // O Substring recorta o texto com base nas posicoes e tamanhos do Copybook (resp-consulta.cpy)
            return new ClientQueryResponse
            {
                Status = linhaResposta.Substring(0, 1).Trim(),
                ClientId = linhaResposta.Substring(1, 6).Trim(),
                Name = linhaResposta.Substring(7, 30).Trim(),
                Phone = linhaResposta.Substring(37, 15).Trim(),
                Email = linhaResposta.Substring(52, 40).Trim(),
                Message = linhaResposta.Substring(92, 50).Trim()
            };
        }

        public ClientUpdateResponse UpdateClient(ClientUpdateRequest request)
        {
            // Operacao 'A' + ID(6) + Telefone(15) + Email(40)
            string idFormatado = request.ClientId.PadLeft(6, '0');
            string telefoneFormatado = request.NewPhone.PadRight(15, ' ');
            string emailFormatado = request.NewEmail.PadRight(40, ' ');
            
            string linhaRequisicao = $"A{idFormatado}{telefoneFormatado}{emailFormatado}";

            string reqPath = Path.Combine(_cobolDataFolder, "req-atualizacao.txt");
            File.WriteAllText(reqPath, linhaRequisicao);

            ExecutarCobol("client-update.exe");

            string respPath = Path.Combine(_cobolDataFolder, "resp-atualizacao.txt");
            
            if (!File.Exists(respPath))
            {
                throw new Exception("O COBOL nao gerou o arquivo de resposta de atualizacao.");
            }

            string linhaResposta = File.ReadAllText(respPath).PadRight(51, ' ');

            // Retorna o Model C# (Status de 1 byte + Mensagem de 50 bytes)
            return new ClientUpdateResponse
            {
                Status = linhaResposta.Substring(0, 1).Trim(),
                Message = linhaResposta.Substring(1, 50).Trim()
            };
        }

        private void ExecutarCobol(string executavel)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_cobolSrcFolder, executavel),
                WorkingDirectory = _cobolSrcFolder, // Diz pro executavel que ele esta rodando dentro da pasta src
                UseShellExecute = false,
                CreateNoWindow = true // Roda sem piscar a tela preta do DOS
            };

            using (var process = Process.Start(processInfo))
            {
                process?.WaitForExit(); 
            }
        }
    }
}