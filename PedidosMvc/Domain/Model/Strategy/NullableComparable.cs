namespace PedidosMvc.Domain.Model.Strategy;
public class NullableComparable : IComparable
{
    public NullableComparable()
    {
    }
    public NullableComparable(IComparable? value)
    {
        Value = value;
    }
    public IComparable? Value { get; set; }

    public int CompareTo(object? obj)
    {
        if (Value == default)
        {
            return -1;
        }
        return Value.CompareTo(obj);
    }
}