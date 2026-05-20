using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VendinhaGR.Models
{
    public class Divida
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public bool Situacao { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
    }
}
