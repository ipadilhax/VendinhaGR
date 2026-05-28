namespace VendinhaGR.Dtos
{
    public class UpdateClienteDto
    {
        public string Nome {  get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Email { get; set; }

        
        public UpdateClienteDto(string nome, string cpf, DateTime dataNascimento, string email)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
            Email = email;
        }
    }
}
