using Microsoft.AspNetCore.Mvc;
using VendinhaGR.Models;
using VendinhaGR.Services;
using VendinhaGR.Dtos;

namespace VendinhaAPI.Controllers
{
    [Route("api/[controller]")]
    public class DividaController : ControllerBase
    {
        private readonly DividaService service;

        public DividaController(DividaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get(string search, int pagina)
        {
            var dividas = service.Listar();
            return Ok(dividas);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Divida divida)
        {
            var sucesso = service.Criar(divida, out var erros);
            return sucesso ? Ok(divida) : UnprocessableEntity(erros);
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateDividaDto dto)
        {
            var sucesso = service.Atualizar(dto, out var erros);
            return sucesso ? Ok(dto) : UnprocessableEntity(erros);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var sucesso = service.Excluir(id, out var erros);
            return sucesso ? Ok(new {mensagem = "Divida removida com sucesso"}) : UnprocessableEntity(erros);
        }
    }
}
