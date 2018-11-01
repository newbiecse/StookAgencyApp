using System;
using System.ComponentModel.DataAnnotations;

namespace StookAgencyApp.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset DateJoined { get; set; }
    }
}