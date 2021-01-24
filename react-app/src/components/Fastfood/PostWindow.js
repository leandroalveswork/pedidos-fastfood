import React from 'react'
import css from '../css/Window.module.css'

export default function PostWindow({ children: pedidoForm, onClickBack, onChangeSelect, onPostPedido }) {
    const handleClickBack = () => {
        onClickBack(false);
    }

    const handleChangeForm = ({ target }) => {
        onChangeSelect(target.id, target.value)
    }

    const handleClickPost = () => {
        onPostPedido(pedidoForm);
    }

    return (
        <section className={css.postSection}>
            <button onClick={handleClickBack} className={css.btnBack}><i className="material-icons">chevron_left</i>Voltar</button>

            <div className={css.formDiv}>
                <div className={css.wrapInput}>
                    <p><b>Lanche</b></p>
                    <select value={pedidoForm.lanche} id="lanche" onChange={handleChangeForm}>
                        <option value="Duplo Cheddar">Duplo Cheddar</option>
                        <option value="X-Salada">X-Salada</option>
                        <option value="Whooper">Whooper</option>
                    </select>
                </div>
                <div className={css.wrapInput}>
                    <p><b>Bebida</b></p>
                    <select value={pedidoForm.bebida} id="bebida" onChange={handleChangeForm}>
                        <option value="Coca-Cola">Coca-Cola</option>
                        <option value="Pepsi">Pepsi</option>
                        <option value="Guaraná">Guaraná</option>
                    </select>
                </div>
                <button className={css.btnCreate} onClick={handleClickPost}>Fazer pedido</button>
            </div>
        </section>
    )
}
