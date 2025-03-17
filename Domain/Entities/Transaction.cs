using System.Text.Json.Serialization;
using DesafioUnibanco.Domain.ValueObjects;

namespace DesafioUnibanco.Domain.Entities;

public class Transaction
{
    [JsonInclude]
    private TransanctionMoment DataHora;
    [JsonInclude]
    private Value Valor;
    private DateTime CreatedAt { get; }
    
    public Transaction () {}
    
    [JsonConstructor]
    public Transaction(TransanctionMoment dataHora, Value valor)
    {
        Valor = valor;
        DataHora = dataHora;
        CreatedAt = DateTime.Now;
    }
    
    public DateTime GetDataHora() => DataHora.GetDateTime();
    public decimal GetValor() => Valor.GetValue();
    public DateTime GetCreatedAt() => CreatedAt;
    
	public override string ToString()
    {
        string formattedDataHora = DataHora.ToString();
        return $"Data: {formattedDataHora}, Valor: {Valor}";
    }
    
}