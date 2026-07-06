# Relatório de Utilização de Inteligência Artificial
## Modernização - Cooperativa Financeira Alfa

---

# Introdução

Durante o desenvolvimento da solução de integração entre a API em .NET e o sistema legado em COBOL, a Inteligência Artificial foi utilizada como uma ferramenta de apoio técnico e consulta.

Seu papel não foi desenvolver a solução de forma automática, mas auxiliar na compreensão de problemas, na análise de alternativas e na tomada de decisões relacionadas à segurança, infraestrutura e boas práticas de desenvolvimento.

A cada interação, as respostas fornecidas pela IA foram analisadas criticamente antes de serem aplicadas ao projeto.

A seguir estão documentadas as três principais utilizações da Inteligência Artificial durante o desenvolvimento.

---

# Interação 1: Resolução de Vulnerabilidade de Segurança (CORS)

## Objetivo do Prompt

Resolver um alerta de segurança apontado pelo SonarCloud referente a uma configuração muito permissiva da API, sem prejudicar o ambiente de desenvolvimento local.

## Prompt Utilizado

> "Apareceu um erro no SonarCloud: 'Make sure this permissive CORS policy is safe here'. Mas fazendo a restrição apenas para o localhost, terei problemas depois no deploy?"

## Resposta Obtida

A Inteligência Artificial explicou o funcionamento do mecanismo de segurança responsável por controlar quais aplicações podem acessar a API.

Como solução, sugeriu uma configuração dinâmica, permitindo que a aplicação utilize um endereço diferente em cada ambiente. Em desenvolvimento, a API continuaria aceitando requisições locais e, em produção, utilizaria o endereço configurado no servidor.

## Análise Crítica

A resposta foi bastante relevante, pois manter a API aberta para qualquer origem representa um risco de segurança, especialmente em sistemas que manipulam dados financeiros.

A solução proposta permitiu aumentar a segurança sem prejudicar o desenvolvimento e tornou a aplicação preparada para futuras publicações em ambientes de produção.

## Impacto na Solução

A configuração da aplicação foi ajustada para utilizar arquivos de configuração e variáveis de ambiente, permitindo maior flexibilidade e reduzindo riscos relacionados à segurança.

---

# Interação 2: Estratégia de DevOps e Qualidade do Código

## Objetivo do Prompt

Entender por que a esteira de integração contínua estava sendo reprovada, mesmo sem apresentar falhas de segurança ou problemas de código.

## Prompt Utilizado

> "O que aconteceu? O Quality Gate falhou pedindo 80% de cobertura no New Code, mas não temos issues abertas."

## Resposta Obtida

A Inteligência Artificial explicou que a ferramenta estava exigindo testes automatizados para arquivos responsáveis apenas pela configuração e inicialização da aplicação.

Também apresentou alternativas para ajustar as regras de validação, permitindo que a cobertura de testes fosse concentrada nas partes que realmente possuem regras de negócio.

## Análise Crítica

A explicação foi importante para compreender que nem todos os arquivos de um projeto precisam ser testados da mesma maneira.

A recomendação ajudou a direcionar os esforços para os pontos mais importantes da aplicação, mantendo o foco na qualidade sem gerar trabalho desnecessário.

## Impacto na Solução

A configuração da plataforma foi ajustada para desconsiderar arquivos de inicialização na análise de cobertura de testes.

Com isso, a esteira de integração contínua foi aprovada e os indicadores de qualidade permaneceram focados em segurança, manutenibilidade e boas práticas de desenvolvimento.

---

# Interação 3: Resolução de Conflito com o Sistema Operacional

## Objetivo do Prompt

Investigar um erro que impedia a execução do programa responsável pela atualização de clientes.

## Prompt Utilizado

> "O teste final deu erro. System.ComponentModel.Win32Exception (740): An error occurred trying to start process 'client-update.exe'. A operação requer elevação."

## Resposta Obtida

A Inteligência Artificial explicou que o problema não estava no código da aplicação, mas em uma proteção do próprio sistema operacional.

Em determinadas situações, o Windows pode interpretar alguns nomes de arquivos executáveis como programas de instalação ou atualização e exigir permissões administrativas para executá-los.

Também foram apresentadas alternativas para contornar o problema, como executar o ambiente com privilégios elevados ou utilizar um nome diferente para o arquivo.

## Análise Crítica

A resposta forneceu uma visão importante sobre o comportamento do sistema operacional, algo que dificilmente seria identificado apenas analisando o código.

Além de resolver o problema imediato, a explicação contribuiu para um melhor planejamento de futuras implantações da solução.

## Impacto na Solução

Para fins de homologação do projeto, a aplicação foi executada com privilégios administrativos, permitindo validar toda a integração.

Como aprendizado arquitetural, ficou definido que, em um ambiente produtivo, os executáveis deverão utilizar nomes mais neutros, evitando possíveis bloqueios ou restrições do sistema operacional.

---

# Conclusão

A utilização da Inteligência Artificial foi além da geração de código.

Ela atuou como uma ferramenta de apoio para pesquisa, validação de hipóteses e compreensão de problemas relacionados à infraestrutura, segurança e arquitetura da aplicação.

Todas as sugestões recebidas foram avaliadas criticamente antes de serem aplicadas ao projeto.

Como resultado, o desenvolvimento tornou-se mais consciente e fundamentado, permitindo que cada decisão técnica fosse compreendida, justificada e implementada com maior segurança.