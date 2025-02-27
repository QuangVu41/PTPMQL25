using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models;

[Table("Persons")]
public class Person
{
  [Key]
  public int PersonId { get; set; }
  public string FullName { get; set; }
  public string Address { get; set; }
  public double? Height { get; set; }
  public double? Weight { get; set; }
}