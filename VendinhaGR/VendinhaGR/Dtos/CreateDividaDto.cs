using System;

namespace VendinhaGR.Dtos
{
    public class CreateDividaDto
    {
        public decimal Valor { get; set; }

        public int ClienteId { get; set; }


        public CreateDividaDto(decimal valor, int clienteId)
        {
            Valor = valor;
            ClienteId = clienteId;
        }
    }
}