using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VendinhaGR.Models
{
    public class Divida
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public bool Paga { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
    }
}
