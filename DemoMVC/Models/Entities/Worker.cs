using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.Entities
{
  public class Worker
  {
    [Key]
    public int WorkerId { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string Address { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Position { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }
  }
}