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
using static CALink.Domain.Enums.Enums;

namespace CALink.Domain.Entities.User_Management
{
    [Table("Roles")]
    public class Role : BaseEntity
    {
        [Column("CompanyId", TypeName = "uuid")]
        [Required]
        public Guid CompanyId { get; set; }

        [JsonIgnore]
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        [Required]
        [Column("RoleName", TypeName = "varchar(200)")]
        public string RoleName { get; set; } = string.Empty;

        [Column("Description", TypeName = "varchar(500)")]     
        public string? Description { get; set; }

        [Column("Status", TypeName = "int")]
        [Required]
        public RoleStatus Status { get; set; } = RoleStatus.Active;

    }
}
