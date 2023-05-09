using System.Runtime.Serialization;

namespace UpsOAuthClient.Common {
  public class Enums {
    public enum ApiVersion {
      Blank,
      [EnumMember(Value = "v1")]
      V1,
    }

    /// <summary>
    ///   Enums API environment mode.
    /// </summary>
    public enum ApiEnvironment { Production, Test };

    public enum HttpBodySchemaType {
      Json,
      UrlEncode
    }
  }
}
