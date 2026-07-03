# Usa a imagem oficial do .NET 10
FROM mcr.microsoft.com/dotnet/sdk:10.0

# Instala o GnuCOBOL no ambiente Linux do container
RUN apt-get update && apt-get install -y gnucobol

# Define a pasta raiz de trabalho
WORKDIR /app

# Copia todo o projeto para o container
COPY . .

# Deleta os arquivos binarios e de resposta copiados do Windows para forçar a criação no padrão Linux
RUN rm -f /app/cobol/data/*.dat /app/cobol/data/resp-*.txt

# Entra na pasta do COBOL, compila as APIs e a Carga Inicial
WORKDIR /app/cobol/src
RUN cobc -x -o client-query client-query.cbl
RUN cobc -x -o client-update client-update.cbl
RUN cobc -x -o carga-inicial carga-inicial.cbl

# Executa a carga inicial para criar o banco de dados nativo do Linux
RUN ./carga-inicial

# Volta para a pasta da API
WORKDIR /app/api-dotnet/src/ClienteModernizacao.Api

# Força o C# a escutar o tráfego externo
ENV ASPNETCORE_URLS=http://+:5085
EXPOSE 5085

# Inicia a API ignorando o 'launchSettings.json' do Windows
ENTRYPOINT ["dotnet", "run", "--no-launch-profile"]