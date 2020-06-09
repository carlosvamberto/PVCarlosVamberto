using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Dapper;
using DapperExtensions;
using DapperExtensions.Mapper;

namespace PVCarlosVamberto.Domain.Models
{
   
    public class Fornecedor
    {       
        
        public int FornecedorId { get; set; }
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Rg { get; set; }
        public Nullable<DateTime> DataNascimento { get; set; }

        // Relações        

        public virtual Empresa Empresa { get; set; } = new Empresa();

        public virtual List<Telefone> Telefones { get; set; } = new List<Telefone>();

    }

    public sealed class FornecedorMap : ClassMapper<Fornecedor>
    {
        public FornecedorMap()
        {            
            Table("Fornecedor");
            Map(f => f.Empresa).Ignore();
            Map(f => f.Telefones).Ignore();
            AutoMap();
        }
    }

}