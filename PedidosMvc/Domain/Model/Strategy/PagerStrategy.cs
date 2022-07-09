namespace PedidosMvc.Domain.Model.Strategy;
public class PagerStrategy
{
    public IEnumerable<TResult> Apply<TResult>(IEnumerable<TResult> results, int page, int resultsPerPage, out int totalPages, out int pageInRange)
    {
        totalPages = Math.Max(Convert.ToInt32(Math.Ceiling((double)(results.Count()) / resultsPerPage)), 1);
        pageInRange = Math.Min(Math.Max(page, 1), totalPages);
        return results.Skip((pageInRange - 1) * resultsPerPage).Take(resultsPerPage);
    }
}