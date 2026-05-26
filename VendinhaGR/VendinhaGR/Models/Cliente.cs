using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VendinhaGR.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nome { get; set; }
        [Required, MaxLength(11)]
        public string CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [NotMapped]
        public int Idade
        {
            get
            {
                var hoje = DateTime.Today;
                var idade = hoje.Year - DataNascimento.Year;
                if (DataNascimento.Date > hoje.AddYears(-idade))
                    idade--;

                return idade;
            }
        }
        [EmailAddress]
        public string? Email { get; set; }
        public List<Divida> Dividas { get; set; } = new();
    }
}

