using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Models
{
    public class Blog : Entity
    {
        [Required]
        public string   Title { get; set; }
        [Required]
        public string   Content { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
