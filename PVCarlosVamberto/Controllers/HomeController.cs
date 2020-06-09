using PVCarlosVamberto.Infra.Business;
using PVCarlosVamberto.ViewModel;
using System;
using System.Web.Mvc;

namespace PVCarlosVamberto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new ViewModel.FornecedorListaViewModel());
        }

        [HttpGet]
        public ActionResult Index(FornecedorListaViewModel vm)
        {
            FornecedorBusiness fornecedorBusiness = new FornecedorBusiness();

            DateTime? filDtCadastro = null;
            if (!string.IsNullOrWhiteSpace(vm.filtroDtCadastro))
            {
                try
                {
                    string[] split = vm.filtroDtCadastro.Split('-');
                    filDtCadastro = new DateTime(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]));
                }
                catch 
                {
                    vm.Retorno = new Domain.DTO.Retorno();
                    vm.Retorno.ErroMensagem = "Data inválida. Use o formato aaaa-mm-dd.";
                    return View(vm);
                }
            }

            vm.lisgagem = fornecedorBusiness
                .ConsultarFornecedor(vm.filtroNome, vm.filtroCpfCnpj, filDtCadastro);

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Carlos Vamberto de Araujo Martins Filho";
            return View();
        }

        public ActionResult Cadastro()
        {            
            return View(new FornecedorCadastroViewModel());
        }

        [HttpPost]
        public ActionResult Cadastro(FornecedorCadastroViewModel vm)
        {
            // Regras de negócio do lado do BackEnd
            // Campos Obrigatórios
            if (vm.fornecedor == null)
            {
                vm.Retorno.ErroMensagem = "Fornecedor não foi preenchido.";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(vm.fornecedor.Nome) 
                    || string.IsNullOrWhiteSpace(vm.fornecedor.CpfCnpj)
                    || (vm.fornecedor.CpfCnpj.Length == 1 && (string.IsNullOrWhiteSpace(vm.fornecedor.Rg) || vm.fornecedor.DataNascimento == null))
                    || string.IsNullOrWhiteSpace(vm.empresa.Nome)
                    || string.IsNullOrWhiteSpace(vm.empresa.UF)
                    )
                {
                    vm.Retorno.ErroMensagem = "Campos obrigatórios não foram preenchidos.";
                    return View(vm);
                }
            }

            // Regra do Estado
            if (vm.empresa.UF == "PR")
            {
                int idade = DateTime.Today.Year - vm.fornecedor.DataNascimento.Value.Year;
                if (idade < 18)
                {
                    vm.Retorno.ErroMensagem = "Para empresas do Paraná, o fornecedor pessoa física deve ser maior de idade.";
                    return View(vm);
                }
            }

            FornecedorBusiness bs = new FornecedorBusiness();

            foreach (var tel in vm.telefones)
            {
                vm.fornecedor.Telefones.Add(new Domain.Models.Telefone
                {
                    Numero = tel
                });
            }

            int _fornecedorId = bs.IncluirFornecedor(vm.fornecedor, vm.empresa, vm.fornecedor.Telefones);

            if (_fornecedorId == 0)
            {
                vm.Retorno.ErroMensagem = "Erro no banco ao gerar o cadastro.";
                return View(vm);
            }
            else
            {
                vm.Retorno.SucessoMensagem = "Sucesso na gravação";
                TempData["SUCESSO"] = "Sucesso na gravação.";
                return RedirectToAction("Index");
            }            
        }
    }
}