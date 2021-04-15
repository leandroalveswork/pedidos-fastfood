import React from 'react';
import { deletePedido, fetchJson, postPedido, fetchJsonFiltered }from './api/ApiService';
import Body from './components/Fastfood/Body';
import Header from './components/Fastfood/Header';
import PostWindow from './components/Fastfood/PostWindow';
import generateRandomCode from './helpers/GenerateRandomCode';
import css from './components/css/Main.module.css'

export default function App() {
  const TIME_INTERVAL = 3000

  const [refreshInterval, setRefreshInterval] = React.useState({});
  const [filteredPedidos, setFilteredPedidos] = React.useState([]);
  const [postOpen, setPostOpen] = React.useState(false)
  const [userId, setUserId] = React.useState(generateRandomCode())
  const [pedidoForm, setPedidoForm] = React.useState({
    solicitanteId: userId,
    lanche: "Cheddar Duplo",
    bebida: "Coca-Cola"
  })

  React.useEffect(() => {
    if (userId === '') {
      clearInterval(refreshInterval);
      (async () => {
        setFilteredPedidos((await fetchJson()).sort((a, b) => a.posicao - b.posicao));
      })()
      setRefreshInterval(setInterval(async () => {
        setFilteredPedidos((await fetchJson()).sort((a, b) => a.posicao - b.posicao));
      }, TIME_INTERVAL))
    } else {
      clearInterval(refreshInterval);
      (async () => {
        setFilteredPedidos((await fetchJsonFiltered(userId)).sort((a, b) => a.posicao - b.posicao));
      })()
      setRefreshInterval(setInterval(async () => {
        setFilteredPedidos((await fetchJsonFiltered(userId)).sort((a, b) => a.posicao - b.posicao));
      }, TIME_INTERVAL))
    }
  }, [userId])

  const handleItemRemove = async (pedido) => {
    await deletePedido(pedido.id);
    setFilteredPedidos(filteredPedidos
      .filter(item => item.id !== pedido.id)
      .map(item => item.posicao > pedido.posicao ? {...item, posicao: item.posicao - 1} : item))
  }

  const changeWindow = open => {
    setPostOpen(open)
  }

  const handleChangeSelect = (propertyName, propertyValue) => {
    setPedidoForm({...pedidoForm, [propertyName]: propertyValue})
  }

  const handlePostPedido = async (pedido) => {
    const response = await postPedido(pedido);
    const pedidoReturned = response.data
    setFilteredPedidos([...filteredPedidos, {
      id: pedidoReturned.id,
      posicao: pedidoReturned.posicao,
      lanche: pedidoReturned.lanche,
      bebida: pedidoReturned.bebida
    }]);
    changeWindow(false);
  }

  const handleAdminToggle = (hasAdmin) => {
    const getCode = generateRandomCode()
    setUserId(hasAdmin ? '' : getCode);
    setFilteredPedidos([])
    if (!hasAdmin) {
      setPedidoForm({
        solicitanteId: getCode,
        lanche: "Cheddar Duplo",
        bebida: "Coca-Cola"
      })
    }
  }

  return (
    <div className={css.appContainer}>
      <Header />
      {!postOpen && <Body solicitanteId={userId} onItemRemove={handleItemRemove} onClickNew={changeWindow} onClickAdminToggle={handleAdminToggle}>{filteredPedidos}</Body>}
      {postOpen && <PostWindow onClickBack={changeWindow} onChangeSelect={handleChangeSelect} onPostPedido={handlePostPedido}>{pedidoForm}</PostWindow>}
    </div>
  );
}
