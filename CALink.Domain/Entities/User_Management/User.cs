using CALink.Domain.Entities.Common;
using CALink.Domain.Entities.Super_Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Domain.Entities.User_Management
{
    public class User : BaseEntity
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

        [Column("MobileNumber", TypeName = "varchar(30)")]
        [MaxLength(30)]
        public string MobileNumber { get; set; } = string.Empty;

        [Column("Email", TypeName = "varchar(200)")]
        [MaxLength(200)]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("Password", TypeName = "varchar(300)")]
        public string Password { get; set; } = string.Empty;

        [Column("RoleId", TypeName = "uuid")]
        public Guid? RoleId { get; set; }

        // Navigation property for Role
        [JsonIgnore]
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        [Column("Status", TypeName = "int")]
        [Required]
        public UserStatus Status { get; set; } = UserStatus.Active;

        [Column("IsFirstLogin")]
        public bool IsFirstLogin { get; set; } = true;

        [Column("LastLogin", TypeName = "timestamp with time zone")]
        public DateTime? LastLogin { get; set; }

    }
}
