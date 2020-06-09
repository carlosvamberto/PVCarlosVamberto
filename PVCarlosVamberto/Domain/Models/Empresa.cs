using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVCarlosVamberto.Domain.Models
{
    public class Empresa
    {
        
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string UF { get; set; }

    }

    public class EmpresaMap : ClassMapper<Empresa>
    {
        public EmpresaMap()
        {
            Table("Empresa");
            Map(x => x.EmpresaId).Key(KeyType.Identity);
            AutoMap();
        }
    }
}