using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VendinhaGR.Models
{
    public class Cliente
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Nome { get; set; }
        [Required, MaxLength(11)]
        public string CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
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
        public string Email { get; set; }
    }
}

