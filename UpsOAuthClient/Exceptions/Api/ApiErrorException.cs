using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using UpsOAuthClient.Exceptions.General;

namespace UpsOAuthClient.Exceptions.Api {
  public class ApiErrorException : BaseException {
    // All constructors for API exceptions are protected, so you cannot directly initialize an instance of the exception class.
    // Instead, you must use the .FromResponse method to retrieve an instance.
    protected ApiErrorException(string code, string message, int? statusCode = null) : base(code, message) {
      this.Code = code;
    }

    internal static ApiErrorException FromErrorResponse(RestResponse response) {
      if (response != null) {

        HttpStatusCode statusCode = response.StatusCode;
        int statusCodeNumber = (int)statusCode;
        object? bodyReponse = JsonConvert.DeserializeObject(response.Content);

        if (bodyReponse != null && (bodyReponse as JObject).ContainsKey("response")) {

          JObject obj = (bodyReponse as JObject);
          if (obj.ContainsKey("response") && obj["response"]?["errors"] != null) {

            ThrowExceptionFromReponse(obj["response"]?["errors"][0]["code"].ToString(),
                                      obj["response"]?["errors"][0]["message"].ToString(),
                                      statusCodeNumber);
          }
        }
      }

      return null;
    }

    private static IBaseException ThrowExceptionFromReponse(string errorCode, string message, int statusCode) {
      throw statusCode switch {
        400 => throw new General.InvalidRequestExeption(errorCode, message),
        401 => throw new General.UnAuthorizedExeption(errorCode, message),
        403 => throw new General.BlockedMerchantExpcetion(errorCode, message),
        429 => throw new General.LimitReqestException(errorCode, message),
        500 => throw new General.InternalServerErrorException(errorCode, message),
        _ => throw new UnknowException(),
      };
    }
  }
}
