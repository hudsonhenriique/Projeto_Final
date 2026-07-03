# Usa a imagem oficial do .NET 10
FROM mcr.microsoft.com/dotnet/sdk:10.0

# Instala o GnuCOBOL
RUN apt-get update && apt-get install -y gnucobol

# Define a pasta raiz de trabalho
WORKDIR /app

# Copia as pastas necessárias 
COPY ./api-dotnet ./api-dotnet
COPY ./cobol ./cobol
COPY ./shared ./shared

# Remove binários do Windows
RUN rm -f /app/cobol/data/*.dat /app/cobol/data/resp-*.txt

# Compila o COBOL encadeando os comandos
WORKDIR /app/cobol/src
RUN cobc -x -o client-query client-query.cbl && \
    cobc -x -o client-update client-update.cbl && \
    cobc -x -o carga-inicial carga-inicial.cbl && \
    ./carga-inicial

# Volta para a pasta da API
WORKDIR /app/api-dotnet/src/ClienteModernizacao.Api

# Transfere a propriedade dos arquivos para o usuário seguro padrão da Microsoft (app)
RUN chown -R app:app /app

USER app

ENV ASPNETCORE_URLS=http://+:5085
EXPOSE 5085

ENTRYPOINT ["dotnet", "run", "--no-launch-profile"]