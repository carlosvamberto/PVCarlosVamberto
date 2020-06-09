using PVCarlosVamberto.Domain.DTO;
using PVCarlosVamberto.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVCarlosVamberto.ViewModel
{
    public class FornecedorListaViewModel
    {
        public Retorno Retorno { get; set; }

        // Filtro
        public string filtroNome { get; set; }
        public string filtroCpfCnpj { get; set; }
        public string filtroDtCadastro { get; set; }

        public List<Fornecedor> lisgagem { get; set; }
    }
}