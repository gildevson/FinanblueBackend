# ?? FinanblueBackend

Projeto backend desenvolvido em **C# (.NET 8)** utilizando **Dapper** como ORM, seguindo o padrão arquitetural adotado pela **Finanblue**.

---

## ?? Estrutura de Camadas do Projeto

### ?? Model (ou Entidade)

**O que é:**  
Representa os dados do sistema. Reflete uma tabela do banco de dados ou um objeto real (ex: Usuário, Produto, Cliente, Pedido).

**Função:**  
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
O que é:
Camada responsável pela conexão direta com o banco de dados.
No caso do Dapper, é usada para abrir conexões e executar queries SQL.

Função:

Gerenciar a conexão (abrir/fechar);

Centralizar a string de conexão;

Evitar repetir código SQL em vários lugares.

Exemplo:
csharp
Copiar código
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
O que é:
Camada responsável por conversar com o banco de dados.
Utiliza o DbContextDapper para executar comandos SQL.

Função:

Executar SELECT, INSERT, UPDATE e DELETE;

Converter resultados SQL em objetos (Model);

Manter o acesso a dados separado da lógica de negócio.

Exemplo:
csharp
Copiar código
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
O que é:
Camada intermediária entre Controller e Repository.
Contém as regras de negócio e validações do sistema.

Função:

Validar dados;

Processar regras (cálculos, duplicidades, validações);

Chamar o repositório;

Garantir que a Controller trate apenas requisições.

Exemplo:
csharp
Copiar código
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
            throw new Exception("Email é obrigatório!");

        _repository.AddUsuario(user);
        return "Usuário cadastrado com sucesso!";
    }
}
?? Controller
O que é:
Ponto de entrada da API — recebe requisições HTTP do frontend (Angular) e retorna respostas JSON.

Função:

Receber e responder requisições da Web (GET, POST, PUT, DELETE);

Chamar o Service;

Retornar o resultado para o frontend.

Exemplo:
csharp
Copiar código
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
?? Fluxo de Requisição
css
Copiar código
[ Angular / Frontend ]
          ?
[ Controller ]  ? recebe a requisição HTTP
          ?
[ Service ]  ? contém regras de negócio e validações
          ?
[ Repository ]  ? executa SQL no banco
          ?
[ Database ]  ? cria e gerencia a conexão
          ?
[ Model ]  ? representa a estrutura dos dados
??? Estrutura do Projeto
mathematica
Copiar código
FinanblueBackend/
¦
+-- Connected Services/
+-- Dependencies/
+-- Properties/
+-- wwwroot/
+-- Pages/
+-- appsettings.json
+-- Program.cs
+-- Readme.md
?? Descrição das Pastas e Arquivos
Pasta / Arquivo	Função Principal
Connected Services	Conectar o projeto a serviços externos (Azure, APIs, etc).
Dependencies	Gerencia as bibliotecas NuGet do projeto.
Properties	Contém as configurações de execução (porta, ambiente, etc).
wwwroot	Armazena arquivos estáticos (imagens, CSS, JS).
Pages	Páginas Razor (usadas em MVC, podem ser removidas se for apenas API).
appsettings.json	Configurações gerais e conexões do sistema.
Program.cs	Ponto de entrada e configuração do servidor.
Readme.md	Documentação do projeto.

?? Arquivo appsettings.json
Contém as configurações e a string de conexão com o banco de dados.
Equivale ao .env em um projeto Node.js.

Exemplo:
json
Copiar código
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
Ponto de inicialização do backend.
Aqui são configurados os serviços, middlewares e endpoints da API.

Exemplo:
csharp
Copiar código
var builder = WebApplication.CreateBuilder(args);

// Configura serviços
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
?? Conclusão
Essa arquitetura garante:

Separação clara de responsabilidades;

Organização e reuso de código;

Facilidade de manutenção e escalabilidade;

Integração direta com SQL Server usando Dapper;

Padrão de desenvolvimento adotado pela Finanblue.

????? Autor
Gilson Fonseca
?? Email: gilsonfonseca92@gmail.com
?? GitHub: gildevson
?? Empresa: Finanblue

yaml
Copiar código

---

Quer que eu te gere **uma versão com diagrama visual em Mermaid** (para o GitHub mostrar o fluxo graficamente)?  
Fica muito profissional no README, mostrando a ligação entre as camadas (Controller ? Service ? Repos