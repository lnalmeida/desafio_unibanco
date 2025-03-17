using System.Text.Json;
using System.Text.Json.Serialization;

namespace DesafioUnibanco.Domain.ValueObjects;

public class TransactionMomentConverter : JsonConverter<TransanctionMoment>
{
    public override TransanctionMoment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var transanctionMoment = reader.GetDateTime();
        return new TransanctionMoment(transanctionMoment);
    }

    public override void Write(Utf8JsonWriter writer, TransanctionMoment value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class ValueConverter : JsonConverter<Value>
{
    public override Value? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetDecimal();
        return new Value(value);
    }

    public override void Write(Utf8JsonWriter writer, Value value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.GetValue());
    }
}