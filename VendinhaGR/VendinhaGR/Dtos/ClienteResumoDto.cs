namespace VendinhaGR.Dtos
{
    // a gente criou esse dto de resumo pq na listagem geral nao precisa vir a lista 
    // cheia de dividas detalhadas de cada um, isso ia deixar a busca lerda
    // desse jeito a gente ja traz a idade calculada e o total que o cara deve somado direto do banco
    public class ClienteResumoDto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = null!;

        public string CPF { get; set; } = null!;

        public int Idade { get; set; }

        public string? Email { get; set; }

        public decimal TotalDividas { get; set; }
    }
}