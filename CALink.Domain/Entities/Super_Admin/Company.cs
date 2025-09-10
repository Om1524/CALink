using CALink.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Entities.Super_Admin
{
    [Table("Companies")]
    public class Company : BaseEntity
    {
        [Required]
        [Column("name", TypeName = "varchar(200)")]
        public string Name { get; set; } = string.Empty;

        [Column("tenant", TypeName = "varchar(200)")]
        public string? Tenant { get; set; } = string.Empty;

        [Column("code", TypeName = "varchar(50)")]
        public string? Code { get; set; } = string.Empty;

        [Column("email", TypeName = "varchar(200)")]
        public string? Email { get; set; }

        [Column("phone", TypeName = "varchar(20)")]
        public string? Phone { get; set; }

        [Column("address", TypeName = "text")]
        public string? Address { get; set; }

        [Column("city", TypeName = "varchar(100)")]
        public string? City { get; set; }

        [Column("state", TypeName = "varchar(100)")]
        public string? State { get; set; }

        [Column("country", TypeName = "varchar(100)")]
        public string? Country { get; set; }

        [Column("zipcode", TypeName = "varchar(20)")]
        public string? ZipCode { get; set; }

        [Column("IsActive", TypeName = "boolean")]
        public bool IsActive { get; set; } = true;
    }
}
