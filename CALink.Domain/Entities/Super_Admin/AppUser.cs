using CALink.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Entities.Super_Admin
{
    [Table("AppUsers")]
    [Index(nameof(Email), IsUnique = true)]
    public class AppUser : BaseEntity
    {
        [Required]
        [Column("FirstName", TypeName = "varchar(100)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column("LastName", TypeName = "varchar(100)")]
        public string LastName { get; set; } = string.Empty;

        [Column("Email", TypeName = "varchar(200)")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("Password", TypeName = "varchar(300)")]
        public string Password { get; set; } = string.Empty;

        [Column("IsActive", TypeName = "boolean")]
        public bool IsActive { get; set; } = true;

        [Column("LastLogin", TypeName = "timestamp with time zone")]
        public DateTime? LastLogin { get; set; }

        [Column("ProfilePictureUrl", TypeName = "varchar(500)")]
        [MaxLength(500)]
        public string? ProfilePictureUrl { get; set; }

        [Column("RefreshToken", TypeName = "text")]
        public string? RefreshToken { get; set; }
    }
}
