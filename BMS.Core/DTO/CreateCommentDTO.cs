using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.DTO
{
    public class CreateCommentDTO
    {
        public string AuthorName { get; set; }

        public string? AuthorEmail { get; set; }

        public string Comment { get; set; } 

        public int PostId { get; set; }
    }
}
