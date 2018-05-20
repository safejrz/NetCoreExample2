using System.Runtime.Serialization;

namespace FlightsChecking.CommonLibrary.Models
{
  [DataContract]
  public class AppSettings
  {
    [DataMember]
    public string FlightsCheckingWebApiUrl { get; set; }
  }
}
