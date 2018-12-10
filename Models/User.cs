using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace BankAccounts.Models
{
  public class User
  {
    [Key]
    public int UserId { get; set; }
    [Required]
    [MinLength(3)]
    public string FirstName { get; set; }
    [Required]
    [MinLength(3)]
    public string LastName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    public string Password { get; set; }

    [Compare("Password")]
    [DataType(DataType.Password)]
    [NotMapped]
    public string Confirm { get; set; }
    public decimal Balance {get;set;} = 0;
    public List<Transaction> Transactions {get;set;}

     public User() { }
  }

}