namespace ProfissionaisAPI.Model
{
    public class ProfissionalBuscaViewModel
    {
        public int NumeroRegistro { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }

        public ProfissionalBuscaViewModel(int NumeroRegistro, string NomeCompleto, string cpf, bool Ativo, DateTime DataCriacao)
        {
            this.NumeroRegistro = NumeroRegistro;
            this.NomeCompleto = NomeCompleto;
            this.Cpf = cpf;
            this.Ativo = Ativo;
            this.DataCriacao = DataCriacao;

        }

    }
}
