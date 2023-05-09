using UpsOAuthClient.Exceptions;

namespace UpsOAuthClient.Exceptions.General {
  /// <summary>
  ///  Unknow exception when the exception code have not defined in 
  ///  the ThrowExceptionFromReponse method <see cref="Exceptions.Api.ApiErrorException"/>.
  /// </summary>
  public class UnknowException : BaseException {
    public UnknowException(string code = "", string message = "Server Internal Error") : base(code, message, System.Net.HttpStatusCode.InternalServerError) { }
  }
}
