# FCG Payments API

## Descrição do Projeto

O **FCG Payments API** é o microsserviço desenvolvido em **.NET 8** responsável pelo processamento (simulado) de pagamentos de compras de jogos da plataforma **FCG (FIAP Cloud Games)**.

A aplicação segue os princípios de **Clean Architecture**, separação de responsabilidades e boas práticas de desenvolvimento backend.

Este serviço é responsável por:

- Processar pagamentos de compras de jogos
- Validar os dados do pagamento
- Simular aprovação ou rejeição de pagamentos
- Persistir transações no banco de dados
- Gerar identificadores únicos de transações
- Consumir eventos de compra
- Publicar eventos de confirmação do pagamento
- Registrar logs estruturados

---

# Responsabilidades do Microsserviço

## Processamento de Pagamentos

O PaymentsAPI recebe as informações necessárias para processar uma compra de jogo.

Cada pagamento possui:

- Usuário
- Jogo
- Valor
- Método de pagamento

---

## Simulação do Pagamento

Como este projeto possui fins acadêmicos, o pagamento é simulado.

Os possíveis status são:

```text
Pending
Approved
Rejected
```

---

## Fluxo de Eventos

No fluxo completo da aplicação:

```text
CatalogAPI
      │
      ▼
OrderPlacedEvent
      │
      ▼
PaymentsAPI
      │
      ▼
PaymentProcessedEvent
```

O PaymentsAPI consome o evento **OrderPlacedEvent**, processa o pagamento e publica o evento:

```text
PaymentProcessedEvent
```

---

# Regras de Negócio

### Pagamento

- O usuário deve estar autenticado.
- O GameId deve ser válido.
- O valor deve ser maior que zero.
- O método de pagamento é obrigatório.
- Cada pagamento gera uma TransactionId única.
- O pagamento recebe um status.

### Status possíveis

```text
Pending
Approved
Rejected
```

---

# Recursos Utilizados

- .NET 8
- Entity Framework Core
- SQL Server
- Clean Architecture
- Repository Pattern
- Unit of Work
- FluentValidation
- Serilog
- JWT Authentication
- Swagger/OpenAPI
- Docker
- Xunit
- Moq
- FluentAssertions

---

# Variáveis de Ambiente

| Variável | Descrição |
|----------|-----------|
| ConnectionStrings__WebApiDatabase | String de conexão |
| JwtSettings__SecretKey | Chave JWT |
| JwtSettings__Issuer | Emissor do Token |
| JwtSettings__Audience | Público do Token |
| RunMigrations | Executa migrations automaticamente |

---

# Executando o Projeto

## Visual Studio

1. Abra a solução.
2. Defina **PaymentsAPI** como Startup Project.
3. Execute a aplicação.

---

## Docker

Na raiz da solução execute:

```bash
docker compose up --build
```

Swagger:

```text
http://localhost:8082/swagger
```

Parar containers:

```bash
docker compose down
```

---

# Endpoints

## Processamento de Pagamento

```http
POST /api/payments
```

Exemplo:

```json
{
  "gameId": 1,
  "amount": 149.90,
  "paymentMethod": "CreditCard"
}
```

Resposta:

```json
{
  "success": true,
  "data": {
    "id": "GUID",
    "userId": "USER_ID",
    "gameId": 1,
    "amount": 149.90,
    "paymentMethod": "CreditCard",
    "transactionId": "PAY-XXXXXXXX",
    "status": "Approved"
  }
}
```

---

# Estrutura da Solução

```text
PaymentsAPI
│
├── docs
├── src
│   ├── PaymentsAPI
│   ├── PaymentsAPI.Application
│   ├── PaymentsAPI.Domain
│   ├── PaymentsAPI.Infrastructure
│   └── PaymentsAPI.Shared
│
├── tests
├── docker-compose
└── README.md
```

---

# Tecnologias

- .NET 8
- SQL Server
- Entity Framework Core
- Serilog
- FluentValidation
- Docker
- Swagger
- JWT
- Clean Architecture
- Repository Pattern
- Unit of Work
