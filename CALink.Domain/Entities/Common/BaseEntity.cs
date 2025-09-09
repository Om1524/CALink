using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Domain.Entities.Common
{
    public class BaseEntity
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("CreatedBy", TypeName = "uuid")]
        public Guid CreatedBy { get; set; }

        [Column("UpdatedBy", TypeName = "uuid")]
        public Guid UpdatedBy { get; set; }

        [Column("CreatedAt", TypeName = "timestamp with time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt", TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; }
    }
}
