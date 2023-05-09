using UpsOAuthClient.Exceptions;

namespace UpsOAuthClient.Exceptions.General {
  public class InvalidRequestExeption : BaseException {
    public InvalidRequestExeption(string code, string message) : base(code, message, System.Net.HttpStatusCode.BadRequest) { }
  }
}
