using Microsoft.AspNetCore.Mvc;
using VendinhaGR.Models;
using VendinhaGR.Services;
using VendinhaGR.Dtos;

namespace VendinhaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService service;

        public ClienteController(ClienteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get(string? search = null, int pagina = 1)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                return Ok(service.Pesquisa(search, 10, pagina));
            }

            return Ok(service.Listar(10, pagina));
        }


        [HttpPost]
        public IActionResult Post([FromBody] CreateClienteDto dto)
        {
            // a gente transforma o dto na entidade real aqui pra mandar pro service
            var cliente = new Cliente
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email
            };

            var sucesso = service.Criar(cliente, out var erros);
            return sucesso ? Ok(cliente) : UnprocessableEntity(erros);
        }


        [HttpPut]
        public IActionResult Update([FromBody] UpdateClienteDto categoria)
        {
            var sucesso = service.Atualizar(categoria, out var erros);
            return sucesso ? Ok(categoria) : UnprocessableEntity(erros);
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sucesso = service.Excluir(id, out var erros);
            return sucesso ? Ok(new { mensagem = "Cliente removido com sucesso." }) : UnprocessableEntity(erros);
        }
    }
}
