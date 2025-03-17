using System.Text.Json.Serialization;

namespace DesafioUnibanco.Domain.ValueObjects;

[JsonConverter(typeof(TransactionMomentConverter))]
public class TransanctionMoment
{
    private readonly DateTime _dateTime;
    
    public TransanctionMoment(DateTime dateTime)
    {
        ValidateDateTime(dateTime);
        _dateTime = dateTime;
    }
    
    public DateTime GetDateTime() => _dateTime;
    
    private static void ValidateDateTime(DateTime dateTime)
    {
        if(dateTime > DateTime.Now)
        {
            throw new ArgumentException("Data não pode ser futura");
        }
    }
    
    public override string ToString()
    {
        string formattedDateTime = _dateTime.ToString("o");
        return $"Data: {formattedDateTime}";
    }
}