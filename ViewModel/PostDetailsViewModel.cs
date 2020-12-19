using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo5.Models;
namespace Demo5.ViewModel
{
    public class PostDetailsViewModel
    {
        public Post posts { get; set; }
        public List<Comment>  comments { get; set; }
        public Comment newComment { get; set; }
    }
}
