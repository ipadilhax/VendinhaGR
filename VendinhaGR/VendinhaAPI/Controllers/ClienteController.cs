using Microsoft.AspNetCore.Mvc;
using VendinhaGR.Models;
using VendinhaGR.Services;
using VendinhaGR.Dtos;

namespace VendinhaAPI.Controllers
{
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService service;

        public ClienteController(ClienteService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Get(string search, int pagina)
        {
            var categorias = service.Listar();
            return Ok(categorias);
        }


        [HttpPost]
        public IActionResult Post([FromBody] Cliente cliente)
        {
            var sucesso = service.Criar(cliente, out var erros);
            return sucesso ? Ok(cliente) : UnprocessableEntity(erros);
        }


        [HttpPut]
        public IActionResult Update([FromBody] UpdateClienteDto categoria)
        {
            var categorias = service.Listar();
            return Ok(categorias);
            //exemplo
        }


        //[HttpDelete]
        //public IActionResult Delete(string search, int pagina)
        //{
        //    var categorias = service.Listar();
        //    return Ok(categorias);
        //}



    }




}
