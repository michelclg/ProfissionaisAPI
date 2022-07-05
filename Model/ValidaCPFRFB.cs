namespace ProfissionaisAPI.Model
{
    public static class ValidaCPFRFB
    {
        /*Código foi criado com base do escrito na receita:
        www.receita.fazenda.gov.br/aplicacoes/atcta/cpf/funcoes.js */
        public static bool ValidaCpf(string strCPF)
        {
            int resultado = 0;
            int soma = 0;
            int valorTotal = 0;
            if (strCPF.Equals("00000000000") ||
                strCPF.Equals("11111111111") ||
                strCPF.Equals("22222222222") ||
                strCPF.Equals("44444444444") ||
                strCPF.Equals("55555555555") ||
                strCPF.Equals("66666666666") ||
                strCPF.Equals("77777777777") ||
                strCPF.Equals("88888888888") || 
                strCPF.Equals("99999999999")) return false;

            {//Primeiramente multiplica-se os 9 primeiros dígitos pela sequência decrescente de números de 10 à 2 e soma os resultados
                for (int i = 1; i <= 9; i++)
                {
                    int number = Int32.Parse(strCPF.Substring(i - 1, 1));
                    resultado = number * (11 - i);
                    soma += resultado;
                }
                valorTotal = (soma * 10) % 11;
                //O resultado que nos interessa é o RESTO da divisão.
                //Se ele for igual ao primeiro dígito verificador (primeiro dígito depois do '-') a validação está correta

                if ((valorTotal == 10) || (valorTotal == 11)) valorTotal = 0;

                if (valorTotal != Int32.Parse(strCPF.Substring(9, 1))) return false;

                soma = 0;
                valorTotal = 0;
                //A validação do segundo dígito é semelhante à primeira, porém vamos considerar os 9 primeiros dígitos,
                //mais o primeiro dígito verificador, e vamos multiplicar esses 10 números pela sequencia decrescente de 11 a 2.
                for (int i = 1; i <= 10; i++)
                {
                    int valor = Int32.Parse(strCPF.Substring(i - 1, 1));
                    int result = valor * (12 - i);
                    soma += result;
                }
                valorTotal = (soma * 10) % 11;

                if ((valorTotal == 10) || (valorTotal == 11)) valorTotal = 0;

                if (valorTotal != Int32.Parse(strCPF.Substring(10, 1))) return false;

                return true;
            }

        }
    }
}
