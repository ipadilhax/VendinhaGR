namespace VendinhaGR.Dtos
{
    public class UpdateClienteDto
    {
        public int Id { get; set; }
        public string Nome {  get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string? Email { get; set; }

        
        public UpdateClienteDto(int id, string nome, string cpf, DateTime dataNascimento, string email)
        {
            Id = id;
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
            Email = email;
        }
    }
}
