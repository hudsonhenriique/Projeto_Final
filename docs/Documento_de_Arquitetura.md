# Documento de Arquitetura
## Modernização - Cooperativa Financeira Alfa

---

# 1. Visão Geral do Projeto (Para Negócios)

Imagine que a Cooperativa Financeira Alfa possui um cofre extremamente seguro e tradicional onde estão armazenadas as informações de todos os seus clientes. Esse cofre, representado pelo nosso sistema legado em COBOL, é extremamente confiável, mas possui uma porta de acesso pesada e pouco prática, dificultando que os atendentes consultem informações rapidamente por meio de computadores ou dispositivos modernos.

O objetivo deste projeto não foi substituir esse cofre, pois ele continua funcionando perfeitamente. Nossa proposta foi construir um **"balcão de atendimento moderno"**, representado por uma API desenvolvida em .NET, posicionada à frente do sistema legado.

Quando um atendente precisa consultar os dados de um cliente, ele realiza um pedido simples nesse balcão moderno. O balcão, por sua vez, possui um mensageiro seguro que se comunica com o sistema legado, busca as informações necessárias, traduz os dados para um formato atual e os entrega na tela do usuário em poucos instantes.

Dessa forma, conseguimos unir a modernidade das aplicações web com a robustez do sistema legado, sem interromper as operações da cooperativa e preservando todo o conhecimento de negócio já existente.

---

# 2. Arquitetura Escolhida e Componentes Técnicos

Para implementar essa solução, adotamos uma arquitetura orientada a serviços (**SOA**) baseada em **REST**, separando claramente as responsabilidades de cada componente.

Os principais componentes utilizados foram:

## Frontend (Apresentação)

Uma interface web leve e intuitiva, desenvolvida com:

- HTML;
- CSS;
- JavaScript.

Sua responsabilidade é permitir que o usuário consulte e atualize as informações dos clientes de forma simples.

---

## Backend (O Balcão Moderno - .NET C#)

Uma Web API desenvolvida em **.NET 10**, responsável por:

- receber as requisições HTTP;
- validar as informações recebidas;
- aplicar as regras de segurança;
- coordenar a comunicação com o sistema legado;
- devolver as respostas para o usuário.

---

## Camada de Integração (FileIntegrationService)

É o componente responsável por fazer a ponte entre a aplicação moderna e o sistema legado.

Suas responsabilidades são:

- preparar os dados de entrada;
- executar os programas COBOL;
- ler os arquivos gerados pelo processamento;
- converter as informações para um formato compreensível pela API.

---

## Processamento Core (Sistema Legado em COBOL)

Representa o núcleo do negócio da cooperativa.

É responsável pelo processamento das informações cadastrais dos clientes e pela execução das regras de negócio já consolidadas ao longo dos anos.

A principal decisão arquitetural foi manter esse sistema preservado, aproveitando sua estabilidade e confiabilidade.

---

# 3. Fluxo de Execução da Solução

O funcionamento da solução segue o seguinte fluxo:

1. O usuário informa o código do cliente na interface web;
2. O Frontend envia uma requisição HTTP para a API;
3. A API recebe a solicitação e aciona a camada de integração;
4. A camada de integração prepara os dados e executa o programa COBOL;
5. O sistema legado processa a solicitação e gera o resultado;
6. A API interpreta os dados recebidos;
7. As informações são convertidas para o formato JSON;
8. O resultado é devolvido ao Frontend;
9. O usuário visualiza as informações na tela.

---

# 4. Desafios Enfrentados e Justificativas Técnicas

Durante o desenvolvimento da solução, algumas decisões foram necessárias para garantir que o sistema fosse confiável, organizado e preparado para futuras evoluções.

## Integração Desacoplada

Optamos por uma comunicação baseada na execução de processos e troca de arquivos.

Essa abordagem foi escolhida porque permite que o sistema legado permaneça isolado e continue funcionando sem necessidade de reescrita ou alterações profundas.

Além disso, a solução reduz riscos e facilita futuras manutenções.

---

## Resiliência e Tratamento de Falhas

A aplicação foi desenvolvida considerando cenários de erro e indisponibilidade de informações.

Por esse motivo:

- valores padrão foram definidos para situações inesperadas;
- as configurações da aplicação foram centralizadas;
- foram implementados mecanismos de proteção para evitar interrupções em produção.

Essas medidas aumentam a estabilidade e a confiabilidade da solução.

---

## Qualidade Contínua (CI/CD)

Além dos testes manuais, foi implementado um processo automatizado de validação utilizando:

- GitHub Actions;
- SonarCloud.

Essas ferramentas realizam verificações automáticas de:

- qualidade do código;
- segurança;
- manutenibilidade;
- boas práticas de desenvolvimento.

Com isso, cada alteração realizada no projeto passa por um processo de validação antes de ser considerada apta para utilização.

---

# 5. Benefícios da Solução

A arquitetura adotada traz diversos benefícios para a Cooperativa Financeira Alfa:

- preserva o investimento realizado no sistema legado;
- moderniza o acesso às informações;
- facilita futuras integrações;
- reduz riscos de migração;
- melhora a experiência dos usuários;
- mantém a estabilidade e a segurança do processamento existente.

---

# 6. Limitações da Solução

A solução foi desenvolvida de acordo com os requisitos definidos para o projeto e contempla as seguintes funcionalidades:

- consulta de clientes;
- visualização dos dados cadastrais;
- atualização de telefone e e-mail.

As funcionalidades de **cadastro** e **exclusão de clientes** não foram implementadas, pois não faziam parte do escopo definido para o projeto. Em um ambiente corporativo real, essas operações normalmente exigem processos adicionais de validação, auditoria e regras de negócio específicas.

---

# 7. Conclusão

Este projeto demonstrou que é possível modernizar um sistema legado sem abrir mão de sua confiabilidade.

Ao criar uma camada moderna de integração, foi possível unir:

- a robustez do COBOL;
- a flexibilidade das aplicações web;
- a facilidade de manutenção;
- a preparação para futuras integrações.

Mais do que uma atualização tecnológica, a solução representa uma estratégia de evolução gradual, segura e sustentável para os sistemas da Cooperativa Financeira Alfa.