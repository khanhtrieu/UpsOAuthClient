using UpsOAuthClient.Exceptions;

namespace UpsOAuthClient.Exceptions.General {
  public class LimitReqestException : BaseException {
    public LimitReqestException(string code, string message, int? statusCode = null) :
                                base(code, message, System.Net.HttpStatusCode.TooManyRequests) { }
  }
}
