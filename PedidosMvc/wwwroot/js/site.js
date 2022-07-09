// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

class ModalCommon {
    constructor(seletorModal) {
        this.seletorModal = seletorModal;
        this.bsModal = new bootstrap.Modal(document.querySelector(seletorModal), {});
    }

    abrir = () => {
        this.bsModal.show();
    }

    fechar = () => {
        this.bsModal.hide();
    }
}

class AlertaModal extends ModalCommon {
    constructor(seletorModal, seletorTitulo, seletorIcone, seletorMensagem) {
        super(seletorModal);
        this.seletorTitulo = seletorTitulo;
        this.seletorIcone = seletorIcone;
        this.seletorMensagem = seletorMensagem;
        this.iconeAtual = '';
        this.callbackAposOk = () => {};
        $(this.seletorModal).on('hidden.bs.modal', () => {
            this.callbackAposOk();
        })
    }

    dispararMensagem = (titulo, icone, mensagem) => {
        $(this.seletorTitulo).text(titulo);
        $(this.seletorMensagem).text(mensagem);
        $(this.seletorIcone).removeClass(this.iconeAtual);
        $(this.seletorIcone).addClass(icone);
        this.iconeAtual = icone;
        this.abrir();
    }

    mostrarPopupHtml = (titulo, conteudoHtml) => {
        $(this.seletorTitulo).text(titulo);
        $(this.seletorMensagem).html(conteudoHtml);
        $(this.seletorIcone).removeClass(this.iconeAtual);
        this.abrir();
    }

    anularCallbackAposOk = () => {
        this.callbackAposOk = () => {};
    }

    setCallbackAposOk = (fnCallbackAposOk) => {
        this.callbackAposOk = fnCallbackAposOk;
    }
}

class ConfirmarModal extends ModalCommon {
    constructor(seletorModal, seletorTitulo, seletorMensagem) {
        super(seletorModal);
        this.seletorTitulo = seletorTitulo;
        this.seletorMensagem = seletorMensagem;
        this.callbackAposSim = () => {};
    }

    dispararConfirmacao = (titulo, mensagem) => {
        $(this.seletorTitulo).text(titulo);
        $(this.seletorMensagem).text(mensagem);
        this.abrir();
    }

    anularCallbackAposSim = () => {
        this.callbackAposSim = () => {
            this.fechar();
        };
    }

    setCallbackAposSim = (fnCallbackAposSim) => {
        this.callbackAposSim = () => {
            fnCallbackAposSim();
            this.fechar();
        };
    }
}

class LoadingModal extends ModalCommon {
    constructor(seletorModal) {
        super(seletorModal);
    }

    bloquearUi = () => {
        this.abrir();
    }

    desbloquearUi = () => {
        this.fechar();
    }
}

class Collapse {
    constructor(seletorCollapse, estaAberto) {
        this.seletorCollapse = seletorCollapse;
        if (!estaAberto) {
            this.bsCollapse = null;
        } else {
            this.bsCollapse = new bootstrap.Collapse(document.querySelector(seletorCollapse), {});
        }
    }

    abrir = () => {
        if (this.bsCollapse == null) {
            this.bsCollapse = new bootstrap.Collapse(document.querySelector(this.seletorCollapse), {});
            return;
        }
        this.bsCollapse.show();
    }

    fechar = () => {
        if (this.bsCollapse != null) {
            this.bsCollapse.hide();
        }
    }
}

const postRequest = (url, data) => {
    return $.ajax({ method: 'POST', url: url, data: JSON.stringify(data), dataType: 'json', contentType: 'application/json' });
}

const alertaModal = new AlertaModal('#alertaModal', '#tituloAlertaModal', '#iconeAlertaModal', '#mensagemAlertaModal');
const confirmarModal = new ConfirmarModal('#confirmarModal', '#tituloConfirmarModal', '#mensagemConfirmarModal');
const loadingModal = new LoadingModal('#loadingModal');

const montarIcone = (indColuna, pagViewModel) => {
    return indColuna == pagViewModel.indiceColunaAOrdenar ? (pagViewModel.ehOrdenacaoCrescente ? "bi-arrow-up-right" : "bi-arrow-down-right") : "bi-arrow-up-right invisible";
}

const listarNumeracoes = (paginaAtual, totalPaginas) => {
    if (totalPaginas < 7) {
        let todosNumeros = [];
        for (let index = 1; index <= totalPaginas; index++) {
            todosNumeros.push(index);
        }
        return todosNumeros;
    }
    let paginaPivot = Math.max(Math.min(paginaAtual, 4), totalPaginas - 3);
    let todosNumeros = [];
    todosNumeros.push(1);
    for (let index = -2; index <= 2; index++) {
        todosNumeros.push(paginaPivot + index);
    }
    todosNumeros.push(totalPaginas);
    return todosNumeros;
}

const navegarPara = (urlRelativa) => {
    loadingModal.bloquearUi();
    if (window.location.href == `${window.location.protocol}//${window.location.host}${urlRelativa}`) {
        window.location.reload();
        return;
    }
    window.location.href = `${window.location.protocol}//${window.location.host}${urlRelativa}`;
}

const listarItensPortugues = (itens) => {
    if (itens.length == 0) {
        return '';
    }
    if (itens.length == 1) {
        return itens[0];
    }
    if (itens.length == 2) {
        return `${itens[0]} e ${itens[1]}`;
    }
    let ultimosDois = `${itens[itens.length - 2]} e ${itens[itens.length - 1]}`;
    return itens
        .filter((x, index) => index < itens.length - 2)
        .reduceRight((prevs, curnt) => `${curnt}, ${prevs}`, ultimosDois);
}

const postRequestRedirectAposOk = (url, data, urlRedirect) => {
    loadingModal.bloquearUi();
    let requestAlterar = postRequest(url, data);
    requestAlterar.done((jsonResult) => {
        if (jsonResult.sucesso) {
            alertaModal.setCallbackAposOk(() => {
                navegarPara(urlRedirect);
            });
            alertaModal.dispararMensagem('Atenção!', 'text-success bi bi-check2-circle', jsonResult.mensagens[0])
        } else {
            alertaModal.anularCallbackAposOk();
            alertaModal.dispararMensagem('Atenção!', 'text-danger bi bi-x-circle', listarItensPortugues(jsonResult.mensagens));
        }
        loadingModal.desbloquearUi();
    });
}

const formatarDataJsonString = (dataJsonString) => {
    let data = new Date(dataJsonString);
    return `${(data.getDate() + '').padStart(2, '0')}/${(data.getMonth() + '').padStart(2, '0')}/${data.getFullYear()} ${(data.getHours() + '').padStart(2, '0')}:${(data.getMinutes() + '').padStart(2, '0')}:${(data.getSeconds() + '').padStart(2, '0')}`;
}

const formatarDataViewModel = (dataViewModel) => {
    let indxT = dataViewModel.indexOf('T');
    return dataViewModel.substring(0, indxT);
}

window.onunload = () => {
    loadingModal.desbloquearUi();
}