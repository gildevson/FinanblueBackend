using Dapper;
using Microsoft.AspNetCore.Mvc;
using FinanblueBackend.Data;

namespace FinanblueBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly DbContextDapper _dapper;

        public TesteController(DbContextDapper dapper)
        {
            _dapper = dapper;
        }

        [HttpGet("conexao")]
        public IActionResult TestarConexao()
        {
            try
            {
                using var connection = _dapper.CreateConnection();
                connection.Open();
                return Ok("✅ Conexão com o banco de dados SQL Server bem-sucedida!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Erro ao conectar com o banco: {ex.Message}");
            }
        }

        [HttpGet("usuarios")]
        public IActionResult GetUsuarios()
        {
            using var connection = _dapper.CreateConnection();
            var usuarios = connection.Query("SELECT TOP 10 * FROM Usuarios");
            return Ok(usuarios);
        }
    }
}
