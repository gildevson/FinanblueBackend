# ?? FinanblueBackend

Projeto backend desenvolvido em **C# (.NET 8)** utilizando **Dapper** como ORM, seguindo o padr�o arquitetural adotado pela **Finanblue**.

---

## ?? Estrutura de Camadas do Projeto

### ?? Model (ou Entidade)

**O que �:**  
Representa os dados do sistema. Reflete uma tabela do banco de dados ou um objeto real (ex: Usu�rio, Produto, Cliente, Pedido).

**Fun��o:**  
Guardar as propriedades e tipos de dados que o sistema usa.

#### Exemplo:
```csharp
public class Usuario 
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}
??? Database
O que �:
Camada respons�vel pela conex�o direta com o banco de dados.
No caso do Dapper, � usada para abrir conex�es e executar queries SQL.

Fun��o:

Gerenciar a conex�o (abrir/fechar);

Centralizar a string de conex�o;

Evitar repetir c�digo SQL em v�rios lugares.

Exemplo:
csharp
Copiar c�digo
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class DbContextDapper
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public DbContextDapper(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
?? Repository
O que �:
Camada respons�vel por conversar com o banco de dados.
Utiliza o DbContextDapper para executar comandos SQL.

Fun��o:

Executar SELECT, INSERT, UPDATE e DELETE;

Converter resultados SQL em objetos (Model);

Manter o acesso a dados separado da l�gica de neg�cio.

Exemplo:
csharp
Copiar c�digo
using Dapper;

public class UsuarioRepository
{
    private readonly DbContextDapper _context;

    public UsuarioRepository(DbContextDapper context)
    {
        _context = context;
    }

    public IEnumerable<Usuario> GetUsuarios()
    {
        using var connection = _context.CreateConnection();
        return connection.Query<Usuario>("SELECT * FROM Usuarios");
    }

    public int AddUsuario(Usuario user)
    {
        using var connection = _context.CreateConnection();
        string sql = "INSERT INTO Usuarios (Nome, Email) VALUES (@Nome, @Email)";
        return connection.Execute(sql, user);
    }
}
?? Service
O que �:
Camada intermedi�ria entre Controller e Repository.
Cont�m as regras de neg�cio e valida��es do sistema.

Fun��o:

Validar dados;

Processar regras (c�lculos, duplicidades, valida��es);

Chamar o reposit�rio;

Garantir que a Controller trate apenas requisi��es.

Exemplo:
csharp
Copiar c�digo
public class UsuarioService
{
    private readonly UsuarioRepository _repository;

    public UsuarioService(UsuarioRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Usuario> ListarUsuarios()
    {
        return _repository.GetUsuarios();
    }

    public string CadastrarUsuario(Usuario user)
    {
        if (string.IsNullOrEmpty(user.Email))
            throw new Exception("Email � obrigat�rio!");

        _repository.AddUsuario(user);
        return "Usu�rio cadastrado com sucesso!";
    }
}
?? Controller
O que �:
Ponto de entrada da API � recebe requisi��es HTTP do frontend (Angular) e retorna respostas JSON.

Fun��o:

Receber e responder requisi��es da Web (GET, POST, PUT, DELETE);

Chamar o Service;

Retornar o resultado para o frontend.

Exemplo:
csharp
Copiar c�digo
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetUsuarios()
    {
        var usuarios = _service.ListarUsuarios();
        return Ok(usuarios);
    }

    [HttpPost]
    public IActionResult AddUsuario([FromBody] Usuario user)
    {
        var msg = _service.CadastrarUsuario(user);
        return Ok(msg);
    }
}
?? Fluxo de Requisi��o
css
Copiar c�digo
[ Angular / Frontend ]
          ?
[ Controller ]  ? recebe a requisi��o HTTP
          ?
[ Service ]  ? cont�m regras de neg�cio e valida��es
          ?
[ Repository ]  ? executa SQL no banco
          ?
[ Database ]  ? cria e gerencia a conex�o
          ?
[ Model ]  ? representa a estrutura dos dados
??? Estrutura do Projeto
mathematica
Copiar c�digo
FinanblueBackend/
�
+-- Connected Services/
+-- Dependencies/
+-- Properties/
+-- wwwroot/
+-- Pages/
+-- appsettings.json
+-- Program.cs
+-- Readme.md
?? Descri��o das Pastas e Arquivos
Pasta / Arquivo	Fun��o Principal
Connected Services	Conectar o projeto a servi�os externos (Azure, APIs, etc).
Dependencies	Gerencia as bibliotecas NuGet do projeto.
Properties	Cont�m as configura��es de execu��o (porta, ambiente, etc).
wwwroot	Armazena arquivos est�ticos (imagens, CSS, JS).
Pages	P�ginas Razor (usadas em MVC, podem ser removidas se for apenas API).
appsettings.json	Configura��es gerais e conex�es do sistema.
Program.cs	Ponto de entrada e configura��o do servidor.
Readme.md	Documenta��o do projeto.

?? Arquivo appsettings.json
Cont�m as configura��es e a string de conex�o com o banco de dados.
Equivale ao .env em um projeto Node.js.

Exemplo:
json
Copiar c�digo
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FinanblueDB;User Id=sa;Password=1234;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
?? Arquivo Program.cs
Ponto de inicializa��o do backend.
Aqui s�o configurados os servi�os, middlewares e endpoints da API.

Exemplo:
csharp
Copiar c�digo
var builder = WebApplication.CreateBuilder(args);

// Configura servi�os
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
?? Conclus�o
Essa arquitetura garante:

Separa��o clara de responsabilidades;

Organiza��o e reuso de c�digo;

Facilidade de manuten��o e escalabilidade;

Integra��o direta com SQL Server usando Dapper;

Padr�o de desenvolvimento adotado pela Finanblue.

????? Autor
Gilson Fonseca
?? Email: gilsonfonseca92@gmail.com
?? GitHub: gildevson
?? Empresa: Finanblue

yaml
Copiar c�digo

---

Quer que eu te gere **uma vers�o com diagrama visual em Mermaid** (para o GitHub mostrar o fluxo graficamente)?  
Fica muito profissional no README, mostrando a liga��o entre as camadas (Controller ? Service ? Repos