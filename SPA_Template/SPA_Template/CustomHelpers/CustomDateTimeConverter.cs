using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Globalization;

namespace SPA_TemplateHelpers
{
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //Handling DateTime? null value
            if (objectType == typeof(DateTime?) && string.IsNullOrEmpty(reader.Value.ToString()))
                return null;

            return DateTime.ParseExact(reader.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //writer.WriteValue(((DateTime)value).ToJSData());

            writer.WriteValue(((DateTime)value).Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local))
                                               .TotalMilliseconds);
        }
    }
}