using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CALink.Application.DTOs.Common_Dto
{
    public class EmailDto
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool IsBodyHtml { get; set; } = false;
        public string ReplyTo { get; set; }        
    }
}
