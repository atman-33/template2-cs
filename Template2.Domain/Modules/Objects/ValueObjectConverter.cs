using System.Text.Json.Serialization;
using System.Text.Json;
using Template2.Domain.ValueObjects;

namespace Template2.Domain.Modules.Objects
{
    public class ValueObjectConverter<T> : JsonConverter<T> where T : ValueObject<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //// ValueObjectは読み取り専用なので、デシリアライズはサポートされていません。
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}

