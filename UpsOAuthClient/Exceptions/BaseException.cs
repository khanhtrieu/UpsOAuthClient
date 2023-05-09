using System.Runtime.Serialization;

namespace UpsOAuthClient.Exceptions {

  public interface IBaseException : ISerializable {
    string? Code { get; set; }
    int? StatusCode { get; set; }
  }

  /// <summary>
  ///  Base Exception UPS Error response.
  /// </summary>
  public class BaseException : Exception, IBaseException {
    public string? Code { get; set; }
    public int? StatusCode { get; set; }

    public BaseException(string code, string message, int? statusCode = null) : base(message) {
      Code = code;
      StatusCode = statusCode;
    }
    public BaseException(string code, string message,
                         System.Net.HttpStatusCode? httpStatusCode) : this(code, message, (int?)httpStatusCode) {
    }

    /// <summary>
    ///   Pretty print error response to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      string errorString = $@"{Code} ({StatusCode}): {Message}";
      return errorString;
    }
  }
}
