import axios from 'axios';

const commonUrl = 'http://localhost:5001/api/pedidos'

const fetchJson = async () => {
    const jsonRes = await axios.get(commonUrl);
    return jsonRes.data;
}

const fetchJsonFiltered = async (solicitanteId) => {
    const url = `${commonUrl}/${solicitanteId}`
    const jsonRes = await axios.get(url);
    return jsonRes.data;
}

const postPedido = async (postPedido) => {
    return await axios.post(commonUrl, postPedido);
}

const deletePedido = async (id) => {
    const url = `${commonUrl}/${id}`
    return (await axios.delete(url)).status;
}

export { fetchJson, fetchJsonFiltered, postPedido, deletePedido }