using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UpsOAuthClient.Dto {
  /// <summary>
  ///   Token object return from UPS Api when generate, create, or refresh.
  /// </summary>
  public class Token {
    /// <summary>
    ///   Expiration time for requested refresh token in seconds.
    /// </summary>
    [JsonProperty("refresh_token_expires_in")]
    public string? RefreshTokenExpiresIn { get; set; }
    /// <summary>
    ///   Status for requested refresh token.
    /// </summary>
    [JsonProperty("refresh_token_status")]
    public string? RefreshTokenStatus { get; set; }
    /// <summary>
    ///   Type of requested access token.
    /// </summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
    /// <summary>
    ///   Issue time of requested token.
    /// </summary>
    [JsonProperty("issued_at")]
    public string IssuedAt { get; set; }
    /// <summary>
    ///   Client id for requested token.
    /// </summary>
    [JsonProperty("client_id")]
    public string ClientId { get; set; }
    /// <summary>
    ///   Token to be used in API requests.
    /// </summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    /// <summary>
    ///   Refresh token to be used in refresh requests when obtaining new access token.
    /// </summary>
    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }
    /// <summary>
    ///   Scope for requested token.
    /// </summary>
    [JsonProperty("scope")]
    public string Scope { get; set; }
    /// <summary>
    ///   Time that refresh token was issued.
    /// </summary>
    [JsonProperty("refresh_token_issued_at")]
    public string? RefreshTokenIssuedAt { get; set; }
    /// <summary>
    ///   Expire time for requested token in seconds.
    /// </summary>
    [JsonProperty("expires_in")]
    public string ExpiresIn { get; set; }
    /// <summary>
    ///   Number of refreshes for requested token.
    /// </summary>
    [JsonProperty("refresh_count")]
    public string RefreshCount { get; set; }
    /// <summary>
    ///   Status for requested token.
    /// </summary>
    [JsonProperty("status")]
    public string Status { get; set; }
  }
}
