using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BankAccounts.Models
{
  public class LoginUser
  {
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public LoginUser() { }
  }

}