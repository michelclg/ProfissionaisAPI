namespace ProfissionaisAPI.Excecoes
{
    public class ValidaIdadeException : Exception
    {
        ValidaIdadeException(string mensagem)  : base(mensagem) { } 
    }
}
