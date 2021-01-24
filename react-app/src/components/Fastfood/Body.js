import React from 'react'
import PedidoItem from './PedidoItem'
import css from '../css/Body.module.css'

export default function Body({ children: pedidoList, onItemRemove, onClickNew, solicitanteId, onClickAdminToggle }) {
    const handleClickNew = () => {
        onClickNew(true)
    }

    const handleClickAdmin = () => {
        onClickAdminToggle(solicitanteId !== '')
    }

    return (
        <section className={css.pedidosSection}>
            {solicitanteId === '' && <h3>Administrador</h3>}
            <ul>
                {pedidoList.map(pedido => {
                    return <PedidoItem buttonLabel={solicitanteId !== '' ? 'Cancelar' : 'Excluir'} key={pedido.id} onItemRemove={onItemRemove}>{pedido}</PedidoItem>
                })}
            </ul>
            {solicitanteId !== '' && <button className={css.createBtn} onClick={handleClickNew}>Fazer um pedido</button>}
            <button className={css.adminBtn} onClick={handleClickAdmin}>{`Trocar para ${solicitanteId !== '' ? 'administrador' : 'usuário'}`}</button>
        </section>
    )
}
