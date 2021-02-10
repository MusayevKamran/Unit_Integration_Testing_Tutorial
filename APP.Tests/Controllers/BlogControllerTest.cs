using APP.Controllers;
using APP.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP.Repositorys.Interfaces;
using Xunit;

namespace APP.Tests.Controllers
{
    public class BlogControllerTest
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly BlogController _blogController;
        private readonly List<Blog> _blogs;

        public BlogControllerTest()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
            _blogController = new BlogController(_mockBlogRepository.Object);
            _blogs = new List<Blog> {
                                                    new Blog{Id = 1,Title="c# Tutorial 1",Content="This is firt Blog",CategoryId =1},
                                                    new Blog{Id = 2,Title="c# Tutorial 2",Content="This is second Blog",CategoryId =1},
                                                    new Blog{Id = 3,Title="c# Tutorial 3",Content="This is third Blog",CategoryId =1}
                                                };
        }

        [Fact]
        public async Task Intex_ActionExecute_ReturnView()
        {
            var result = await _blogController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_ActionExecute_ReturnProductList()
        {
            _mockBlogRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_blogs);

            var result = await _blogController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var blogList = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.Model);

            Assert.Equal<int>(3, blogList.Count());

        }

        [Fact]
        public async Task Details_IdIsNull_ReturnToActionIndex()
        {
            var result = await _blogController.Details(null);

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Details_IdInValid_ReturnNotFound()
        {
            Blog blog = null;

            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync(blog);

            var result = await _blogController.Details(0);

            var redirect = Assert.IsType<NotFoundResult>(result);

            Assert.Equal<int>(404, redirect.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task Details_IdIsValid_ReturnBlog(int blogId)
        {
            Blog blog = _blogs.First(x => x.Id == blogId);

            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(blogId)).ReturnsAsync(blog);

            var result = await _blogController.Details(blogId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var resultBlog = Assert.IsAssignableFrom<Blog>(viewResult.Model);

            Assert.Equal(blog.Id, resultBlog.Id);
            Assert.Equal(blog.Title, resultBlog.Title);
        }

        [Fact]
        public void Create_ActionsExecute_ReturnView()
        {
            var result = _blogController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CreatePost_InValidModelState_ReturnView()
        {
            _blogController.ModelState.AddModelError("Title", "Title is required");

            var result = await _blogController.Create(_blogs.First());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<Blog>(viewResult.Model);
        }

        [Fact]
        public async Task CreatePost_ValidModelState_ReturnRedirectToActionIndex()
        {
            var result = await _blogController.Create(_blogs.First());

            var redirected = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirected.ActionName);
        }

        [Fact]
        public async Task CreatePost_ValidModelState_ExecuteCreateMethod()
        {
            Blog newBlog = null;

            _mockBlogRepository.Setup(repo => repo.Create(It.IsAny<Blog>()))
                                             .Callback<Blog>(val => newBlog = val);

            var result = await _blogController.Create(_blogs.First());

            _mockBlogRepository.Verify(repo => repo.Create(It.IsAny<Blog>()), Times.Once);

            Assert.Equal(_blogs.First().Id, newBlog.Id);
        }

        [Fact]
        public async Task CreatePost_InValidModelState_NeverExecuteCreateMethod()
        {
            _blogController.ModelState.AddModelError("Name", "");

            var result = await _blogController.Create(It.IsAny<Blog>());

            _mockBlogRepository.Verify(repo => repo.Create(It.IsAny<Blog>()), Times.Never);
        }

        [Fact]
        public async Task Edit_IdIsNull_ReturnNotFound()
        {
            var result = await _blogController.Edit(null);

            var redirected = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, redirected.StatusCode);
        }

        [Fact]
        public async Task Edit_IdInValid_ReturnNotFound()
        {
            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync((Blog)null);

            var result = await _blogController.Edit(0);

            var redirect = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, redirect.StatusCode);

        }

        [Theory]
        [InlineData(1)]
        public async Task Edit_IdIsValid_ReturnBlog(int blogId)
        {
            Blog blog = _blogs.First(obj => obj.Id == blogId);

            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(blogId)).ReturnsAsync(blog);

            var result = await _blogController.Edit(blogId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var viewResultBlog = Assert.IsAssignableFrom<Blog>(viewResult.Model);

            Assert.Equal(viewResultBlog.Id, blog.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task Edit_IdIsValid_ExecuteGetByIdAsyncMethod(int blogId)
        {
            Blog blog = _blogs.First(obj => obj.Id == blogId);

            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(blogId)).ReturnsAsync(blog);

            var result = await _blogController.Edit(blogId);

            _mockBlogRepository.Verify(repo => repo.GetByIdAsync(blogId), Times.Once);

            Assert.Equal(blogId, blog.Id);
        }

        
        [Theory]
        [InlineData(1)]
        public async Task EditPost_IdNotEqualBlogId_ReturnNotFoundAsync(int blogId)
        {
            Blog blog = _blogs.First(obj => obj.Id == 2);

            var result = await _blogController.Edit(blogId, blog);

            var redirected = Assert.IsType<NotFoundResult>(result);

            Assert.Equal(404, redirected.StatusCode);
        }

    }
}
