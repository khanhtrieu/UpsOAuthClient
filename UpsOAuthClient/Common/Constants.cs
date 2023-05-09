namespace UpsOAuthClient.Common {
  /// <summary>
  ///   Defaults error message for JsoneError <see cref="Exceptions.JsonError"/>.
  /// </summary>
  public static class Constants {
    public static class ErrorMessages {
      public const string InvalidClientInfo = "Invalid client key or client secret.";
      public const string InvalidRefreshToken = "Invalid refresh token.";
      public const string JsonDeserializationError = "Error deserializing JSON into object of type {0}.";
      public const string JsonNoDataToDeserialize = "No data to deserialize.";
      public const string JsonSerializationError = "Error serializing {0} object into JSON.";
      public const string UpsCodeRequired = "Ups Code in required.";

    }
  }
}
