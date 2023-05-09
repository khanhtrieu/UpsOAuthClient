using System.Data;
using System.Globalization;

namespace UpsOAuthClient.Exceptions {
  public class JsonError : DataException {
    /// <summary>
    ///   Initializes a new instance of the <see cref="JsonError"/> class.
    /// </summary>
    /// <param name="message"></param>
    public JsonError(string message) : base(message) { }
  }

  public class JsonDeserializationError : JsonError {
    /// <summary>
    ///   Initializes a new instance of the <see cref="JsonDeserializationError"/> class.
    /// </summary>
    /// <param name="toType"></param>
    public JsonDeserializationError(Type toType) : base(
        string.Format(CultureInfo.InvariantCulture,
                      Common.Constants.ErrorMessages.JsonDeserializationError, toType.FullName)) {

    }
  }

  public class JsonSerializationError : JsonError {
    /// <summary>
    ///   Initializes a new instance of the <see cref="JsonSerializationError"/> class.
    /// </summary>
    /// <param name="toType"></param>
    public JsonSerializationError(Type toType) : base(
        string.Format(CultureInfo.InvariantCulture,
                      Common.Constants.ErrorMessages.JsonSerializationError, toType.FullName)) {

    }
  }

  public class JsonNoDataError : JsonError {
    /// <summary>
    ///   Initializes a new instance of the <see cref="JsonNoDataError"/> class.
    /// </summary>
    public JsonNoDataError() : base(
        string.Format(CultureInfo.InvariantCulture,
                      Common.Constants.ErrorMessages.JsonSerializationError)) {

    }
  }
}
