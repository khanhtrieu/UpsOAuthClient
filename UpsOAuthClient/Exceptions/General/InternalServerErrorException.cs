using UpsOAuthClient.Exceptions;

namespace UpsOAuthClient.Exceptions.General {
  public class InternalServerErrorException : BaseException {
    public InternalServerErrorException(string code, string message) :
                                        base(code, message, System.Net.HttpStatusCode.InternalServerError) { }
  }
}
