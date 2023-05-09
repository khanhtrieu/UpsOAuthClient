using Microsoft.Extensions.Configuration;
using UpsOAuthClient.Exceptions.General;

namespace UpsOAuthClient.Tests {
  [TestFixture]
  public class OAuthClientTest {
    private IOAuth2Client _client;
    private string _clientId;
    private string _clientSecret;
    private string _redirectUrl;

    [OneTimeSetUp]
    public void init() {
      string path = TestContext.CurrentContext.TestDirectory;
      var config = new ConfigurationBuilder().SetBasePath(TestContext.CurrentContext.TestDirectory)
        .AddJsonFile("appsettings.json", optional: false).Build();
      _clientId = config["ClientID"].ToString();
      _clientSecret = config["ClientSecret"].ToString();
      _redirectUrl = config["RedirectUrl"].ToString();
      UpsOAuthClient.Http.ClientConfiguration configuration = new UpsOAuthClient.Http.ClientConfiguration(_clientId, _clientSecret,
        Common.Enums.ApiEnvironment.Test);
      _client = new UpsOAuthClient.OAuthClient(configuration);
    }

    #region CreateToken
    [Test]
    [TestCase("", "", "")]
    [TestCase(null, "", "")]
    [TestCase(null, null, "")]
    [TestCase(null, null, null)]
    [TestCase("", "", null)]
    [TestCase("", null, null)]
    [TestCase("", "Test Client Secret", "Test Uri")]
    [TestCase(null, "Test Client Secret", "Test Uri")]
    [TestCase("Test Client Id", "", "Test Uri")]
    [TestCase("Test Client Id", null, "Test Uri")]
    [TestCase("Test Client Id", "", "")]
    [TestCase("Test Client Id", null, "")]
    [TestCase("Test Client Id", null, null)]
    public void CreateToken_ThrowFieldRequiredException_WhenMissingRedirectUrlOrClientIdOrClientSecret(string clientId, string clientSecret, string redirectUri) {
      UpsOAuthClient.Http.ClientConfiguration configuration = new UpsOAuthClient.Http.ClientConfiguration(clientId, clientSecret, Common.Enums.ApiEnvironment.Test);
      configuration.RedirectUrl = redirectUri;
      var client = new UpsOAuthClient.OAuthClient(configuration);
      Assert.ThrowsAsync<ArgumentNullException>(async () => await client.CreateToken());
    }

    [Test]
    public void CreateToken_RetrieveToken_WhenValidDataRequest() {
      Thread.Sleep(3000);
      UpsOAuthClient.Http.ClientConfiguration configuration = new UpsOAuthClient.Http.ClientConfiguration(
        _clientId, _clientSecret,
        Common.Enums.ApiEnvironment.Test);
      var client = new UpsOAuthClient.OAuthClient(configuration);
      var token = Task.Run(async () => await client.CreateToken()).GetAwaiter().GetResult();

      Assert.IsInstanceOf<UpsOAuthClient.Dto.Token>(token);
      Assert.That(token.ClientId, Is.EqualTo(_clientId));
      Assert.That(token.TokenType, Is.EqualTo("Bearer"));
      Assert.That(token.Status, Is.EqualTo("approved"));
    }
    #endregion

    #region Generate Token
    [Test]
    [TestCase("", "", "")]
    [TestCase(null, "", "")]
    [TestCase(null, null, "")]
    [TestCase(null, null, null)]
    [TestCase("", "", null)]
    [TestCase("", null, null)]
    [TestCase("", "Test Client Secret", "Test Uri")]
    [TestCase(null, "Test Client Secret", "Test Uri")]
    [TestCase("Test Client Id", "", "Test Uri")]
    [TestCase("Test Client Id", null, "Test Uri")]
    [TestCase("Test Client Id", "", "")]
    [TestCase("Test Client Id", null, "")]
    [TestCase("Test Client Id", null, null)]
    public void GenerateToken_ThrowFieldRequiredException_WhenMissingClientIdOrClientSecret(string clientId, string clientSecret, string redirectUri) {
      UpsOAuthClient.Http.ClientConfiguration configuration = new UpsOAuthClient.Http.ClientConfiguration(clientId, clientSecret, Common.Enums.ApiEnvironment.Test);
      configuration.RedirectUrl = redirectUri;
      var client = new UpsOAuthClient.OAuthClient(configuration);
      Assert.ThrowsAsync<ArgumentNullException>(async () => await client.GenerateToken(""));
    }
    #endregion

    #region RefreshToken
    [Test]
    [TestCase("")]
    public void RefreshToken_ThrowFieldRequiredException_WhenRefreshTokenEmpty(string refreshToken) {
      Assert.ThrowsAsync<ArgumentNullException>(async () => await _client.RefreshToken(refreshToken));
    }
    #endregion

    #region Validate Client
    [Test]
    public void ValidateClient_RetrieveValidateClientObject_WhenValidDataRequest() {
      Thread.Sleep(2000);
      UpsOAuthClient.Http.ClientConfiguration configuration = new(_clientId, _clientSecret, Common.Enums.ApiEnvironment.Test, _redirectUrl);
      var client = new UpsOAuthClient.OAuthClient(configuration);
      var validateClient = Task.Run(async () => await client.ValidateClient()).GetAwaiter().GetResult();
      Assert.That(validateClient.Result, Is.EqualTo("success"));
      Assert.That(validateClient.Type, Is.EqualTo("ups_com_api"));
      Assert.That(validateClient.LassoRedirectURL, Is.EqualTo("https://www.ups.com/lasso/signin"));
    }
    #endregion
  }
}
