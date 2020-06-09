using PVCarlosVamberto.Domain.Models;
using PVCarlosVamberto.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PVCarlosVamberto.ViewModel
{
    public class FornecedorCadastroViewModel
    {
        // Empresa
        public Empresa empresa { get; set; } = new Empresa();
        public Fornecedor fornecedor { get; set; } = new Fornecedor();
        public List<string> telefones { get; set; } = new List<string>();

        public Retorno Retorno { get; set; } = new Retorno();
    }
}