using BMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.DTO
{
    public class PostCommentDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public List<Comment>? Comments { get; set; }
    }
}
