# .NET Clean Architecture – Template de Referência

## Propósito
Este repositório é uma **base opinada de Clean Architecture para aplicações .NET modernas**.

Foi criado como:
- uma **referência pessoal** para decisões arquiteturais
- um **ponto de partida reutilizável** (mentalidade de template, não de framework)
- um **registro de aprendizado** que evolui com a experiência prática

O foco é **clareza em vez de boilerplate**, **pragmatismo em vez de dogma**, e **padrões que escalam de pequenas APIs a sistemas complexos**.

---

## Filosofia Arquitetural

Este projeto segue os **princípios de Clean Architecture**, inspirado por:
- Diretrizes de Arquitetura .NET da Microsoft
- Jason Taylor (Clean Architecture)
- Steve Smith (Ardalis)
- DDD Pragmático e Vertical Slice Architecture

Ideias centrais:
- **Domain é rei** – puro, isolado, independente de frameworks
- **Application define os casos de uso**, não os detalhes técnicos
- **Infrastructure implementa contratos**, nunca vaza para dentro
- **WebApi é apenas um ponto de entrada**, fino e descartável

As dependências fluem **somente para dentro**.

```
WebApi → Application → Domain
        ↑
   Infrastructure
```

---

## Estrutura da Solução

```
MyProject
├── MyProject.Domain
│   ├── Entities
│   ├── ValueObjects
│   └── Exceptions
│
├── MyProject.Application
│   ├── Features
│   ├── Behaviors
│   └── Abstractions
│       ├── Persistence
│       ├── Messaging
│       └── Services
│
├── MyProject.Infrastructure
│   ├── Integration
│   ├── Messaging
│   └── Services
│
└── MyProject.WebApi
    ├── Endpoints
    ├── Middlewares
    ├── Extensions
    └── Program.cs
```

---

## Responsabilidades das Camadas

### Em termos simples:
- **Domain** – o "como": regras de negócio, invariantes e comportamentos
- **Application** – o "o quê": casos de uso e orquestração do fluxo
- **Infrastructure** – o "onde e com quê": banco de dados, mensageria, serviços externos
- **Presentation (Web API)** – o ponto de entrada: traduz HTTP em chamadas de aplicação

> Este resumo é especialmente útil para quem está aprendendo Clean Architecture pela primeira vez.  
> — @Bruno Milagres

### Detalhado

#### Domain
O **núcleo do sistema**.
- Regras de negócio, invariantes e comportamentos
- Sem dependências de frameworks ou infraestrutura
- Totalmente testável em memória
Contém:
- Entidades
- Objetos de Valor
- Exceções de Domínio
> O Domain responde **"como o negócio funciona"**.

---

#### Application
Define **o que o sistema faz** por meio de casos de uso.
- Orquestra fluxos de trabalho
- Aplica regras de negócio via Domain
- Depende apenas de abstrações
Conceitos-chave:
- Fatias verticais (`Features`)
- CQRS (Commands / Queries)
- Behaviors de pipeline (validação, logging, transações)
> A Application responde **"o que precisa ser feito"**.

---

#### Infrastructure
Contém **implementações técnicas**.
- Bancos de dados, serviços externos, mensageria, integrações
- Implementa interfaces definidas na Application
- Conhece EF Core, HTTP, brokers, SDKs, etc.
> A Infrastructure responde **"onde e com quê"**.

---

#### WebApi
O **ponto de entrada** do sistema.
- Minimal APIs
- Endpoints apenas traduzem HTTP ↔ Application
- Sem lógica de negócio
Responsabilidades:
- Mapeamento de requisições/respostas
- Composição de injeção de dependência
- Configuração de middlewares
> A WebApi é intencionalmente fina e substituível.

---

## Por que Não Existe um Projeto `Common`

Este template **evita intencionalmente um projeto `Common` compartilhado**.

Motivos:
- `Common` tende a se tornar um depósito de código sem critério
- Incentiva acoplamento forte entre camadas
- Oculta as fronteiras arquiteturais

Em vez disso:
- **Conceitos de negócio** ficam no Domain
- **Resultados / respostas de casos de uso** ficam na Application
- **Utilitários técnicos** ficam na Infrastructure

Cada peça pertence **onde faz sentido contextualmente**.

---

## Estratégia de Testes (Planejado)

- Domain: testes unitários puros
- Application: testes de casos de uso com abstrações mockadas
- Infrastructure: testes de integração
- WebApi: testes mínimos de fumaça / contrato

---

## Objetivos de Evolução

Este repositório deve evoluir com:
- Observabilidade (OpenTelemetry)
- Persistência (EF Core)
- Mensageria (outbox, eventos assíncronos)
- Autenticação / Autorização
- Prontidão para nuvem

As mudanças são intencionais e documentadas como marcos de aprendizado.

---

## Uso

Este repositório **não é um pacote NuGet**.

Ele deve ser:
- clonado
- adaptado
- renomeado
- evoluído

Pense nele como uma **linha de partida**, não um produto acabado.

---

## Funcionalidades Adicionadas

- **Serilog** para logging estruturado (todas as requisições HTTP e logs da aplicação são exibidos no console)
- **OpenTelemetry** para rastreamento distribuído (rastreia requisições HTTP e exporta para o console)
- **OpenAPI** documentação (JSON em `/openapi/v1.json`)
- **Scalar UI** para documentação interativa da API em `/`
- **Endpoint de saúde mínimo** em `/health` (`GET /health` retorna `{ "status": "Healthy" }`)

### Principais Pacotes
- [Serilog.AspNetCore](https://www.nuget.org/packages/Serilog.AspNetCore/): Logging
- [OpenTelemetry.Extensions.Hosting](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting/): Rastreamento
- [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi/): OpenAPI/Swagger
- [Scalar.AspNetCore](https://www.nuget.org/packages/Scalar.AspNetCore/): UI do OpenAPI

---

## Como Executar
1. Instale o SDK do .NET 10
2. Restaure os pacotes:
   ```bash
   dotnet restore
   ```
3. Execute o projeto:
   ```bash
   dotnet run --project MyProject.App/MyProject.Api.csproj
   ```
4. Acesse:
   - Scalar UI: [http://localhost:5000/](http://localhost:5000/) (ou a porta configurada)
   - OpenAPI JSON: [http://localhost:5000/openapi/v1.json](http://localhost:5000/openapi/v1.json)
   - Health check: [http://localhost:5000/health](http://localhost:5000/health)

---

## Contribuição
Pull requests são bem-vindos. Para mudanças significativas, abra uma issue primeiro para discutir o que você gostaria de alterar.

## Licença
Este projeto está licenciado sob a Licença MIT.

---

**Mantenedor:** @Bruno Milagres – 2024-06-07
