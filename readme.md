# Sistema Base - Clean Architecture 
Este projeto é um **template completo e escalável** de sistema ASP.NET Core com estrutura em Clean
Architecture.
Ele serve como base para qualquer novo sistema que envolva operações CRUD, autenticação JWT,
documentação com Swagger e separação em camadas.
## Estrutura de Pastas
 - Backend.API # Camada de apresentação (WebAPI)
 - Backend.Application # Regras de negócio e serviços
 - Backend.Domain # Entidades, DTOs e interfaces
 - Backend.Infrastructure # Acesso a dados e contexto EF Core
 - Backend.CrossCutting.IoC # Injeção de dependência
## Tecnologias Utilizadas
- ASP.NET Core 9+
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Swagger (Swashbuckle)
- JWT (Autenticação via Bearer Token)
- dotenv.net (variáveis de ambiente)
- Middleware global de tratamento de erros
## Padrões e Convenções
- Separação em camadas (API, Application, Domain, Infrastructure)
- DTOs para entrada e saída de dados
- Soft Delete (com campo `DeletedAt`)
- Suporte a Ativação/Reativação de registros
- Controllers genéricos baseados em `BaseController`
- Serviços genéricos baseados em `BaseService`
- Repositórios genéricos baseados em `BaseRepository`
## Como Executar o Projeto
1. Crie um arquivo `.env` na raiz do projeto com os seguintes valores:
DB_HOST=localhost
DB_PORT=5432
DB_USER=postgres
DB_PASSWORD=123456
DB_DATABASE=equipment_appointment
2. Execute o comando para aplicar a primeira migration:

```
dotnet ef migrations add FirstMigration --startup-project .\CoreSuit.API\ --project .\CoreSuit.Infrastructure\
```

3. Inicie a aplicação com:
```
dotnet run --project .\Backend.API\
```
A aplicação estará disponível em `http://localhost` (ou conforme configurado).
## Documentação Swagger
Acesse `http://localhost/swagger` para visualizar a documentação interativa da API.
- Todas as rotas padrão de CRUD estarão documentadas.
- É possível testar endpoints protegidos com JWT inserindo o token no botão "Authorize".
## Configuração opcional de PathBase
Se você deseja hospedar o sistema em uma subpasta como `https://dominio.com/Backend`,
descomente este trecho no `Program.cs`:
```
// if (!app.Environment.IsDevelopment())
// {
// app.Use((context, next) =>
// {
// context.Request.PathBase = "/Backend";
// return next();
// });
// }
```
## Exemplo de Endpoint Customizado
Você pode herdar `BaseController` e criar endpoints adicionais no seu controller específico:
[HttpGet("ativos")]public async Task<IActionResult> ObterAtivos(){ var result = await
_service.ObterSomenteAtivos(); return Ok(result);}
---
Desenvolvido por **Cenix** com arquitetura limpa e foco em escalabilidade. Ideal para novos projetos
.NET!