using UpsOAuthClient.Common;

namespace UpsOAuthClient.Http {
  /// <summary>
  ///   Default configuration of Ups API
  /// </summary>
  public interface IClientConfiguration {
    string ApiBaseUrl { get; }

    /// <summary>
    ///   Client ID (ID of application provided by UPS)
    /// </summary>
    string ClientId { get; }

    /// <summary>
    ///   Client Secret (Provided by UPS).
    /// </summary>
    string ClientSecret { get; }

    /// <summary>
    ///   Redirect Url which set up in UPS Api Account
    /// </summary>
    string? RedirectUrl { get; set; }

    /// <summary>
    ///   Default connection time out for rest request.
    /// </summary>
    int ConnectTimeoutMillisecond { get; set; }

    /// <summary>
    ///   The environment of the API will be connected.
    /// </summary>
    Enums.ApiEnvironment Environment { get; set; }
  }

  ///<inheritdoc/>
  public class ClientConfiguration : IClientConfiguration {

    /// <summary>
    ///   The connection timeout in milliseconds.
    /// </summary>
    private const int _defaultConnectTimeoutMilliseconds = 30000;

    /// <summary>
    ///   The API base URI production environment.
    /// </summary>
    private const string _defaultProductionUrl = "https://onlinetools.ups.com/";

    /// <summary>
    ///   The API base URI test environment.
    /// </summary>
    private const string _defaultTestUrl = "https://wwwcie.ups.com/";

    private int _connectTimeoutMilliseconds = 30000;

    ///<inheritdoc/>
    public string ClientId { get => _clientId; }

    ///<inheritdoc/>
    public string ClientSecret { get => _clientSecret; }

    /// <summary>
    ///   Redirect URL (URL user will be redirected to after authentication using third-party service).
    /// </summary>
    public string? RedirectUrl { get; set; }
    public int ConnectTimeoutMillisecond {
      get => _connectTimeoutMilliseconds;
      set => _connectTimeoutMilliseconds = value;
    }

    /// <summary>
    ///   Base Api Url depends on the environment set up.
    /// </summary>
    public string ApiBaseUrl => Environment == Enums.ApiEnvironment.Production ? _defaultProductionUrl : _defaultTestUrl;

    public Enums.ApiEnvironment Environment { get; set; }

    private string _clientId;

    private string _clientSecret;

    public ClientConfiguration(string clientId, string clientSecret, Enums.ApiEnvironment env = Enums.ApiEnvironment.Production, 
        string redirectUrl = "") {
      _clientId = clientId;
      _clientSecret = clientSecret;
      Environment = env;
      this.RedirectUrl= redirectUrl;
    }
  }
}
