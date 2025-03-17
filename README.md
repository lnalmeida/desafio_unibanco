# 🏦 Desafio Backend Unibanco - Port para C# (.NET)

**Link original do desafio: https://github.com/feltex/desafio-itau-backend.git

**Implementação didática** do desafio técnico originalmente proposto em Java/Spring, recriado em **C# (.NET 8)** utilizando:
- **Minimal APIs** para simplicidade e performance
- **Object Calisthenics** para qualidade de código
- **System.Text.Json** para serialização otimizada

[![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 📌 Contexto do Desafio

Implementação de uma API para gestão de transações financeiras com:
- Validações de formato de dados
- Códigos de status HTTP específicos
- Arquitetura limpa e extensível

**Diferencial Técnico:** Aplicação prática dos princípios de [**Object Calisthenics**](https://www.cs.olemiss.edu/~hcc/csci581/lectures/Handout-ObjectCalisthenics.pdf) para melhoria da qualidade do código.

---

## 🎯 Funcionalidades

| Endpoint         | Método | Descrição                              | Status Codes                         |
|------------------|--------|----------------------------------------|--------------------------------------|
| `/transacoes`    | POST   | Cria nova transação com validações     | 201 Created, 400 Bad Request, 422 Unprocessable Entity |
| `/transacoes`    | GET    | Lista todas as transações cadastradas  | 200 OK                               |
| `/transacoes`    | DELETE | Remove todas as transações da lista    | 200 OK                               |

---

## 🔧 Tecnologias e Princípios

- **.NET 8** com Minimal APIs
- **Value Objects** para `DataHora` e `Valor`
- **Custom Converters** para serialização JSON
- **Injeção de Dependência** nativa
- **Validação por Contrato** (precondições)
- **Imutabilidade** sempre que possível

---

## 🛠️ Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- IDE (Recomendado: [VS Code](https://code.visualstudio.com/) com [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit))

### Passo a Passo
```bash
# Clone o repositório
git clone https://github.com/seu-usuario/unibanco-challenge-csharp.git
cd unibanco-challenge-csharp

# Restaure pacotes
dotnet restore

# Execute a API
dotnet run --project src/Unibanco.API
