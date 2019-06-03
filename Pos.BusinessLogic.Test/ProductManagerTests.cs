using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pos.BusinessLogic.Dto;
using Pos.DataAccess;
using Pos.DataAccess.Entities;
using Pos.Utility;
using Xunit;

namespace Pos.BusinessLogic.Test
{

    /// <summary>
    /// This test class uses moq library for db sets
    /// </summary>
    public class ProductManagerTests
    {
        private readonly ProductManager _productManager;

        public ProductManagerTests()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Code = "M001",
                    Name = "T-Shirt",
                    Price = (decimal) 49.99
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Code = "M002",
                    Name = "Jean",
                    Price = (decimal) 149.99
                }

            }.AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(products.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());

            var mockContext = new Mock<PosDbContext>(new DbContextOptions<PosDbContext>(), new HttpContextAccessor());
            mockContext.Setup(c => c.Products).Returns(mockSet.Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UnitTestMappingProfile());
            });
            var mapper = config.CreateMapper();
            _productManager = new ProductManager(mockContext.Object, mapper);
        }

        [Fact]
        public void Test_Get_With_Paging()
        {
            var pagedProducts = _productManager.GetWithPaging(1, 5);

            Assert.Equal(1, pagedProducts.CurrentPage);
            Assert.Equal(1, pagedProducts.FirstRowOnPage);
            Assert.Equal(2, pagedProducts.LastRowOnPage);
            Assert.Equal(1, pagedProducts.PageCount);
            Assert.Equal(5, pagedProducts.PageSize);
            Assert.Equal(2, pagedProducts.RowCount);
            Assert.False(pagedProducts.Results.IsNullOrEmpty());
        }

        [Theory]
        [InlineData("M001")]
        [InlineData("M003")]
        public void Test_Add(string productCode)
        {
            var newProductId = Guid.NewGuid();
            var newProductDto = new ProductDto
            {
                Id = newProductId,
                Code = productCode,
                Name = "Test-Product",
                Price = (decimal) 99.99
            };
            var result = _productManager.Add(newProductDto);

            if (productCode == "M001") Assert.False(result.IsSuccess);
            else Assert.True(result.IsSuccess);
        }
    }
}
