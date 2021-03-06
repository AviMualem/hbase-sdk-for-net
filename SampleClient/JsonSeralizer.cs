﻿using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;


namespace SampleClient
{
    public class JsonDotNetSerializer 
    {
     private static JsonSerializer GetSerializerInstance()
        {
            var jsonSerializer = new JsonSerializer()
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            jsonSerializer.Converters.Add(new IsoDateTimeConverter());
            return jsonSerializer;
        }

        /// <summary>
        /// Serializes a specific object into a BSON format.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public byte[] SerializeToBson(object obj)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BsonWriter(stream))
                {
                    var jsonSerializer = GetSerializerInstance();
                    jsonSerializer.Serialize(writer, obj);
                    return stream.ToArray();
                }
            }
        }


        /// <summary>
        /// Deseralizes from bson.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bsonContent">Content of the bson.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public T DeseralizeFromBson<T>(byte[] bsonContent)
        {
            using (var stream = new MemoryStream(bsonContent))
            {
                using (var reader = new BsonReader(stream))
                {
                    var jsonSerializer = GetSerializerInstance();
                    return (T)jsonSerializer.Deserialize(reader, typeof(T));
                }
            }
        }

        /// <summary>
        /// Serializes a specified object into a JSON format.
        /// </summary>
        /// <param name="obj">Serialization object.</param>
        /// <returns></returns>
        string SerializeToJson(object obj)
        {
            var sb = new StringBuilder(256);
            using (var writer = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    var jsonSerializer = GetSerializerInstance();
                    jsonSerializer.Serialize(jsonWriter, obj);
                    return writer.ToString();
                }
            }
        }

        /// <summary>
        /// Performs JSON Deserialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonContent">JSON content</param>
        /// <returns></returns>
        public object DeseralizeFromJson<T>(string jsonContent)
        {
            using (var reader = new StringReader(jsonContent))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var jsonSerializer = GetSerializerInstance();
                    return jsonSerializer.Deserialize(jsonReader, typeof(T));
                }
            }
        }
    
    }
}
