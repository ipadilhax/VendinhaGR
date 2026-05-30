using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace VendinhaGR.Models
{
    public class Divida
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Valor { get; set; }
        public bool Paga { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataPagamento { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; } = null!;
    }
}
