using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Models
{
    public class Blog : Entity
    {
        public string   Title { get; set; }
        public string   Content { get; set; }
        public int CategoryId { get; set; }
    }
}
