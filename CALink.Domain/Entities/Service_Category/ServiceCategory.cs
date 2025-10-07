using CALink.Domain.Entities.Common;
using CALink.Domain.Entities.Super_Admin;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static CALink.Domain.Enums.Enums;

namespace CALink.Domain.Entities.Service_Category
{
    [Table("ServiceCategory")]
    public class ServiceCategory : BaseEntity
    {
        [Column("CompanyId", TypeName = "uuid")]
        [Required]
        public Guid CompanyId { get; set; }

        [JsonIgnore]
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        [Column("CategoryName", TypeName = "varchar(200)")]
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Column("CategoryCode", TypeName = "varchar(200)")]
        [Required]
        public string CategoryCode { get; set; } = string.Empty;

        [Column("Description", TypeName = "varchar(500)")]
        public string Description { get; set; } = string.Empty;

        [Column("Status", TypeName = "int")]
        [Required]
        public CategoryStatus Status { get; set; } = CategoryStatus.Active;
    }
}
