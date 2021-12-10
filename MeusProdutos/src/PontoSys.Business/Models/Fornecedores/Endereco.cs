using PontoSys.Business.Core.Models;

namespace PontoSys.Business.Models.Fornecedores
{
    public class Endereco : Entity
    {
        public string Logradouro { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Cep { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        /* EF Relations */
        public Fornecedor Fornecedor { get; set; }
    }
}
