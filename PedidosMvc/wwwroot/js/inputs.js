class InputDinheiro {
    constructor(seletor) {
        this.seletor = seletor;
    }

    valorConvertido = () => {
        return $(this.seletor).val().replace(',', '.');
    }

    valorExato = () => {
        return Math.round(parseFloat(this.valorConvertido()) * 100) / 100;
    }

    foiInformado = () => {
        return $(this.seletor).val() != undefined && ($(this.seletor).val() + '').trim() != '';
    }

    foiInformadoIncorretamente = () => {
        if (!this.foiInformado()) {
            return false;
        }
        return isNaN(this.valorConvertido());
    }
}

class InputNumero {
    constructor(seletor) {
        this.seletor = seletor;
    }

    valorExato = () => {
        return parseInt($(this.seletor).val())
    }

    foiInformado = () => {
        return $(this.seletor).val() != undefined && ($(this.seletor).val() + '').trim() != '';
    }

    foiInformadoIncorretamente = () => {
        if (!this.foiInformado()) {
            return false;
        }
        return isNaN(this.valorExato());
    }
}

const aplicarInputmaskDinheiro = (seletor) => {
    $(seletor).inputmask({
        mask: [ '9', '99', '9,99', '99,99', '999,99' ],
        keepStatic: true,
        jitMasking: true
    })
}

const aplicarInputmaskNumero = (seletor) => {
    $(seletor).inputmask({
        mask: '999999',
        jitMasking: true
    })
}
