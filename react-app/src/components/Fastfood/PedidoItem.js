import React from 'react'
import css from '../css/Body.module.css'

export default function PedidoItem({ children: pedido, onItemRemove, buttonLabel }) {
    const handleClickClose = () => {
        onItemRemove(pedido);
    }

    return (
        <li>
            <div>
                <p>Posição na fila: #{pedido.posicao}</p>
            </div>
            <div>
                <p className={css.wrapBold}><b>Lanche</b> {pedido.lanche}</p>
                <p className={css.wrapBold}><b>Bebida</b> {pedido.bebida}</p>
            </div>
            <button onClick={handleClickClose}><i className="material-icons">close</i> {buttonLabel}</button>
        </li>
    )
}
