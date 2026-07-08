# Modernização de Sistema Legado: Cooperativa Financeira Alfa

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=hudsonhenriique_Projeto_Final&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=hudsonhenriique_Projeto_Final) [![Frontend](https://img.shields.io/badge/Frontend-Online-success)](https://projeto-final-sigma-eight.vercel.app/) [![API](https://img.shields.io/badge/API-Railway-blue)](https://projetofinal-production-2e65.up.railway.app)

> ** Ambientes em Produção (Live Demo)**
> * **Interface Web (Frontend):** [Acessar a Aplicação](https://projeto-final-sigma-eight.vercel.app/)
> * **API .NET (Backend):** O servidor está operando no Railway atuando como motor do projeto. O *Base URL* para consumo do Frontend é: `https://projetofinal-production-2e65.up.railway.app`

---

## Visão Geral

Este projeto é uma Prova de Conceito (PoC) desenvolvida para demonstrar como um sistema legado em COBOL pode ser modernizado sem a necessidade de substituí-lo completamente.

A Cooperativa Financeira Alfa possui um sistema confiável e estável responsável pelo cadastro e processamento das informações de seus clientes. Entretanto, esse sistema foi criado em uma época em que aplicações web e integrações modernas ainda não eram uma realidade.

Em vez de substituir esse sistema, a proposta foi criar uma nova camada de acesso, funcionando como um **"balcão de atendimento moderno"**.

Agora, os atendentes podem consultar e atualizar informações de contato dos clientes por meio de uma interface web simples e intuitiva, enquanto o sistema legado continua sendo responsável pelo processamento das informações.

Dessa forma, foi possível unir:
* A robustez do sistema legado;
* A facilidade de uso das aplicações modernas;
* A preparação para futuras integrações;
* A preservação das regras de negócio já existentes.

---

## Tecnologias Utilizadas

### Backend e Integração
* **C# .NET 10:** Desenvolvimento da Web API responsável pela comunicação entre a aplicação web e o sistema legado.
* **COBOL:** Sistema responsável pelo processamento das informações dos clientes e pela persistência dos dados.

### Frontend
* **HTML5, CSS3 e JavaScript:** Tecnologias utilizadas para construir uma interface leve, simples e responsiva.

### DevOps, Qualidade e Infraestrutura
* **Docker:** Conteinerização da aplicação, garantindo consistência entre diferentes ambientes.
* **GitHub Actions:** Automação de build, testes e validações do projeto (CI/CD).
* **SonarCloud:** Análise automática de qualidade, segurança e manutenibilidade do código.

---

## Funcionalidades

A solução permite:
* Consultar um cliente pelo código;
* Visualizar seus dados cadastrais;
* Atualizar telefone e e-mail;
* Informar de forma clara quando um cliente não é encontrado.

*Nota: As funcionalidades de cadastro e exclusão de clientes não fazem parte do escopo deste projeto.*

---

# Arquitetura da Solução

O funcionamento da aplicação pode ser representado pelo fluxo abaixo:

```text
Usuário
   ↓
Frontend (HTML/CSS/JavaScript)
   ↓
API .NET
   ↓
Camada de Integração
   ↓
Sistema Legado COBOL
   ↓
Arquivos de Dados
```

Cada componente possui uma responsabilidade específica:

### Frontend

Responsável por receber as ações do usuário e enviar as solicitações para a API.

### API .NET

Recebe as solicitações, valida as informações e coordena a comunicação com o sistema legado.

### Camada de Integração

Responsável por preparar os dados, executar os programas COBOL e interpretar as respostas recebidas.

### Sistema Legado em COBOL

Responsável pelo processamento das regras de negócio e pelo armazenamento das informações.

---

# Estrutura do Projeto

```text
Projeto_Final/
│
├── .github/             # Configurações de CI/CD
├── api-dotnet/
│   ├── src/             # Código da API
│   └── tests/           # Testes automatizados
├── cobol/               # Programas e arquivos do sistema legado
├── docs/                # Documentação do projeto
├── frontend/            # Interface Web
├── images/              # Evidências utilizadas na documentação
├── shared/              # Estruturas de dados compartilhadas
├── Dockerfile
└── README.md
```

---

# Como Executar o Projeto Localmente

## Pré-requisitos

- .NET SDK 10;
- Compilador COBOL (OpenCOBOL/GnuCOBOL) ou binários já compilados;
- Visual Studio Code;
- Extensão Live Server;
- Docker (opcional).

---

## 1. Clone o repositório

```bash
git clone https://github.com/hudsonhenriique/Projeto_Final.git
cd Projeto_Final
```

---

## 2. Executar a API

```bash
cd api-dotnet/src/ClienteModernizacao.Api
dotnet restore
dotnet build
dotnet run
```

A API ficará disponível em:

```text
https://localhost:7065
```

ou

```text
http://localhost:5169
```

---

## 3. Executar o Frontend

Abra a pasta `frontend` no Visual Studio Code e utilize a extensão **Live Server**.

---

## 4. Executar com Docker (Opcional)

Construir a imagem:

```bash
docker build -t projeto-final .
```

Executar o container:

```bash
docker run -p 8080:8080 projeto-final
```

---

# Testes

O projeto possui:

- Testes funcionais documentados;
- Testes automatizados;
- Integração contínua com GitHub Actions;
- Análise de qualidade utilizando SonarCloud.

Os testes têm como objetivo garantir que futuras alterações não comprometam funcionalidades já implementadas.

---

# Documentação

A pasta `docs` contém:

- Documento de Arquitetura;
- Estrutura Compartilhada;
- Plano de Testes;
- Relatório de Utilização de Inteligência Artificial.

---

# Qualidade e Segurança

O projeto utiliza uma esteira automatizada de validação, incluindo:

- Build automatizado;
- Execução de testes;
- Análise estática de código;
- Verificação de segurança;
- Avaliação de manutenibilidade.

Essas práticas ajudam a garantir que o projeto permaneça estável e preparado para futuras evoluções.

---

# Objetivo do Projeto

Mais do que modernizar uma aplicação, este projeto demonstra como sistemas legados podem continuar gerando valor para as organizações quando integrados a tecnologias modernas.

A solução preserva a estabilidade do negócio, evita riscos de uma substituição completa do sistema existente e prepara a cooperativa para futuras integrações e evoluções tecnológicas.

# Autor

**Hudson Henrique**

- GitHub: https://github.com/hudsonhenriique
- LinkedIn: https://www.linkedin.com/in/hudsonhenriique
