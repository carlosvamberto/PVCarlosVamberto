using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PVCarlosVamberto.Infra.Business;

namespace PVCarlosVamberto.Test
{
    [TestClass]
    public class NegociosTest
    {
        [TestMethod]
        public void IncluirEmpresaTest()
        {
            EmpresaBusiness eb = new EmpresaBusiness();
            int id = eb.IncluirEmpresa(new Domain.Models.Empresa
            {
                EmpresaId = 0,
                Nome = "Empresa 001 test",
                UF = "AM"
            });

            Assert.IsTrue(id > 0);
        }
    }
}
