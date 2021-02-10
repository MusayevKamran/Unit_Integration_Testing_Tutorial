using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APP.Data;
using APP.Models;
using APP.Repositorys;
using APP.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APP.Tests.Repositories
{
    public class BlogRepositoryTest
    {
        #region Fields
        private readonly IBlogRepository _blogRepository;

        #endregion

        #region Constructor
        public BlogRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase("InMemoryBlogDatabase").Options;

            _blogRepository = new BlogRepository(new BlogContext(options));

            SaveBlogsInMemory();
        }

        #endregion

        #region Test Metods 
        [Fact]
        public async Task GetAllAsync_ReturnAllBlogs()
        {
            var firstBlog = new Blog
            {
                Id = 1,
                Title = "First Title",
                Content = "First Content",
                CategoryId = 1
            };

            List<Blog> blogs = await _blogRepository.GetAllAsync();

            Assert.Equal(firstBlog.Id, blogs.First().Id);
            Assert.Equal(3, blogs.Count);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetByIdAsync_One_ReturnBlogWithIdOne(int blogId)
        {
            Blog result = await _blogRepository.GetByIdAsync(blogId);

            Assert.Equal(1, result.Id);
            Assert.Equal("First Title", result.Title);
            Assert.Equal("First Content", result.Content);
            Assert.Equal(1, result.CategoryId);
        }

        [Fact]
        public async Task Create_NewBlog_AddNewBlogInMemoryDatabase()
        {
            var newBlog = new Blog
            {
                Id = 4,
                Title = "New Title",
                Content = "New Content",
                CategoryId = 4
            };

            _blogRepository.Create(newBlog);
            await _blogRepository.SaveAsync();

            List<Blog> blogs = await _blogRepository.GetAllAsync();

            Assert.Equal(4, blogs.Count);


        }

        [Theory]
        [InlineData(1)]
        public async Task Update_BlogWithIdOne_UpdateBlogInMemoryDatabase(int blogId)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);

            blog.Title = "Updated Blog";
            blog.Content = "updated Category";
            blog.CategoryId = 1;

            _blogRepository.Update(blog);
            await _blogRepository.SaveAsync();

            var updateBlog = await _blogRepository.GetByIdAsync(blogId);

            Assert.Equal(blog, updateBlog);

        }

        [Theory]
        [InlineData(1)]
        public async Task Delete_Blog_ReturnDeletedBlogList(int blogId)
        {
            var blog = await _blogRepository.GetByIdAsync(blogId);

            _blogRepository.Delete(blog);
            await _blogRepository.SaveAsync();

            List<Blog> blogs = await _blogRepository.GetAllAsync();

            Assert.Equal(2, blogs.Count);
        }
        #endregion

        #region Private methods
        private void SaveBlogsInMemory()
        {
            _blogRepository.Create(new Blog
            {
                Id = 1,
                Title = "First Title",
                Content = "First Content",
                CategoryId = 1
            });

            _blogRepository.Create(new Blog
            {
                Id = 2,
                Title = "Second Title",
                Content = "Second Content",
                CategoryId = 2
            });

            _blogRepository.Create(new Blog
            {
                Id = 3,
                Title = "Third Title",
                Content = "Third Content",
                CategoryId = 3
            });

            _blogRepository.Save();
        }

        #endregion
    }
}
