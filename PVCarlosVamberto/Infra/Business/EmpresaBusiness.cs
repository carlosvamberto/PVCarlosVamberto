using Dapper;
using DapperExtensions;
using PVCarlosVamberto.Domain.Models;
using PVCarlosVamberto.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PVCarlosVamberto.Infra.Business
{
    public class EmpresaBusiness
    {
        /// <summary>
        /// Insere empresa e retorna o ID gerado
        /// </summary>
        /// <param name="empresa">Objeto do Model Empresa</param>
        /// <returns>ID gerado pelo banco</returns>
        public int IncluirEmpresa(Empresa empresa)
        {
            Conexao conexao = new Conexao();
            conexao.Open();
            conexao.Connection.Insert(empresa);
            conexao.Close();
            return empresa.EmpresaId;
        }

        /// <summary>
        /// Atualiza os dados da Empresa
        /// </summary>
        /// <param name="empresa"></param>
        public void AtualizarEmpresa(Empresa empresa)
        {
            Conexao conexao = new Conexao();
            conexao.Open();
            conexao.Connection.Update(empresa);
            conexao.Close();
        }

        /// <summary>
        /// Exclui uma empresa caso ela não exista fornecedores vinculados
        /// </summary>
        /// <param name="empresa"></param>
        public void ExcluirEmpresa(Empresa empresa)
        {
            Conexao conexao = new Conexao();
            conexao.Open();
            // Verificando se existe Fornecedor vinculado a empresa
            var listaFornecedor = conexao.Connection
                .Query<Fornecedor>("SELECT * FROM FORNECEDOR where EmpresaID = @EmpresaId", empresa);
            
            if (listaFornecedor.Count() > 0)
            {
                throw new Exception("Existem fornecedores vinculados a esta empresa.\nExclusão não realizada.");
            }
            conexao.Connection.Delete(empresa);
            conexao.Close();
        }

        /// <summary>
        /// Listar empresas baseado em um filtro
        /// </summary>
        /// <param name="filtro">Parte ou Nome completo da Empresa</param>
        /// <returns>Lista de empresas</returns>
        public List<Empresa> ConsultarEmpresa(string filtro)
        {
            Conexao conexao = new Conexao();
            conexao.Open();

            var predicates = Predicates
                .Field<Empresa>(f => f.Nome, Operator.Like, filtro);

            List<Empresa> lista = conexao
                .Connection
                .GetList<Empresa>(predicates)
                .ToList();
            conexao.Close();

            return lista;
        }


    }
}