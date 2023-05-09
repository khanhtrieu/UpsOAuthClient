using UpsOAuthClient.Exceptions;

namespace UpsOAuthClient.Exceptions.General {
  public class BlockedMerchantExpcetion : BaseException {
    public BlockedMerchantExpcetion(string code, string message) : base(code, message, System.Net.HttpStatusCode.Forbidden) { }
  }
}
