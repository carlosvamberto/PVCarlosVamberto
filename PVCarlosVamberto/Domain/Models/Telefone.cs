using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PVCarlosVamberto.Domain.Models
{
    public class Telefone
    {
        
        public int TelefoneId { get; set; }
        public int FornecedorId { get; set; }
        public string Numero { get; set; }
    }
}