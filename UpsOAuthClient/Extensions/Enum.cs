using System.Reflection;
using System.Runtime.Serialization;

namespace UpsOAuthClient.Extensions {
  /// <summary>
  ///  Extensions for Enumerations.
  /// </summary>
  public static class Enum {
    /// <summary>
    ///   Enum extension to get value from <see cref="EnumMemberAttribute"/>
    /// </summary>
    /// <param name="@enum">Enum member</param>
    /// <returns>Null or string value of enum member</returns>
    public static string? GetEnumMemberValue(this System.Enum @enum) {
      var attr = @enum.GetType().GetTypeInfo().DeclaredMembers.SingleOrDefault(x => @enum.ToString() == x.Name);

      if (attr != null) {

        return attr.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
      }

      return null;
    }
  }
}
