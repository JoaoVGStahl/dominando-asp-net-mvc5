using PontoSys.Business.Core.Models;
using PontoSys.Business.Models.Fornecedores.Validations;
using PontoSys.Business.Models.Produtos;
using System.Collections;
using System.Collections.Generic;

namespace PontoSys.Business.Models.Fornecedores
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }

        public string Documento { get; set; }

        public TipoFornecedor TipoFornecedor { get; set; }

        public Endereco Endereco { get; set; }

        public bool Ativo { get; set; }

        /* EF  Relation*/
        public ICollection<Produto> Produtos { get; set; }

    }
}
