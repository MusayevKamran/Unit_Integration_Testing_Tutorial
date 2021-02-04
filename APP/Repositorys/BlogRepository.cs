using APP.Data;
using APP.Models;
using APP.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APP.Repositorys
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(BlogContext context) : base(context) { }

    }
}
