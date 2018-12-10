using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace BankAccounts.Models
{
  public class Transaction
  {
    [Key]
    public int TransactionId {get;set;}
    [Required]
    public Decimal Amount {get;set;}
    public DateTime CreatedAt {get;set;}
    public int UserId {get;set;}
    public User Transactor {get;set;}
    public Transaction(){}

  

  }
}