using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProfissionaisAPI.Model
{
    public class ProfissionalViewModel 
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(300)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string cpf { get; set; }

        [Required(ErrorMessage = "A Data de nascimento é obrigatório.")]
        [DefaultValue("2004-01-01")]//valor padrão para maioridade
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O sexo é obrigatório e pode conter apenas um caracter.")]
        [MaxLength(1)]
        [DefaultValue("M")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "O campo ativo é obrigatório.")]
        public bool Ativo { get; set; }
        public decimal ValorRenda { get; set; }

        [MaxLength(8)]
        public string Cep { get; set; }

        [MaxLength(300)]
        public string cidade { get; set; }

        [JsonIgnore]
        public int NumeroRegistro { get; set; }

        [JsonIgnore]
        public DateTime DataCriacao { get; set; }
    }
}
