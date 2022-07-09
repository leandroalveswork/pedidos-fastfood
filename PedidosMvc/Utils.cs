namespace PedidosMvc;
public static class Utils
{
    public static string RemoverAcentos(this string texto)
    {
        string comAcento = "áàãâäéèêëíìîïóòõôöúùûüñÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÔÖÚÙÛÜÑ";
        string semAcento = "aaaaaeeeeiiiiooooouuuunAAAAAEEEEIIIIOOOOOUUUUN";
        string textoSemAcentos = texto;
        for (int i = 0; i < comAcento.Length; i++)
        {
            textoSemAcentos = textoSemAcentos.Replace(comAcento[i], semAcento[i]);
        }
        return textoSemAcentos;
    }
}