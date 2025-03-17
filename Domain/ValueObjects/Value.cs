using System.Globalization;
using System.Text.Json.Serialization;

namespace DesafioUnibanco.Domain.ValueObjects;

[JsonConverter(typeof(ValueConverter))]
public class Value
{
    private readonly decimal _value;
    
    public Value(decimal value)
    {
        ValidateValue(value);
        _value = value;
    }
    
    public decimal GetValue() => _value;
    
    private static void ValidateValue(decimal value)
    {
        if(value < 0)
        {
            throw new ArgumentException("Valor não pode ser negativo");
        }
    }
}