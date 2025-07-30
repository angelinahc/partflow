
# PartFlow - Sistema de Rastreabilidade de Peças | PartFlow - Industrial Parts Traceability System


## 📌 Visão Geral | Overview

Este repositório contém o código-fonte de um sistema full-stack de rastreabilidade de peças industriais, desenvolvido como uma solução para um desafio técnico. O projeto simula um ambiente industrial onde peças precisam passar por um fluxo obrigatório de estações, e cada passo desse processo é registrado e validado.

This repository contains the source code for a full-stack system for industrial parts traceability, developed as a solution for a technical challenge. The project simulates an industrial environment where parts must go through a mandatory station flow, with each step recorded and validated.

---

## 🎯 Requisitos e Regras de Negócio | Requirements and Business Rules

- **Fluxo de Processo Obrigatório**: As peças devem seguir uma sequência pré-definida de estações (ex: Recebimento → Montagem → Inspeção Final), sem pular ou retroceder etapas.  
- **Mandatory Process Flow**: Parts must follow a predefined sequence of stations (e.g., Receiving → Assembly → Final Inspection) without skipping or going backward.

- **Rastreabilidade Completa**: Cada movimentação de uma peça, incluindo o responsável, data e estações de origem/destino, deve ser registrada.  
- **Complete Traceability**: Every movement of a part, including the responsible person, date, and origin/destination stations, must be recorded.

- **Unicidade de Dados**: Cada peça deve ter um código único, e cada estação, um nome único.  
- **Data Uniqueness**: Each part must have a unique code, and each station a unique name.

- **Gerenciamento Dinâmico**: A interface deve permitir o cadastro e a listagem tanto de peças quanto de estações.  
- **Dynamic Management**: The interface must allow registering and listing both parts and stations.

---

## 🛠️ Tecnologias Utilizadas | Technologies Used

- **Back-end**: C#, .NET 8, ASP.NET Core Web API  
- **Front-end**: Angular 18, TypeScript, CSS  
- **Testes | Testing**: xUnit & Moq  
- **Controle de versão | Version Control**: Git

---

## ▶️ Como Executar | How to Run

### Pré-requisitos | Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Node.js](https://nodejs.org/) & [Angular CLI](https://angular.io/cli)

### Rodando o Back-end (API) | Running the Back-end (API)

```bash
cd api
dotnet watch run
```

A API estará disponível em: `https://localhost:7123`  
The API will be available at: `https://localhost:7123`

### Rodando o Front-end (Angular) | Running the Front-end (Angular)

```bash
cd partflow-ui
ng serve
```

A aplicação estará acessível em: `http://localhost:4200`  
The application will be accessible at: `http://localhost:4200`

---

## 🚧 Desafios e Soluções | Challenges and Solutions

### 🔄 enum Estático vs. Estações Dinâmicas | Static enum vs. Dynamic Stations

**Problema | Problem**: O status da peça era controlado por um `enum` fixo.  
**Solução | Solution**: Remoção do enum e uso de `CurrentStationId`.

---

### 🗃️ Exclusão e Histórico | Deletion and History

**Problema | Problem**: DELETE físico apagaria o histórico.  
**Solução | Solution**: Implementação de soft delete (`IsActive = false`).

---

### ♻️ Reutilização de Códigos | Reusing Part Codes

**Problema | Problem**: Código de peça já utilizado anteriormente.  
**Solução | Solution**: Reativação da peça com reset do progresso.

---

### 📐 Ordem das Estações | Station Order

**Problema | Problem**: Exclusão criava buracos na ordem.  
**Solução | Solution**: Lógica de reordenação automática no back-end.

---

## 📎 Licença | License

Este projeto foi desenvolvido como parte de um desafio técnico e é distribuído sem fins comerciais.  
This project was developed as part of a technical challenge and is distributed for non-commercial purposes.
