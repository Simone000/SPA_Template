using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SharedUtilsNoReference; //todo: remove (use directly in code)

namespace SPA_Template
{
    public class CustomDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToJSData());

            //writer.WriteValue(((DateTime)value).ToString("dd/MM/yyyy hh:mm"));
        }
    }
}