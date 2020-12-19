using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Demo5.Models
{
    
    public class Category : BaseEntity
    {
        [Required]
        [Display(Name = "Category Name")]
        public string name { get; set; }
        public ICollection<Post> posts { get; set; }

    }
}
