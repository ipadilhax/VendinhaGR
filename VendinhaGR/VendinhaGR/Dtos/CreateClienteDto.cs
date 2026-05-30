using System;

namespace VendinhaGR.Dtos
{
    public class CreateClienteDto
    {
        public string Nome { get; set; }

        public string CPF { get; set; }

        public DateTime DataNascimento { get; set; }

        public string? Email { get; set; }

        public CreateClienteDto(string nome, string cpf, DateTime dataNascimento, string? email)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
            Email = email;
        }
    }
}