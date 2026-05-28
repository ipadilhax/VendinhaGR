using System;
using System.Collections.Generic;
using System.Text;

namespace VendinhaGR.Dtos
{
    public class UpdateDividaDto
    {
        public int Id { get; set; }
        public bool Paga {  get; set; }

        public UpdateDividaDto(int id, bool paga) 
        {
            Id = id;
            Paga = paga;
        } 
    }
}
