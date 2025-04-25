
# CoreSuit - Sistema Base Clean Architecture 🚀

**CoreSuit** é um **template profissional, completo e escalável** para projetos ASP.NET Core, estruturado seguindo os padrões da **Clean Architecture**.

Ele foi projetado para acelerar o desenvolvimento de sistemas que envolvam operações CRUD, autenticação JWT, documentação via Swagger e separação clara de responsabilidades.

---

## 📦 Como instalar o template

Para instalar o CoreSuit na sua máquina:

```bash
dotnet new install https://github.com/oliver-soft-tech/core-suit
```

---

## 🗂 Estrutura de Pastas

- **CoreSuit.API** — Camada de apresentação (WebAPI)
- **CoreSuit.Application** — Regras de negócio e serviços (Application Layer)
- **CoreSuit.Domain** — Entidades, DTOs e interfaces de domínio
- **CoreSuit.Infrastructure** — Acesso a dados e contexto EF Core
- **CoreSuit.CrossCutting.IoC** — Injeção de dependência e configurações

---

## 🛠 Tecnologias Utilizadas

- ASP.NET Core 9.0+
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Swashbuckle (Swagger para documentação de APIs)
- JWT Authentication (Autenticação via Bearer Token)
- dotenv.net (Gerenciamento de variáveis de ambiente)
- Middleware global para tratamento de erros

---

## 🎯 Padrões e Convenções Aplicados

- Separação em camadas (API, Application, Domain, Infrastructure)
- Utilização de DTOs para entrada e saída de dados
- Soft Delete (campo `DeletedAt` nas entidades)
- Suporte a Ativação e Reativação de registros
- Controllers genéricos (`BaseController`)
- Serviços genéricos (`BaseService`)
- Repositórios genéricos (`BaseRepository`)
- Injeção de dependência configurada automaticamente

---

## 🚀 Como Executar um Projeto Base

1. **Crie o arquivo `.env`** na raiz do projeto com as variáveis:

```
DB_HOST=localhost
DB_PORT=5432
DB_USER=postgres
DB_PASSWORD=123456
DB_DATABASE=baseDeDados
```

2. **Crie a primeira migration do banco de dados**:

```bash
dotnet ef migrations add FirstMigration --startup-project .\CoreSuit.API\ --project .\CoreSuit.Infrastructure\
```

3. **Execute a aplicação**:

```bash
dotnet run --project .\CoreSuit.API\
```

A aplicação estará disponível em:  
👉 `http://localhost`

---

## 📄 Documentação Swagger

Após subir o projeto, acesse:

```
http://localhost/swagger
```

- Toda a API CRUD será documentada automaticamente.
- Você pode autenticar usando JWT através do botão "Authorize" no Swagger.

---

## 🌐 Configuração opcional de PathBase

Se você for hospedar o projeto em uma subpasta (ex.: `https://dominio.com/Backend`), ative o seguinte trecho no `Program.cs`:

```csharp
// if (!app.Environment.IsDevelopment())
// {
//     app.Use((context, next) =>
//     {
//         context.Request.PathBase = "/Backend";
//         return next();
//     });
// }
```

---

## ✨ Exemplo de Endpoint Customizado

Você pode adicionar métodos personalizados em qualquer controller:

```csharp
[HttpGet("ativos")]
public async Task<IActionResult> ObterAtivos()
{
    var result = await _service.ObterSomenteAtivos();
    return Ok(result);
}
```

---

## 💼 Sobre

Desenvolvido por **Jean Oliveira**
Foco em alta escalabilidade, produtividade, organização e melhores práticas de desenvolvimento .NET.

---
