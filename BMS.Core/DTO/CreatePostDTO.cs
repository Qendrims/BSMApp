using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BMS.Core.DTO
{
    public class CreatePostDTO
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<IFormFile> Images {get; set; }

        public List<string>? Tags { get; set; }  
    }
}
