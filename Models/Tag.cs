using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo5.Models
{
    public class Tag :BaseEntity
    {
        [Required]
        [Display(Name ="Tag name")]
        public string name { get; set; }
    }
}
