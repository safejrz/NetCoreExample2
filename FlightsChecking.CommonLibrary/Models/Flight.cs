using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FlightsChecking.CommonLibrary.Models
{
  [DataContract]
  public class Flight
  {
    [DataMember]
    [Required]
    public int Id { get; set; }

    [DataMember]
    [MaxLength(100)]
    public string Description { get; set; }

    [DataMember]
    [Required]
    [MaxLength(50)]
    public string Company { get; set; }

    [DataMember]
    [Required]
    [Range(1, 20000)]
    public decimal Price { get; set; }

    [DataMember]
    [Required]
    public DateTime Departure { get; set; }
  }
}
