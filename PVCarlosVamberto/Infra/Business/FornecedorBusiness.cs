using Dapper;
using DapperExtensions;
using PVCarlosVamberto.Domain.Models;
using PVCarlosVamberto.Infra.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PVCarlosVamberto.Infra.Business
{
    public class FornecedorBusiness
    {
        public int IncluirFornecedor(Fornecedor fornecedor, Empresa empresa, List<Telefone> telefones)
        {
            // Regra de Negócio
            if (empresa.UF == "PR")
            {
                bool pessoaFisica = fornecedor.CpfCnpj.Length == 11; // 11-CPF 18-CNPJ
                if (pessoaFisica)
                {
                    if (fornecedor.DataNascimento == null || string.IsNullOrWhiteSpace(fornecedor.Rg))
                    {
                        throw new Exception("RG e Data de Nascimento são obrigatórios para pessoa física.");
                    }

                    int idade = DateTime.Today.Year - fornecedor.DataNascimento.Value.Year;

                    if (idade < 18)
                    {
                        throw new Exception("Não é permitido fornecedor menor de idade para empresas do Paraná");
                    }
                }
            }

            // Banco
            // Incluindo Empresa
            Conexao conexao = new Conexao();
            conexao.Open();
            conexao.Connection.Insert(empresa);

            // Incluindo Fornecedor
            fornecedor.EmpresaId = empresa.EmpresaId;
            fornecedor.DataCadastro = DateTime.Today.Date;
            conexao.Connection.Insert(fornecedor);

            // Incluindo Telefones
            foreach (var tel in telefones)
            {
                tel.FornecedorId = fornecedor.FornecedorId;
                conexao.Connection.Insert(tel);
            }

            conexao.Close();
            return fornecedor.FornecedorId;
        }

        public Fornecedor ObterFornecedor(int fornecedorId)
        {
            Conexao conexao = new Conexao();
            conexao.Open();
            string _consulta = "SELECT * FROM Fornecedor WHERE 1=1 ";

            // Carregado Fornecedor
            Fornecedor fornecedor = conexao
                .Connection
                .Get<Fornecedor>(Predicates
                    .Field<Fornecedor>(f => f.FornecedorId, Operator.Eq, fornecedorId)
                );

            // Carregando Empresa
            fornecedor.Empresa = conexao
                .Connection.Get<Empresa>(Predicates
                    .Field<Empresa>(f => f.EmpresaId, Operator.Eq, fornecedor.EmpresaId)
                );

            // Carregando Telefones
            fornecedor.Telefones = conexao
                .Connection
                .GetList<Telefone>(Predicates
                    .Field<Telefone>(f => f.FornecedorId, Operator.Eq, fornecedorId)
                 )
                .ToList();

            conexao.Close();
            return fornecedor;
        }

        public List<Fornecedor> ConsultarFornecedor(string nome, string cpfCnpj, DateTime? dataCadastro)
        {
            List<Fornecedor> listaFornecedores = new List<Fornecedor>();
            string _consulta = "SELECT * FROM Fornecedor WHERE 1=1 ";


            //var gp = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
            
            if (!string.IsNullOrWhiteSpace(nome))
            {
                
                _consulta += " AND Nome like @Nome ";


                //gp.Predicates.Add(Predicates
                //    .Field<Fornecedor>(f => f.Nome, Operator.Like, nome)
                //);
            }

            if (!string.IsNullOrWhiteSpace(cpfCnpj))
            {
                _consulta += " AND CpfCnpj = @CpfCnpj ";
                //gp.Predicates.Add(Predicates
                //    .Field<Fornecedor>(f => f.CpfCnpj, Operator.Eq, cpfCnpj)
                //);
            }

            if (dataCadastro != null )
            {
                _consulta += " AND DataCadastro = @DataCadastro ";
                //gp.Predicates.Add(Predicates
                //    .Field<Fornecedor>(f => f.DataCadastro, Operator.Eq, dataCadastro)
                //);
            }

            
            
            Conexao conexao = new Conexao();
            conexao.Open();

            if (dataCadastro == null)
            {
                var parametros = new
                {
                    Nome = $"%{nome}%",
                    CpfCnpj = cpfCnpj
                };

                listaFornecedores = conexao
                    .Connection
                    .Query<Fornecedor>(_consulta, parametros)
                    .ToList();
            }
            else
            {
                var parametros = new
                {
                    Nome = $"%{nome}%",
                    CpfCnpj = cpfCnpj,
                    DataCadastro = dataCadastro.Value
                };

                listaFornecedores = conexao
                    .Connection
                    .Query<Fornecedor>(_consulta, parametros)
                    .ToList();
            }

            

            // DapperExtensions.DapperExtensions.DefaultMapper = typeof(FornecedorMap);
                        
            //listaFornecedores = conexao.Connection
            //    .GetList<Fornecedor>(gp)
            //    .ToList();

            

            // Obter lista de Empresas dos fornecedores
            foreach (Fornecedor fornecedor in listaFornecedores)
            {
                //DapperExtensions.DapperExtensions.DefaultMapper = typeof(EmpresaMap);

                //var predEmpresa = Predicates
                //    .Field<Empresa>(f => f.EmpresaId, Operator.Eq, fornecedor.EmpresaId);
                //fornecedor.Empresa = conexao
                //.Connection.Get<Empresa>(predEmpresa);

                fornecedor.Empresa = conexao
                    .Connection
                    .QueryFirst<Empresa>("SELECT * FROM EMPRESA WHERE EmpresaId = @EmpresaId", fornecedor);

                fornecedor.Telefones = conexao
                    .Connection
                    .Query<Telefone>("SELECT * FROM TELEFONE WHERE FornecedorId = @FornecedorId", fornecedor)
                    .ToList();

                //fornecedor.Telefones = conexao
                //    .Connection
                //    .GetList<Telefone>(Predicates
                //        .Field<Telefone>(f => f.FornecedorId, Operator.Eq, fornecedor.FornecedorId)
                //     )
                //    .ToList();
            }

            conexao.Close();
            return listaFornecedores;
        }
    }
}