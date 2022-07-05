using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProfissionaisAPI.Model
{
    public class Profissional
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(300)]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string cpf { get; set; }

        [Required(ErrorMessage = "A Data de nascimento é obrigatório.")]
        [DefaultValue("2004-01-01")]//valor padrão para maioridade
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O sexo é obrigatório e pode conter apenas um caractere.")]
        [MaxLength(1)]
        [DefaultValue("M")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "O campo ativo é obrigatório.")]
        public bool Ativo { get; set; }

        [JsonIgnore]
        public int NumeroRegistro { get; set; }

        public string Cep { get; set; }

        public string cidade { get; set; }

        public decimal ValorRenda { get; set; }

        [JsonIgnore]
        public DateTime DataCriacao { get; set; }
    }
}
