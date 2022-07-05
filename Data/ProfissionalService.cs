using Microsoft.EntityFrameworkCore;
using ProfissionaisAPI.Excecoes;
using ProfissionaisAPI.Model;

namespace ProfissionaisAPI.Data
{
    public class ProfissionalService
    {
        private readonly ProfissionalContext _context;

        public ProfissionalService(ProfissionalContext context)
        {
            _context = context;
        }

        public List<Profissional> GetAll()//Esse método traz todos os profissionais em ordem alfabética

        {
            return _context.Profissionais.AsNoTracking().OrderBy(x => x.NomeCompleto).ToList();
        }
        public List<Profissional> GetProfCadastrados()//Criei esse método para não trazer ordenado e pegar o último registro cadastrado

        {
            return _context.Profissionais.AsNoTracking().ToList();
        }

        public Profissional? GetById(int id)
        {
            return _context.Profissionais.Find(id) is not null ? _context.Profissionais.Find(id) : null;

        }

        public async Task<Profissional> Create(Profissional newProfissional)
        {
            newProfissional.DataCriacao = DateTime.Now;
            List<Profissional> validaProf = GetProfCadastrados();
            Profissional count = validaProf.Last();
            newProfissional.NumeroRegistro = ++count.NumeroRegistro;

            foreach (var prof in validaProf)
            {
                bool result = prof.NomeCompleto is not null ? prof.NomeCompleto.Equals(newProfissional.NomeCompleto) : false;
                if (result)
                    throw new validaProfissionalException($"Esse profissional já foi cadastrado com o id: {prof.Id}");

            }

            _context.Profissionais.Add(newProfissional);
            await _context.SaveChangesAsync();

            return newProfissional;
        }

        internal List<ProfissionalBuscaViewModel> ListProf(IEnumerable<Profissional> profissional)
        {
            List<Profissional> profResult = profissional.ToList();//retorno da lista de Profissional
            var result = new List<ProfissionalBuscaViewModel>();
            //aqui estou add um novo objeto a lista para retorno dos campos da view ProfissionalBuscaViewModel
            foreach (var prof in profResult) 
            {
                result.Add(new ProfissionalBuscaViewModel(prof.NumeroRegistro, prof.NomeCompleto, prof.cpf, prof.Ativo, prof.DataCriacao) { });
            }
            return result;
        }

        internal IQueryable<Profissional> GetByNumeroRegistro(ProfissionalConsultaViewModel prof)
        {
            //Aqui retornei a lista de Prof como IQueryable e verifiquei quais campos estão nulos para criar a consulta no banco.
            var retornoBanco = _context.Profissionais.AsQueryable().OrderBy(n => n.NomeCompleto);

            if (prof.nome is not null)
            {
                retornoBanco = retornoBanco.Where(p => p.NomeCompleto.Contains(prof.nome)).OrderBy(n => n.NomeCompleto);
            }
            if (prof.numeroRegistroInicial is not null)
            {
                retornoBanco = retornoBanco.Where(p => p.NumeroRegistro >= prof.numeroRegistroInicial).OrderBy(n => n.NomeCompleto);
            }
            if (prof.numeroRegistroFinal is not null)
            {
                retornoBanco = retornoBanco.Where(p => p.NumeroRegistro <= prof.numeroRegistroFinal).OrderBy(n => n.NomeCompleto);
            }
            if (prof.ativo is not null)
            {
                retornoBanco = retornoBanco.Where(p => p.Ativo == prof.ativo).OrderBy(n => n.NomeCompleto);
            }

            return retornoBanco;
        }

        public void UpdateProfissional(int profId)
        {
            var ProfToUpdate = _context.Profissionais.Find(profId);//recupero o profissional por id

            if (ProfToUpdate is null)
            {
                throw new NullReferenceException("Profissional não existe");
            }
            List<Profissional> validaProf = GetProfCadastrados();

            foreach (var prof in validaProf)
            {
                bool result = prof.NomeCompleto.Equals(ProfToUpdate.NomeCompleto);
                if (result)
                    throw new validaProfissionalException($"Esse profissional já foi cadastrado com o id: {prof.Id}");

            }
            _context.SaveChanges();
        }

        internal void UpdateProf(Profissional profToUpdate, ProfissionalViewModel newProfissional)
        {
            profToUpdate.NomeCompleto = newProfissional.NomeCompleto;
            profToUpdate.cpf = newProfissional.cpf;
            profToUpdate.DataNascimento = newProfissional.DataNascimento;
            profToUpdate.Sexo = newProfissional.Sexo;
            profToUpdate.Ativo = newProfissional.Ativo;
            profToUpdate.ValorRenda = newProfissional.ValorRenda;
            profToUpdate.Cep = newProfissional.Cep;
            profToUpdate.cidade = newProfissional.cidade;
            profToUpdate.DataCriacao = DateTime.Now;
        }

        internal int CalcularMaioridade(DateTime dataNascimento)
        {//docs.microsoft.com/pt-br/dotnet/api/system.datetime?view=net-6.0

            //Foi substraído da data atual a data de nascimento.
            TimeSpan dataAgora = (DateTime.Now - dataNascimento);
            //por padrão o DateTime coloca a data 1/1/0001.Somei à data o valor da subtração. Retornou 19 anos. Não sei pq!
            DateTime idade = (new DateTime() + dataAgora).AddYears(-1);//Retirei um ano

            return idade.Year;

        }

        public void DeleteById(int id)
        {
            var profToDelete = _context.Profissionais.Find(id);
            if (profToDelete is not null)
            {
                _context.Profissionais.Remove(profToDelete);
                _context.SaveChanges();
            }
        }
    }
}
