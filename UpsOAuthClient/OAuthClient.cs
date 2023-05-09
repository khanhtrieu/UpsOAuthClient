using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using UpsOAuthClient.Common;
using UpsOAuthClient.Dto;
using UpsOAuthClient.Exceptions.General;
using UpsOAuthClient.Http;
using UpsOAuthClient.Services;

namespace UpsOAuthClient {
  public interface IOAuth2Client {
    /// <summary>
    ///   Retrieve OAuth Bearer token on behalf of personal resources, not on behalf of another user.
    /// </summary>
    /// <returns></returns>
    Task<Token> CreateToken();

    /// <summary>
    ///   Generate Token of Retrieve OAuth Bearer token on behalf of another user.
    /// </summary>
    /// <returns></returns>
    Task<Token> GenerateToken(string code);

    /// <summary>
    ///   Refresh Token of Retrieve OAuth Bearer token on behalf of another user.
    /// </summary>
    /// <returns></returns>
    Task<Token> RefreshToken(string refreshToken);

    /// <summary>
    ///   Refresh Token of Retrieve OAuth Bearer token on behalf of another user.
    /// </summary>
    /// <returns></returns>
    Task<ValidateClient> ValidateClient();
  }

  /// <summary>
  ///   OAuth2 client service used to create authorization token for UPS' customer application to utilize UPS APIs.
  /// </summary>
  public class OAuthClient : GeneralService, IOAuth2Client {
    private IClientConfiguration _clientConfiguration;

    public OAuthClient(string clientId, string clientSecret,
        Common.Enums.ApiEnvironment env = Enums.ApiEnvironment.Production,
        string redirectUrl = "") : this(new ClientConfiguration(clientId, clientSecret, env)) { }
    public OAuthClient(IClientConfiguration clientConfiguration) : base(clientConfiguration) {
      this._clientConfiguration = clientConfiguration;
    }

    ///<inheritdoc/>
    public virtual async Task<Token> CreateToken() {
      Dictionary<string, object> request = new();
      request.Add("grant_type", "client_credentials");
      return await this.Post<Token>("oauth/token", request, getHeaderParamters(), Enums.HttpBodySchemaType.UrlEncode);
    }

    ///<inheritdoc/>
    public virtual async Task<Token> GenerateToken(string code) {
      if (string.IsNullOrWhiteSpace(code)) {

        throw new ArgumentNullException(Constants.ErrorMessages.UpsCodeRequired);
      }

      Dictionary<string, object> request = new();
      request.Add("grant_type", "authorization_code");
      request.Add("code", code);
      request.Add("redirect_uri", _clientConfiguration.RedirectUrl);

      return await this.Post<Token>("oauth/token", request, getHeaderParamters(), Enums.HttpBodySchemaType.UrlEncode);
    }

    ///<inheritdoc/>
    public virtual async Task<Token> RefreshToken(string refreshToken) {
      if (string.IsNullOrWhiteSpace(refreshToken)) {

        throw new ArgumentNullException(Constants.ErrorMessages.InvalidRefreshToken);
      }

      Dictionary<string, object> request = new();
      request.Add("grant_type", "refresh_token");
      request.Add("refresh_token", refreshToken);

      return await this.Post<Token>("oauth/refresh", request, contentType: Enums.HttpBodySchemaType.UrlEncode);
    }

    ///<inheritdoc/>
    public virtual async Task<ValidateClient> ValidateClient() {
      Dictionary<string, object> request = new();
      request.Add("client_id", _clientConfiguration.ClientId);
      request.Add("redirect_uri", _clientConfiguration.RedirectUrl);

      return await this.Get<ValidateClient>("oauth/validate-client", request);
    }

    /// <inheritdoc/>
    protected override string GetResource() {
      return "security";
    }

    /// <inheritdoc/>
    protected override Enums.ApiVersion GetApiVersion() {
      return Enums.ApiVersion.V1;
    }


    /// <summary>
    ///  Get header's merchant id parameter from client configuration.
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, object>? getHeaderParamters() {
      if (string.IsNullOrWhiteSpace(_clientConfiguration.ClientId) || string.IsNullOrWhiteSpace(_clientConfiguration.ClientSecret)) {

        throw new ArgumentNullException(Constants.ErrorMessages.InvalidClientInfo);
      }

      Dictionary<string, object>? headerParameters = new Dictionary<string, object>();
      headerParameters.Add("x-merchant-id", _clientConfiguration.ClientId);
      var authorizationBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}",
          _clientConfiguration.ClientId, _clientConfiguration.ClientSecret));
      headerParameters.Add("Authorization", string.Format("Basic {0}", Convert.ToBase64String(authorizationBytes)));
      headerParameters.Add("Content-Type", "application/x-www-form-urlencoded");

      return headerParameters;
    }

  }
}
