using Newtonsoft.Json;

namespace UpsOAuthClient.Dto {
  /// <summary>
  ///   Validate Client object return from UPS' API
  /// </summary>
  public class ValidateClient {
    [JsonProperty("result")]
    public string Result { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    public string LassoRedirectURL { get; set; }
  }
}
