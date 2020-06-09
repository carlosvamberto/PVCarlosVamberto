
/// <reference path="../typings/bootstrap/index.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />

declare var $: JQueryStatic;

class FornecedorCadastroClass {
    btnGravar = $("#btnGravar");
    btnCancelar = $("#btnCancelar");
    btnAddTel = $("#btnAdicionarTel");

    divFornCpfCnpj = $("#div_FornCpfCnpj");
    divFornDtNasc = $("#div_FornDtNasc");
    divFornRg = $("#div_FornRg")
    divAlerta = $("#divAlerta");
    labMensagemAlerta = $("#labErro");

    txtFornNome = $("#fornecedor_nome");
    txtFornCpfCnpj = $("#fornecedor_cpfcnpj")
    txtFornDataNasc = $("#fornecedor_datanascimento");
    txtFornRg = $("#fornecedor_rg");
    txtTelefone = $("#txtTelefone");

    txtEmpresaNome = $("#empresa_nome");
    txtEmpresaUf = $("#empresa_uf");

    form = $("#formCadastro");

    qtd = 0;

    init(): void {

        this.divAlerta.hide();
        this.divFornDtNasc.hide();
        this.divFornRg.hide();

        // Regra de negócio da Empresa
        this.btnGravar.on("click", () => {
            if (this.validaForm() === true) {
                this.form.submit();
            }
        });

        this.txtFornCpfCnpj.on("change", () => {
            if (this.ehPessoaFisica()) {
                this.divFornDtNasc.show();
                this.divFornRg.show();
            }
            else {
                this.divFornDtNasc.hide();
                this.divFornRg.hide();
            }
        });

        this.btnAddTel.on("click", () => {
            if (this.txtTelefone.val().trim() === "") {
                this.labMensagemAlerta.text("Precisa digitar um número para adicionar a lista.");
                this.divAlerta.show(500);
                this.divAlerta.delay(3000).fadeOut();
            }
            else {

                const inputtext = "<input type='hidden' name='telefones' value='" + this.txtTelefone.val() + "' />";
                const linha = "<tr><td style = 'vertical-align:middle'> " + this.txtTelefone.val() + inputtext
                    + " </td><td style='text-align:right'><button type='button' class='btn btn-sm btn-danger btnsExcluir'>"
                    + "excluir </button></td > </tr>";
                $('#tabTelefones > tbody:last-child').append(linha);

                this.qtd++;
                this.txtTelefone.val("");

                this.adicionaEventoExcluir();
            }
        });
    }

    adicionaEventoExcluir(): void {
        $(".btnsExcluir").on("click", (e: JQueryEventObject) => {
            $(e.currentTarget).closest("tr").remove();
        });
    }

    validaEstado(): boolean {
        // Caso a empresa seja do Paraná, não permitir cadastrar um fornecedor pessoa física menor de idade

        if (this.txtEmpresaUf.val() === "PR") {            
            let anoNacimento:number = parseInt(this.txtFornDataNasc.val().substring(0, 4));
            let hoje:Date = new Date();
            let ano:number = hoje.getFullYear();
            let idade:number = ano - anoNacimento;

            if (idade < 18) {
                // Verifica se é pessoa fisica
                if (this.ehPessoaFisica() && idade < 18) {
                    return false;        
                }
            }            
        }
        return true;
    }

    validaCamposObrigatorios(): boolean {
        let retorno: boolean = true;

        if (this.txtEmpresaNome.val().trim() === "") {
            retorno = false;
        }

        if (this.txtEmpresaUf.val().trim() === "") {
            retorno = false;
        }

        if (this.txtFornNome.val().trim() === "") {
            retorno = false;
        }

        if (this.txtFornCpfCnpj.val().trim() === "") {
            retorno = false;
        }

        if (this.txtFornCpfCnpj.val().trim().length === 11 && (this.txtFornRg.val().trim() === "" || this.txtFornDataNasc.val().trim() === "")) {
            retorno = false;
        }

        return retorno;
    }

    validaPessoaFisica(): boolean {
        // Caso o fornecedor seja pessoa física, também é necessário cadastrar o RG e a data de nascimento
        if (this.ehPessoaFisica() && (this.txtFornRg.val().length === 0 || this.txtFornDataNasc.val() === "")) {
            return false;
        }
        return true;
    }

    ehPessoaFisica(): boolean {
        // usando 11 digitos para pessoa fisica sem pontos e sem barra
        if (this.txtFornCpfCnpj.val().length === 11) {
            return true;
        }
        else {
            return false;
        }
    }

    validaForm(): boolean {
        let mensagem = "";

        if (!this.validaCamposObrigatorios()) {
            mensagem = " - Existe campos obrigatórios que não foram preenchidos";
        }

        if (!this.validaPessoaFisica()) {
            if (mensagem !== "") mensagem += "<br/>";
            mensagem = " - Para Pessoa Física, é obrigatório digitar RG e Data de Nascimento.";
        }

        if (!this.validaEstado()) {
            if (mensagem !== "") mensagem += "<br/>";
            mensagem += " - Para fornecedores pessoa física de empresas do Paraná, é necessário que seja maior de idade.";
        }

        if (mensagem !== "") {
            this.labMensagemAlerta.text(mensagem);
            this.divAlerta.show(500);
            this.divAlerta.delay(5000).fadeOut();
            return false;
        }
        else {
            this.divAlerta.hide();
            return true;
        }
    }
}

// Inicializa quando a página carregar
window.onload = () => {
    const obj = new FornecedorCadastroClass();
    obj.init();
}