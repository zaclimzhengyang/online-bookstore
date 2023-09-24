using System.ComponentModel.DataAnnotations;

namespace OnlineBookShop.Models;

public class Company
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int PostalCode { get; set; }
    public int PhoneNumber { get; set; }
    
}