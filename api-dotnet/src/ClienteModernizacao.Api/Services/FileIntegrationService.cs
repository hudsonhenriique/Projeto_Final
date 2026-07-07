using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using ClienteModernizacao.Api.Models;

namespace ClienteModernizacao.Api.Services
{
    public class FileIntegrationService : IClientIntegrationService
    {
        private const string CobolFolderName = "cobol";
        
        private readonly string _cobolDataFolder;
        private readonly string _cobolSrcFolder;

        public FileIntegrationService()
        {
            string repositoryRoot = ResolveRepositoryRoot();
            _cobolDataFolder = Path.Combine(repositoryRoot, CobolFolderName, "data");
            _cobolSrcFolder = Path.Combine(repositoryRoot, CobolFolderName, "src");
        }

        public ClientQueryResponse ConsultarCliente(ClientQueryRequest request)
        {
            // Operacao '0' (Consulta) + Codigo de 6 posicoes com zeros a esquerda
            string idFormatado = request.ClientId.PadLeft(6, '0');
            string linhaRequisicao = $"0{idFormatado}";

            string reqPath = Path.Combine(_cobolDataFolder, "req-consulta.txt");
            File.WriteAllText(reqPath, linhaRequisicao);

            ExecutarCobol(ObterNomeExecutavel("client-query"));

            string respPath = Path.Combine(_cobolDataFolder, "resp-consulta.txt");
            
            if (!File.Exists(respPath))
            {
                throw new FileNotFoundException("O COBOL nao gerou o arquivo de resposta.", respPath);
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

            ExecutarCobol(ObterNomeExecutavel("client-update"));

            string respPath = Path.Combine(_cobolDataFolder, "resp-atualizacao.txt");
            
            if (!File.Exists(respPath))
            {
                throw new FileNotFoundException("O COBOL nao gerou o arquivo de resposta de atualizacao.", respPath);
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

        private static string ObterNomeExecutavel(string nomeBase)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{nomeBase}.exe" : nomeBase;
        }

        private static string ResolveRepositoryRoot()
        {
            string? cobolRootOverride = Environment.GetEnvironmentVariable("COBOL_ROOT");
            if (!string.IsNullOrEmpty(cobolRootOverride))
            {
                var root = Path.GetFullPath(cobolRootOverride);
                string dataPath = Path.Combine(root, CobolFolderName, "data");
                string srcPath = Path.Combine(root, CobolFolderName, "src");

                if (Directory.Exists(dataPath) && Directory.Exists(srcPath))
                {
                    return root;
                }
            }

            string[] candidates = { AppContext.BaseDirectory, Environment.CurrentDirectory, "/app", "/usr/src/app" };

            foreach (var candidate in candidates)
            {
                if (string.IsNullOrWhiteSpace(candidate))
                {
                    continue;
                }

                var current = new DirectoryInfo(candidate);

                while (current != null)
                {
                    string dataPath = Path.Combine(current.FullName, CobolFolderName, "data");
                    string srcPath = Path.Combine(current.FullName, CobolFolderName, "src");

                    if (Directory.Exists(dataPath) && Directory.Exists(srcPath))
                    {
                        return current.FullName;
                    }

                    current = current.Parent;
                }
            }

            throw new DirectoryNotFoundException("Não foi possível localizar a pasta do repositório com os arquivos COBOL. Verifique se /app/cobol está presente no container ou defina a variável de ambiente COBOL_ROOT.");
        }
    }
}