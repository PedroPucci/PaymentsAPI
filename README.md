# FCG Payments API

## Descrição

O **FCG Payments API** é o microsserviço responsável pelo processamento (simulado) dos pagamentos das compras de jogos da plataforma **FCG (FIAP Cloud Games)**.

Desenvolvido em **.NET 8**, o projeto utiliza **Clean Architecture**, separação de responsabilidades e boas práticas de desenvolvimento backend.

Este microsserviço é responsável por:

- Processar pagamentos;
- Validar os dados da requisição;
- Simular aprovação ou rejeição do pagamento;
- Persistir as transações;
- Publicar o resultado do processamento para os demais microsserviços.

---

# Responsabilidades

O PaymentsAPI possui apenas uma responsabilidade:

## Processar pagamentos

Recebe as informações da compra:

- Usuário
- Jogo
- Valor
- Método de pagamento

Realiza o processamento do pagamento e registra a transação.

No fluxo completo da arquitetura orientada a eventos, este microsserviço consumirá o evento:

```text
OrderPlacedEvent
```

Após processar o pagamento, publicará:

```text
PaymentProcessedEvent
```

---

# Regras de Negócio

- O usuário deve estar autenticado.
- O GameId deve ser válido.
- O valor do pagamento deve ser maior que zero.
- O método de pagamento é obrigatório.
- Cada pagamento recebe um identificador único.
- Cada pagamento possui um TransactionId.
- O pagamento pode ser aprovado ou rejeitado.

Status possíveis:

```text
Pending
Approved
Rejected
```

---

# Tecnologias Utilizadas

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
| ConnectionStrings__WebApiDatabase | Banco SQL Server |
| JwtSettings__SecretKey | Chave JWT |
| JwtSettings__Issuer | Emissor do Token |
| JwtSettings__Audience | Público do Token |
| RunMigrations | Executa migrations automaticamente |

---

# Executando

## Visual Studio

Defina o projeto **PaymentsAPI** como Startup Project e execute.

---

## Docker

```bash
docker compose up --build
```

Swagger:

```text
http://localhost:8082/swagger
```

---

# Endpoint

## Processar Pagamento

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

# Fluxo da Arquitetura

```text
CatalogAPI
      │
      ▼
OrderPlacedEvent
      │
      ▼
PaymentsAPI
      │
Processa pagamento
      │
      ▼
PaymentProcessedEvent
```

---

# Estrutura

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
