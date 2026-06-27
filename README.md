Desafio Técnico – Backend .NET | Vertrau

Este projeto foi desenvolvido como solução para o desafio técnico da Vertrau, com o objetivo de construir uma API REST para gerenciamento de usuários.

A aplicação foi estruturada com foco em boas práticas de engenharia de software, incluindo separação de responsabilidades, validações de regras de negócio, uso de ORM com migrations, containerização com Docker e tratamento de logs.

Tecnologias Utilizadas
.NET 8 (ASP.NET Core Web API)
Entity Framework Core
PostgreSQL
Swagger (OpenAPI)
Docker & Docker Compose
Microsoft Logging (ILogger)
C#

Funcionalidades da API

A API permite o gerenciamento completo de usuários:

Método	Endpoint	Descrição
POST	/usuarios	Criar usuário
GET	/usuarios	Listar usuários
GET	/usuarios/{id}	Buscar usuário por ID
PUT	/usuarios/{id}	Atualizar usuário
DELETE	/usuarios/{id}	Remover usuário

Modelo de Dados
UserModel
Campo	Tipo	Obrigatório
Id	long	Sim
Nome	string	Sim
Sobrenome	string	Sim
Email	string	Sim
Genero	enum	Sim
DataNascimento	DateTime	Não

Enum Genero
MASCULINO = 1  
FEMININO = 2  
OUTRO = 3

Regras de Negócio Implementadas
Email deve ser único no sistema
Data de nascimento não pode ser futura
Validação de campos obrigatórios via [Required]
Conversão segura de datas para UTC
Proteção contra duplicidade de e-mail tanto na aplicação quanto no banco


Arquitetura do Projeto

O projeto segue uma arquitetura em camadas, visando escalabilidade e manutenção:

 Camadas
Controller
Exposição dos endpoints REST
Controle de entrada/saída HTTP
Logging de requisições
Service
Regras de negócio
Validações
Integração com banco de dados
Data (DbContext)
Configuração do Entity Framework
Mapeamento de entidades
Migrations
DTOs
Separação entre modelo interno e externo
Proteção da entidade do banco
Mappers
Conversão entre Entity ↔ DTO

Docker Architecture
API (.NET 8)
PostgreSQL 17
Comunicação via network interna Docker

Execução com Docker
 Subir o projeto
docker-compose up --build

A API ficará disponível em:
http://localhost:5000/swagger

Banco de Dados

O projeto utiliza PostgreSQL com Docker.

Configuração:
Database: vertrau_db
User: postgres
Port: 5432

Banco de Dados (EF Core)
Migration inicial:
Criação da tabela Users
Configuração de chave primária
Campos obrigatórios
Migration adicional:
Índice único para Email:
.HasIndex(u => u.Email)
.IsUnique();

Tratamento de Erros

A aplicação trata:

Usuário não encontrado → 404 NotFound
Dados inválidos → 400 BadRequest
Conflitos de email → 400 BadRequest
Erros internos → logs via ILogger

Documentação da API

A documentação está disponível via Swagger:

http://localhost:5000/swagger

Inclui:

Endpoints
Modelos
Schemas de request/response
Testes diretos via interface
