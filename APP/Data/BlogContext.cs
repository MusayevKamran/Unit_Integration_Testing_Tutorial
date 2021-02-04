using Microsoft.EntityFrameworkCore;
using APP.Models;

namespace APP.Data
{
    public sealed class BlogContext : DbContext
    {
        public  DbSet<Blog> Blogs { get; set; }
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
    }
}
