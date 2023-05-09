using UpsOAuthClient.Exceptions;

namespace UpsOAuthClient.Exceptions.General {
  public class UnAuthorizedExeption : BaseException {
    public UnAuthorizedExeption(string code, string message) :
      base(code, message, System.Net.HttpStatusCode.Unauthorized) { }
  }
}
