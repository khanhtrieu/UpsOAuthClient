using RestSharp;
using RestSharp.Authenticators;
using UpsOAuthClient.Extensions;
using CommonEnums = UpsOAuthClient.Common.Enums;


namespace UpsOAuthClient.Http {

  public class Request {

    /// <summary>
    ///   Optional root element for the JSON to begin deserialization at.
    /// </summary>
    public readonly string? RootElement;

    public Dictionary<string, object> HeaderParameters {
      get => _headerParameters ?? new Dictionary<string, object>();
      set => _headerParameters = value;
    }

    public Dictionary<string, object> Parameters {
      get => _parameters ?? new Dictionary<string, object>();
      set => _parameters = value;
    }

    /// <summary>
    ///   Optional parameters that add to the query string paramters of the request.
    /// </summary>
    public Dictionary<string, object>? _parameters;

    /// <summary>
    ///   Optional header parameters that add to the header of the request.
    /// </summary>
    private Dictionary<string, object>? _headerParameters;

    private readonly RestRequest _restRequest;

    private readonly CommonEnums.HttpBodySchemaType _contentType;

    public Request(string resource, CommonEnums.ApiVersion verion, string endpoint, Method method,
        Dictionary<string, object>? paramters, Dictionary<string, object>? headerParameters = null,
        CommonEnums.HttpBodySchemaType contentType = CommonEnums.HttpBodySchemaType.Json, string? rootElement = null) {
      string path = string.Format("{0}/{1}{2}", resource,
                                  (verion.GetEnumMemberValue() is not null ? $"{verion.GetEnumMemberValue()}/" : ""), endpoint);
      RootElement = rootElement;
      _contentType = contentType;
      _headerParameters = headerParameters;
      _parameters = paramters;
      _restRequest = new RestRequest(path, method);
    }

    public static explicit operator RestRequest(Request request) => request._restRequest;

    /// <summary>
    ///  Build request header and content.
    /// </summary>
    internal void Build() {
      BuildHeaderParamters();
      BuildParamters();
    }

    /// <summary>
    ///   Build request contents.
    /// </summary>
    private void BuildParamters() {
      if (Parameters.Count == 0) {

        return;
      }

      switch (_restRequest.Method) {
        case Method.Get:
        case Method.Delete:
          BuildQueryParamters();
          break;
        case Method.Post:
        case Method.Patch:
        case Method.Put:
          BuildBodyParameters();
          break;
        case Method.Head:
        case Method.Options:
        case Method.Merge:
        case Method.Copy:
        case Method.Search:
        default:
          break;
      }
    }

    /// <summary>
    ///   Add optional header parameters to Rest Request's header.
    /// </summary>
    private void BuildHeaderParamters() {

      if (_headerParameters == null) {

        return;
      }

      foreach (KeyValuePair<string, object> pair in this.HeaderParameters) {
        if (pair.Value == null) {

          continue;
        }

        _restRequest.AddOrUpdateParameter(pair.Key, pair.Value, ParameterType.HttpHeader);
      }
    }

    /// <summary>
    ///   Build Header content type base on body schema type <see cref="CommonEnums.HttpBodySchemaType"/>
    /// </summary>
    private void BuildHeaderContentType() {
      string applicationContentType = "application/json";

      switch (_contentType) {
        case CommonEnums.HttpBodySchemaType.UrlEncode:
          applicationContentType = "application/x-www-form-urlencoded";
          break;
        default:
          applicationContentType = "application/json";
          break;
      }

      _restRequest.AddOrUpdateParameter("Content-Type", applicationContentType, ParameterType.HttpHeader);
    }

    /// <summary>
    ///   Add paramters (body request parameters) to query string. 
    /// </summary>
    private void BuildQueryParamters() {
      foreach (KeyValuePair<string, object> pair in this.Parameters) {
        if (pair.Value == null) {

          continue;
        }
        _restRequest.AddOrUpdateParameter(pair.Key, pair.Value, ParameterType.QueryString);
      }
    }

    /// <summary>
    ///  Add parameters (body request parameters) to rest request's body.
    /// </summary>
    private void BuildBodyParameters() {
      if (_contentType == CommonEnums.HttpBodySchemaType.Json) {

        _restRequest.AddJsonBody(JsonSerialization.ConvertObjectToJson(this.Parameters));
      } else {

        foreach (var param in this.Parameters) {
          _restRequest.AddOrUpdateParameter(param.Key, param.Value, ParameterType.GetOrPost);
        }
      }
    }
  }
}
