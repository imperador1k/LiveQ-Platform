# LiveQ Platform

LiveQ é uma plataforma web de perguntas e respostas em tempo real, desenvolvida com **ASP.NET Core Razor Pages**, **Entity Framework Core**, **ASP.NET Core Identity** e **SignalR**.

O projeto foi desenvolvido no âmbito da unidade curricular de **Desenvolvimento Web**, da Licenciatura em Engenharia Informática, pelos alunos **Miguel Santos** e **João Sousa**.

## Objetivo

A aplicação permite criar salas de Q&A em tempo real, onde os participantes podem submeter perguntas e votar nas perguntas mais relevantes.  
O orador consegue acompanhar as questões mais votadas, marcar perguntas como respondidas e gerir o evento.

## Tecnologias Utilizadas

| Tecnologia | Função |
|---|---|
| ASP.NET Core Razor Pages | Interface web da aplicação |
| Entity Framework Core | Acesso e gestão da base de dados |
| ASP.NET Core Identity | Autenticação e gestão de utilizadores |
| SignalR | Comunicação em tempo real |
| SQL Server | Base de dados |
| Bootstrap | Estilização da interface |
| jQuery | Validação e interatividade no cliente |

## Diagrama da Base de Dados

O modelo de dados da aplicação é composto pelas entidades principais **AspNetUsers**, **Events**, **Questions** e **Upvotes**.

![Diagrama Entidade-Relacionamento](Docs/LiveQ_ERD.png)

## Tabelas da Base de Dados

### AspNetUsers

Tabela gerida pelo ASP.NET Core Identity, responsável por armazenar os utilizadores autenticados da aplicação.

| Campo | Tipo | Chave | Descrição |
|---|---|---|---|
| Id | string | PK | Identificador único do utilizador |
| UserName | string |  | Nome de utilizador |
| Email | string |  | Email do utilizador |

---

### Events

Representa uma sala ou evento de perguntas e respostas criado por um orador.

| Campo | Tipo | Chave | Descrição |
|---|---|---|---|
| Id | int | PK | Identificador único do evento |
| Title | string |  | Título do evento |
| Description | string |  | Descrição opcional do evento |
| CreatedAt | DateTime |  | Data e hora de criação |
| CreatorId | string | FK | Referência ao utilizador que criou o evento |

---

### Questions

Representa uma pergunta submetida num determinado evento.

| Campo | Tipo | Chave | Descrição |
|---|---|---|---|
| Id | int | PK | Identificador único da pergunta |
| Content | string |  | Texto da pergunta |
| CreatedAt | DateTime |  | Data e hora de submissão |
| IsAnswered | bool |  | Indica se a pergunta já foi respondida |
| EventId | int | FK | Referência ao evento associado |
| UserId | string | FK | Referência ao utilizador que submeteu a pergunta |

> Nota: O campo `UserId` pode ser nulo para permitir perguntas submetidas por utilizadores anónimos.

---

### Upvotes

Tabela de junção que representa os votos dos utilizadores nas perguntas.

| Campo | Tipo | Chave | Descrição |
|---|---|---|---|
| QuestionId | int | PK / FK | Referência à pergunta votada |
| UserId | string | PK / FK | Referência ao utilizador que votou |

A chave primária composta por `QuestionId` e `UserId` garante que cada utilizador só pode votar uma vez em cada pergunta.

## Relações entre Entidades

| Relação | Tipo | Descrição |
|---|---|---|
| AspNetUsers → Events | 1:N | Um utilizador pode criar vários eventos |
| Events → Questions | 1:N | Um evento pode conter várias perguntas |
| AspNetUsers → Questions | 1:N | Um utilizador pode submeter várias perguntas |
| AspNetUsers ↔ Questions | M:N | Um utilizador pode votar em várias perguntas e uma pergunta pode receber votos de vários utilizadores, através da tabela Upvotes |

## Funcionalidades Principais

- Criação e gestão de eventos por utilizadores com papel de orador
- Submissão de perguntas em eventos
- Votação em perguntas por utilizadores autenticados
- Atualização de votos em tempo real com SignalR
- Marcação de perguntas como respondidas
- Autenticação e autorização com ASP.NET Core Identity
- Relações 1:N e M:N implementadas com Entity Framework Core

## Autores

- Miguel Santos
- João Sousa

## Unidade Curricular

Projeto desenvolvido para a unidade curricular de **Desenvolvimento Web**  
Licenciatura em Engenharia Informática
