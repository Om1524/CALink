using CALink.Domain.Entities.Common;
using CALink.Domain.Entities.Super_Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CALink.Domain.Entities.Customer
{
    [Table("Customer")]
    public class Customer : BaseEntity
    {
        [Column("CompanyId", TypeName = "uuid")]
        [Required]
        public Guid CompanyId { get; set; }

        [JsonIgnore]
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }


        [Column("FirstName", TypeName = "varchar(200)")]
        [MaxLength(200)]
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Column("LastName", TypeName = "varchar(200)")]
        [MaxLength(200)]
        [Required]
        public string LastName { get; set; } = string.Empty;

        [Column("CompanyName", TypeName="varchar(500)")]
        [Required]
        public string? CompanyName { get; set; } 

        [Column("MobileNumber", TypeName = "varchar(30)")]
        public string MobileNumber { get; set; } = string.Empty;

        [Column("Email", TypeName = "varchar(200)")]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Column("BirthDate", TypeName = "timestamp with time zone")]
        [Required]
        public DateTime? BirthDate { get; set; }

        [Column("Gender", TypeName = "varchar(20)")]
        [Required]
        public string Gender { get; set; } = string.Empty;

        [Column("CustomerCategory", TypeName = "varchar(10)")]
        [Required]
        public string CustomerCategory { get; set; } = string.Empty;

        [Column("GSTNumber", TypeName = "varchar(50)")]
        [Required]
        public string GSTNumber { get; set; } = string.Empty;

        [Column("PancardNumber", TypeName = "varchar(50)")]
        [Required]
        public string PancardNumber { get; set; } = string.Empty;

        [Column("AadharNumber", TypeName = "varchar(50)")]
        [Required]
        public string AadharNumber { get; set; } = string.Empty;

        [Column("StartDate", TypeName = "timestamp with time zone")]
        [Required]
        public DateTime StartDate { get; set; }

        [Column("Address", TypeName = "varchar(200)")]
        [Required]
        public string Address { get; set; } = string.Empty;

        [Column("PostalCode", TypeName = "varchar(20)")]
        [Required]
        public string PostalCode { get; set; } = string.Empty;

        [Column("Country", TypeName = "varchar(50)")]
        [Required]
        public string Country { get; set; } = string.Empty;

        [Column("State", TypeName = "varchar(100)")]
        [Required]
        public string State { get; set; } = string.Empty;

        [Column("City", TypeName = "varchar(100)")]
        [Required]
        public string City { get; set; } = string.Empty;
    }
}
