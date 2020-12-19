using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Demo5.Models
{
    public class Post : BaseEntity
    {
        public string title { get; set; }
        public string content { get; set; }
        public string imageName { get; set; }
        public DateTime postTime { get; set; }
        public DateTime updTime { get; set; }
        
        public string userId { get; set; }
        public IdentityUser user { get; set; }

        public int categoryId { get; set; }
        public Category category { get; set; }

        public ICollection<Comment> comments { get; set; }
    }
}
