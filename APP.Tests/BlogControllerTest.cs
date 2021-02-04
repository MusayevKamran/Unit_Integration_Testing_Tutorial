using APP.Controllers;
using APP.Models;
using APP.Repositorys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace APP.Tests
{
    public class BlogControllerTest
    {
        private readonly Mock<IBlogRepository> _mockBlogRepository;
        private readonly BlogController _blogController;
        private List<Blog> blogs;

        public BlogControllerTest()
        {
            _mockBlogRepository = new Mock<IBlogRepository>();
            _blogController = new BlogController(_mockBlogRepository.Object);
            blogs = new List<Blog> {
                                                    new Blog{Id = 1,Title="c# Tutorial 1",Content="This is firt Blog",CategoryId =1},
                                                    new Blog{Id = 1,Title="c# Tutorial 2",Content="This is second Blog",CategoryId =1},
                                                    new Blog{Id = 1,Title="c# Tutorial 3",Content="This is third Blog",CategoryId =1}
                                                };
        }

        [Fact]
        public async void Intex_ActionExecute_ReturnView()
        {
            var result = await _blogController.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ActionExecute_ReturnProductList()
        {
            _mockBlogRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(blogs);

            var result = await _blogController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var blogList = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.Model);

            Assert.Equal<int>(3, blogList.Count());

        }

        [Fact]
        public async void Details_IdIsNull_ReturnToActionIndex()
        {
            var result = await _blogController.Details(null);

            var redirect = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async void Details_IdInValid_ReturnNotFound()
        {
            Blog blog = null;

            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(0)).ReturnsAsync(blog);

            var result = await _blogController.Details(0);

            var redirect = Assert.IsType<NotFoundResult>(result);

            Assert.Equal<int>(404, redirect.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void Details_ValidId_ReturnBlog(int blogId)
        {
            Blog blog = blogs.First(x => x.Id == blogId);

            _mockBlogRepository.Setup(repo => repo.GetByIdAsync(blogId)).ReturnsAsync(blog);

            var result = await _blogController.Details(blogId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var resultBlog = Assert.IsAssignableFrom<Blog>(viewResult.Model);

            Assert.Equal(blog.Id, resultBlog.Id);
            Assert.Equal(blog.Title, resultBlog.Title);


        }
    }
}
