using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string? Email { get; set; }

        public string Contet { get; set; }

        public DateTime Created { get; set; }

        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post Post { get; set; }

    }
}
