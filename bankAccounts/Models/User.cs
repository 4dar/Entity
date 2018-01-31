using System;
using System.Collections.Generic;
namespace bankAccounts.Models
{
    public abstract class BaseEntity {}
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Transaction> Transactions {get;set;}

        public User()
        {
            Transactions = new List<Transaction>();
        }
    }
}