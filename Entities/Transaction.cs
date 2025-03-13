using System;
using System.Text.Json.Serialization;

namespace DesafioUnibanco.Entities;

public class Transaction
{

    public DateTime DataHora { get; }
    public double Valor { get; }
    
    public Transaction () {}
    
    [JsonConstructor]
    public Transaction(DateTime dataHora, double valor)
    {
		ValidateFields(dataHora, valor);
        Valor = valor;
        DataHora = dataHora;
    }

	public override string ToString()
    {
        string formattedDataHora = DataHora.ToString("o");
        return $"Data: {formattedDataHora}, Valor: {Valor}";
    }
    
	private static void ValidateFields(DateTime dataHora, double valor)
    {
        if(valor < 0)
        {
            throw new ArgumentException("Valor não pode ser negativo");
        }

        if(dataHora > DateTime.Now)
        {
            throw new ArgumentException("Data não pode ser futura");
        }
    }
    
}