using Microsoft.AspNetCore.Mvc;
using ProfissionaisAPI.Model;
using ProfissionaisAPI.Data;

namespace ProfissionaisAPI.Controllers
{
    /*Api criada com base nas Aulas:
     docs.microsoft.com/pt-br/learn/paths/aspnet-core-minimal-api/
     docs.microsoft.com/pt-br/learn/modules/improve-api-developerexperience-with-swagger/
     */

    [ApiController]
    [Route("api/[controller]")]
    public class ProfissionaisController : ControllerBase
    {
        private readonly ProfissionalService _service;
        public ProfissionaisController(ProfissionalService service)
        {
            _service = service;
        }

        /// <summary>
        /// Este método retorna os profissionais cadastrados na base em ordem alfabética.
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        [HttpGet()]
        public List<ProfissionalBuscaViewModel> GetRangeNumeroRegistro([FromQuery] ProfissionalConsultaViewModel prof)//fromquery busca os parametros pela url
        {
            //Classe criada para validar os EndPoints de busca ->ProfissionalConsultaViewModel
            //[FromQuery] – Obtém valores da cadeia de caracteres de consulta.
            var profissional = _service.GetByNumeroRegistro(prof);

            var profResul = _service.ListProf(profissional);

            return profResul;
        }

        /// <summary>
        /// Este metódo cadastrar um Profissional na base local.
        /// </summary>
        /// <param name="newProfissional"> Cadastre um novo Profissional</param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAsync([FromBody] Profissional newProfissional)
        {
            if (!ModelState.IsValid)//Validação do ErrorMessage da classe de Profissional
            {
                return BadRequest();
            }
            DateTime dataNascimento = newProfissional.DataNascimento.Date;
            int anos = _service.CalcularMaioridade(dataNascimento);

            if (anos < 18)
            {
                return BadRequest("O profissional cadastrado não é maior de idade.");
            }


            bool retornoCPF = ValidaCPFRFB.ValidaCpf(newProfissional.cpf);//Classe statica para validação de CPF

            if (!retornoCPF)
            {
                return BadRequest("CPF inválido");
            }

            var prof = await _service.Create(newProfissional);
            return Ok($"O profissional foi cadastrado com sucesso com o id: {prof.Id}");
        }

        /// <summary>
        /// Método para alterar um profissional.
        /// </summary>
        /// <param name="id">Id:</param>
        /// <param name="newProfissional"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateProfissional(int id, [FromBody] ProfissionalViewModel newProfissional)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var profToUpdate = _service.GetById(id);
            if (profToUpdate is null)
            {
                return NotFound($"O profissional com id {id}, não foi encontrado.");
            }

            _service.UpdateProf(profToUpdate, newProfissional);

            DateTime dataNascimento = newProfissional.DataNascimento.Date;
            int anos = _service.CalcularMaioridade(dataNascimento);

            if (anos < 18)
            {
                return BadRequest("O profissional alterado, não é maior de idade.");
            }

            if (profToUpdate is not null)
            {
                _service.UpdateProfissional(id);
                return Ok($"Profissional com id {id} alterado com sucesso.");
            }
            else
            {
                return NotFound();//BadRequest();
            }
        }
        /// <summary>
        /// Método para deletar um Profissional.
        /// </summary>
        /// <param name="id">Id:</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var profissional = _service.GetById(id);

            if (profissional is not null)
            {
                _service.DeleteById(id);
                return Ok($"Profissional com id {id} deletado com sucesso.");
            }
            else
            {
                return NotFound($"O Profissional com o id {id} não foi encontrado.");//NoContent();
            }

        }

    }
}
