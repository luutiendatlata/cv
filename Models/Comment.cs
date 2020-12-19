using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Demo5.Models
{
    public class Comment : BaseEntity
    {
        public string content { get; set; }
        public int postId { get; set; }
        public Post post { get; set; }
        public string userId { get; set; }
        public IdentityUser user { get; set; }
    }
}
