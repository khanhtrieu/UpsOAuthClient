using System.Dynamic;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace UpsOAuthClient.Extensions {
  public class JsonSerialization {
    /// <summary>
    ///   Gets the default <see cref="Newtonsoft.Json.JsonSerializerSettings"/> to use for deserialization
    /// </summary>
    private static JsonSerializerSettings DefaultJsonSerializerSettings => new() {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      DateFormatHandling = DateFormatHandling.IsoDateFormat,
      DateTimeZoneHandling = DateTimeZoneHandling.Utc,
    };


    /// <summary>
    ///   Deserialize a JSON string into a T-type object.
    /// </summary>
    /// <typeparam name="T">Type of object to deserialize to</typeparam>
    /// <param name="data">A string of JSON data.</param>
    /// <param name="jsonSerializerSettings">
    ///   The <see cref="Newtonsoft.Json.JsonSerializerSettings" /> to use for
    ///   deserialization. Defaults to <see cref="DefaultJsonSerializerSettings" /> if not provided.
    /// </param>
    /// <param name="rootElementKeys">
    ///   List, in order, of sub-keys path to follow to 
    ///   deserialization starting position.</param>
    /// <returns>A T-type object.</returns>
    /// <exception cref="JsonDeserializationError">Error convert JSON string to T-type object.</exception>
    internal static T ConvertJsonToObject<T>(string? data,
      JsonSerializerSettings? jsonSerializerSettings = null,
      List<string>? rootElementKeys = null) where T : class {
      object obj = ConvertJsonToObject(data, typeof(T), jsonSerializerSettings, rootElementKeys);
      return obj is T t ? t : throw new Exceptions.JsonDeserializationError(typeof(T));
    }

    /// <summary>
    /// Deserialize a JSON string into a T-type object.
    /// </summary>
    /// <param name="data">A string of JSON data.</param>
    /// <param name="type">Type of object to deserialize to.</param>
    /// <param name="jsonSerializerSettings">
    ///   The <see cref="Newtonsoft.Json.JsonSerializerSettings"/> to use for
    ///   deserialization. Defaults to <see cref="DefaultJsonSerializerSettings"/> if not provided.
    /// </param>
    /// <param name="rootElementKeys">
    ///   List, in order, of sub-keys path to 
    ///   follow to deserialization starting position.
    /// </param>
    /// <returns>A T-type object.</returns>
    /// <exception cref="JsonNoDataError">JSON string emply or does not exist the root element.</exception>
    /// <exception cref="JsonDeserializationError">Error convert JSON string to T-type object.</exception>
    internal static object ConvertJsonToObject(string? data,
      Type type,
      JsonSerializerSettings? jsonSerializerSettings = null,
      List<string>? rootElementKeys = null) {
      if (rootElementKeys != null && rootElementKeys.Any()) {

        data = GoToRootElement(data, rootElementKeys);
      }

      if (data == null || string.IsNullOrWhiteSpace(data)) {

        throw new Exceptions.JsonNoDataError();
      }

      try {
        object? obj =
            JsonConvert.DeserializeObject(data, type, jsonSerializerSettings ?? DefaultJsonSerializerSettings);
        return (obj ?? default)!;
      } catch {
        throw new Exceptions.JsonDeserializationError(type);
      }
    }

    /// <summary>
    ///   Deserialize a JSON string into a dynamic object.
    /// </summary>
    /// <param name="data">A string of JSON data.</param>
    /// <param name="jsonSerializerSettings">
    ///   The <see cref="Newtonsoft.Json.JsonSerializerSettings" /> to use for
    ///   deserialization. Defaults to <see cref="DefaultJsonSerializerSettings" /> if not provided.
    /// </param>
    /// <param name="rootElementKeys">
    ///   List, in order, of sub-keys path to follow to deserialization starting position.
    /// </param>
    /// <returns>An <see cref="ExpandoObject"/> object.</returns>
    internal static ExpandoObject ConvertJsonToObject(string? data,
        JsonSerializerSettings? jsonSerializerSettings = null,
        List<string>? rootElementKeys = null
        ) {
      return ConvertJsonToObject<ExpandoObject>(data, jsonSerializerSettings, rootElementKeys);
    }

    /// <summary>
    ///   Deserialize data from a RestSharp.RestResponse into 
    ///   a T-type object, using this instance's <see cref="JsonSerializerSettings" />.
    /// </summary>
    /// <typeparam name="T">Type of object to deserialize to.</typeparam>
    /// <param name="reponse"><see cref="RestSharp.RestResponse"/> object to extract data from.</param>
    /// <param name="jsonSerializerSettings">
    ///   The <see cref="Newtonsoft.Json.JsonSerializerSettings" /> to use for
    ///   deserialization. Defaults to <see cref="DefaultJsonSerializerSettings" /> if not provided.
    /// </param>
    /// <param name="rootElementKeys">
    ///   List, in order, of sub-keys path to follow to 
    ///   deserialization starting position.</param>
    /// <returns>A T-type object.</returns>
    internal static T ConvertJsonToObject<T>(RestResponse reponse,
        JsonSerializerSettings? jsonSerializerSettings = null,
        List<string>? rootElementKeys = null) where T : class {
      return ConvertJsonToObject<T>(reponse.Content, jsonSerializerSettings, rootElementKeys);
    }

    internal static string ConvertObjectToJson(object data,
                                               JsonSerializerSettings? jsonSerializerSettings = null) {
      try {
        return JsonConvert.SerializeObject(data, jsonSerializerSettings ?? DefaultJsonSerializerSettings);
      } catch {
        throw new Exceptions.JsonSerializationError(data.GetType());
      }
    }

    /// <summary>
    /// Venture through the data to find the root element of JSON string
    /// </summary>
    /// <param name="data">A string of JSON data.</param>
    /// <param name="rootElementKeys">List, in order, of sub-keys path 
    /// to follow to desrialization starting position</param>.
    /// <returns></returns>
    private static string? GoToRootElement(string? data, List<string> rootElementKeys) {

      if (data == null) {

        return null;
      }

      object? json = JsonConvert.DeserializeObject(data);


      try {
        rootElementKeys.ForEach(key => { json = (json as JObject)?.Property(key)?.Value; });
        return (json as JObject)?.ToString();
      } catch {
        return null;
      }
    }
  }
}
