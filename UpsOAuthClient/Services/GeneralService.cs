using Newtonsoft.Json;
using RestSharp;
using UpsOAuthClient.Exceptions;
using UpsOAuthClient.Extensions;
using UpsOAuthClient.Http;
using RestSharp.Serializers.NewtonsoftJson;
using RestSharp.Authenticators.OAuth2;
using UpsOAuthClient.Exceptions.Api;

namespace UpsOAuthClient.Services {
  /// <summary>
  ///   Base class for Service requests.
  /// </summary>
  public abstract class GeneralService {
    /// <summary>
    ///   Resource of the service
    ///   <example>security</example>
    /// </summary>
    /// <returns></returns>
    protected abstract string GetResource();

    /// <summary>
    ///   Version of services <see cref="Common.Enums.ApiVersion"/>.
    /// </summary>
    /// <returns></returns>
    protected abstract Common.Enums.ApiVersion GetApiVersion();

    /// <summary>
    ///   Default configration of the service <see cref="IClientConfiguration"/>.
    /// </summary>
    private IClientConfiguration _clientConfiguration;

    public GeneralService(IClientConfiguration clientConfiguration) {
      _clientConfiguration = clientConfiguration;
    }

    /// <summary>
    ///     Gets the default <see cref="Newtonsoft.Json.JsonSerializerSettings" /> to use for de/serialization.
    /// </summary>
    private static JsonSerializerSettings DefaultJsonSerializerSettings => new() {
      NullValueHandling = NullValueHandling.Ignore,
      MissingMemberHandling = MissingMemberHandling.Ignore,
      DateFormatHandling = DateFormatHandling.IsoDateFormat,
      DateTimeZoneHandling = DateTimeZoneHandling.Utc,
      FloatFormatHandling = FloatFormatHandling.String,
    };

    /// <summary>
    ///   Execute a HTTP request
    /// </summary>
    /// <typeparam name="T">Type of object to serizlize response.</typeparam>
    /// <param name="request">Request content to execute.</param>
    /// <param name="allowRestThrowError">Optional allow RestSharp throw an exceptions</param>
    /// <returns>A RestResponse containing a T-Type object.</returns>
    internal virtual async Task<RestResponse<T>> ExecuteRequest<T>(RestRequest request, JsonSerializerSettings? jsonSerializerSettings = null, bool allowRestThrowError = false) {
      RestClientOptions clientOptions = new RestClientOptions() {
        BaseUrl = new Uri(_clientConfiguration.ApiBaseUrl),
        MaxTimeout = _clientConfiguration.ConnectTimeoutMillisecond,
        ThrowOnAnyError = allowRestThrowError,
      };

      jsonSerializerSettings = jsonSerializerSettings ?? DefaultJsonSerializerSettings;

      RestClient restClient = new RestClient(clientOptions, configureSerialization: s =>
          s.UseNewtonsoftJson(jsonSerializerSettings));

      return await restClient.ExecuteAsync<T>(request);
    }

    /// <summary>
    ///   Perform restful request with Get method.
    /// </summary>
    /// <typeparam name="T">T type object return when retrieve from the API.</typeparam>
    /// <param name="endpoint">The endpoint of the service resource (Append after the version of the service).</param>
    /// <param name="parameters">Optional parameters that add to the query string paramters of the request.</param>
    /// <param name="headerParameters">Optional header parameters that add to the header of the request.</param>
    /// <param name="rootElement">Optional root element for the JSON to begin deserialization at.</param>
    /// <param name="jsonSerializerSettings">Optional override the default JsonSerialierSetting <see cref="JsonSerializerSettings"/>.</param>
    /// <param name="allowRestThrowError">Optional allow RestSharp throw an exceptions.</param>
    /// <returns>T-type object after deserialize from response</returns>
    protected internal async Task<T> Get<T>(string endpoint, Dictionary<string, object>? parameters = null,
                                            Dictionary<string, object>? headerParameters = null, string? rootElement = null,
                                            JsonSerializerSettings? jsonSerializerSettings = null,
                                            bool allowRestThrowError = false) where T : class {
      return await Request<T>(Method.Get, endpoint, parameters,
                              headerParameters: headerParameters, rootElement: rootElement,
                              jsonSerializerSettings: jsonSerializerSettings, allowRestThrowError: allowRestThrowError);
    }

    /// <summary>
    ///   Perform restful request with Post method
    /// </summary>
    /// <typeparam name="T">T type object return when retrieve from the API.</typeparam>
    /// <param name="endpoint">The endpoint of the service resource (Append after the version of the service).</param>
    /// <param name="parameters">Optional parameters that add to the body of the request.</param>
    /// <param name="headerParameters">Optional header parameters that add to the header of the request.</param>
    /// <param name="contentType">
    ///   Define content type for the RestSharp send a request.
    ///   <example>
    ///     Common.Enums.HttpBodySchemaType.Json: 'Content-Type: application/json'
    ///   </example>
    /// </param>
    /// <param name="rootElement">Optional root element for the JSON to begin deserialization at.</param>
    /// <param name="jsonSerializerSettings">Optional override the default JsonSerialierSetting <see cref="JsonSerializerSettings"/>.</param>
    /// <param name="allowRestThrowError">Optional allow RestSharp throw an exceptions.</param>
    /// <returns>T-type object after deserialize from response</returns>
    protected internal async Task<T> Post<T>(string endpoint, Dictionary<string, object>? parameters = null,
        Dictionary<string, object>? headerParameters = null,
        Common.Enums.HttpBodySchemaType contentType = Common.Enums.HttpBodySchemaType.Json,
        string? rootElement = null, JsonSerializerSettings? jsonSerializerSettings = null,
        bool allowRestThrowError = false) where T : class {
      return await Request<T>(Method.Post, endpoint, parameters, headerParameters,
                              contentType, rootElement, jsonSerializerSettings, allowRestThrowError);
    }

    /// <summary>
    ///   Make an HTTP request to the UPS API and deserialize the reponse JSON into an object
    /// </summary>
    /// <typeparam name="T">Type of object to serizlize response.</typeparam>
    /// <param name="method">HTTP method to send a request</param>
    /// <param name="endpoint">The option or the endpoint of the service (Not include base url and version).</param>
    /// <param name="parameters">Optional parameters that add to the body or query string paramters of the request.</param>
    /// <param name="headerParameters">Optional header parameters that add to the header of the request.</param>
    /// <param name="contentType">
    ///   Define content type for the RestSharp send a request.
    ///   <example>
    ///     Common.Enums.HttpBodySchemaType.Json: 'Content-Type: application/json'
    ///   </example>
    /// </param>
    /// <param name="rootElement">Optional root element for the JSON to begin deserialization at.</param>
    /// <param name="jsonSerializerSettings">Optional override the default JsonSerialierSetting <see cref="JsonSerializerSettings"/>.</param>
    /// <param name="allowRestThrowError">Optional allow RestSharp throw an exceptions.</param>
    /// <returns>An instance of a T-type of object.</returns>
    protected internal async Task<T> Request<T>(Method method, string endpoint,
        Dictionary<string, object>? parameters = null, Dictionary<string, object>? headerParameters = null,
        Common.Enums.HttpBodySchemaType contentType = Common.Enums.HttpBodySchemaType.Json, string? rootElement = null,
        JsonSerializerSettings? jsonSerializerSettings = null, bool allowRestThrowError = false) where T : class {
      Request requestContent = new(GetResource(), GetApiVersion(),
                                   endpoint, method, parameters, headerParameters, contentType, rootElement);
      return await Request<T>(requestContent, rootElement, jsonSerializerSettings, allowRestThrowError);
    }

    /// <summary>
    ///   Make an HTTP request to the UPS API and deserialize the reponse JSON into an object
    /// </summary>
    /// <typeparam name="T">Type of object to serizlize response.</typeparam>
    /// <param name="requestContent"></param>
    /// <param name="rootElement">Optional root element for the JSON to begin deserialization at.</param>
    /// <param name="jsonSerializerSettings">Optional override the default JsonSerialierSetting <see cref="JsonSerializerSettings"/>.</param>
    /// <param name="allowRestThrowError">Optional allow RestSharp throw an exceptions.</param>
    /// <returns>An instance of a T-type of object.</returns>
    /// <exception cref="JsonDeserializationError">An error of being unable to serialize JSON content.</exception>
    protected internal async Task<T> Request<T>(Request requestContent, string? rootElement = null,
                                                JsonSerializerSettings? jsonSerializerSettings = null,
                                                bool allowRestThrowError = false) where T : class {
      //Build the rest request from Request object.
      RestRequest request = PrepareRequest(requestContent);

      //Execute the request
      RestResponse<T> response = await ExecuteRequest<T>(request, jsonSerializerSettings, allowRestThrowError);

      //Throw an error if the response status code different 200
      if (!response.IsSuccessful) {

        throw ApiErrorException.FromErrorResponse(response);
      }

      // Get the order of the root elements to use during deserialization
      List<string>? rootElements = null;

      if (request.RootElement != null) {

        rootElements = new List<string>() { request.RootElement };
      }

      T responseBody = JsonSerialization.ConvertJsonToObject<T>(response, null, rootElements);

      if (responseBody is null) {

        throw new JsonDeserializationError(typeof(T));
      }

      return responseBody;
    }

    /// <summary>
    ///   Build request header paramters, query string paramters
    /// </summary>
    /// <param name="request">The <see cref="UpsOAuthClient.Http.Request"/>Object instance to prepare.</param>
    /// <returns>The instance of <see cref="RestRequest"/> to execute.</returns>
    private RestRequest PrepareRequest(Request request) {
      request.Build();
      RestRequest restRequest = (RestRequest)request;

      return restRequest;
    }
  }
}
