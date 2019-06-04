using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pos.BusinessLogic.Dto;
using Pos.Core.Enum;
using Pos.DataAccess;
using Pos.DataAccess.Entities;
using Pos.Utility;
using Xunit;

namespace Pos.BusinessLogic.Test
{
    /// <summary>
    /// This test class uses inmemory database for db sets.
    /// </summary>
    public class OrderManagerTests
    {
        private readonly OrderManager _orderManager;

        public OrderManagerTests()
        {
            var options = new DbContextOptionsBuilder<PosDbContext>()
                .UseInMemoryDatabase(databaseName: "PosDb")
                .Options;

            var context = new PosDbContext(options, new HttpContextAccessor());
            SeedDatabase(context);

            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new UnitTestMappingProfile()); });
            var mapper = config.CreateMapper();
            _orderManager = new OrderManager(context, mapper);
        }

        [Fact]
        public void Test_Add_ProductNotFound()
        {
            var newOrderDto = new NewOrderDto
            {
                CustomerName = "TestCustomer",
                CampaignCode = "CMP01",
                ProductList = new List<NewOrderProductDto>
                {
                    new NewOrderProductDto
                    {
                        ProductCode = "M001",
                        Quantity = 1
                    },
                    new NewOrderProductDto
                    {
                        ProductCode = "Z001",
                        Quantity = 1
                    }
                }
            };

            var result = _orderManager.Add(newOrderDto);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Test_Add_InvalidCampaignCode()
        {
            var newOrderDto = new NewOrderDto
            {
                CustomerName = "TestCustomer",
                CampaignCode = "CMP91",
                ProductList = new List<NewOrderProductDto>
                {
                    new NewOrderProductDto
                    {
                        ProductCode = "M001",
                        Quantity = 2
                    }
                }
            };

            var result = _orderManager.Add(newOrderDto);

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Test_Add_CampaignUsageCountExceeded()
        {
            var newOrderDto = new NewOrderDto
            {
                CustomerName = "TestCustomer",
                CampaignCode = "CMP01",
                ProductList = new List<NewOrderProductDto>
                {
                    new NewOrderProductDto
                    {
                        ProductCode = "M001",
                        Quantity = 2
                    }
                }
            };

            var result = _orderManager.Add(newOrderDto);
            Assert.True(result.IsSuccess);

            result = _orderManager.Add(newOrderDto);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Test_Add_Success()
        {
            var newOrderDto = new NewOrderDto
            {
                CustomerName = "TestCustomer",
                CampaignCode = "CMP01",
                ProductList = new List<NewOrderProductDto>
                {
                    new NewOrderProductDto
                    {
                        ProductCode = "M001",
                        Quantity = 1
                    },
                    new NewOrderProductDto
                    {
                        ProductCode = "M002",
                        Quantity = 2
                    }
                }
            };

            var result = _orderManager.Add(newOrderDto);

            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_Get(bool isExists)
        {
            var orderId = Guid.NewGuid();
            if (isExists)
            {
                var newOrderDto = new NewOrderDto
                {
                    CustomerName = "TestCustomer",
                    ProductList = new List<NewOrderProductDto>
                    {
                        new NewOrderProductDto
                        {
                            ProductCode = "M001",
                            Quantity = 1
                        },
                    }
                };

                var result = _orderManager.Add(newOrderDto);
                Assert.True(result.IsSuccess);
                orderId = result.Id;
            }

            var orderDto = _orderManager.Get(orderId);
            var testResult = isExists == (orderDto != null && orderDto.IsSuccess && orderDto.Id == orderId);
            Assert.True(testResult);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Test_GetOrderItems(bool isExists)
        {
            var orderId = Guid.NewGuid();
            if (isExists)
            {
                var newOrderDto = new NewOrderDto
                {
                    CustomerName = "TestCustomer",
                    ProductList = new List<NewOrderProductDto>
                    {
                        new NewOrderProductDto
                        {
                            ProductCode = "M001",
                            Quantity = 1
                        },
                    }
                };

                var result = _orderManager.Add(newOrderDto);
                Assert.True(result.IsSuccess);
                orderId = result.Id;
            }

            var listOfOrderItems = _orderManager.GetOrderItems(orderId);
            var testResult = isExists == (listOfOrderItems != null && listOfOrderItems.Count > 0);
            Assert.True(testResult);
        }

        private void SeedDatabase(PosDbContext db)
        {
            var hasher = new CryptoHelper();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Users.Add(new User
            {
                Id = Guid.Parse("523755D5-E791-4FEA-B4EC-412E123E66F4"),
                FirstName = "Gokcan",
                LastName = "Ustun",
                Email = "gokcan.ustun@yandex.com",
                UserName = "admin",
                Password = hasher.Hash("1"),
            });
            db.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Code = "M001",
                Name = "T-Shirt",
                Price = 49.99.ToDecimal()
            });
            db.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Code = "M002",
                Name = "Jean",
                Price = 149.99.ToDecimal()
            });
            db.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Code = "M003",
                Name = "Skirt",
                Price = 79.99.ToDecimal()
            });

            db.Campaigns.Add(new Campaign
            {
                Id = Guid.NewGuid(),
                Code = "CMP01",
                Name = "%10 Discount",
                MaxUsageCount = 3,
                UsageCount = 2,
                DiscounType = EDiscountType.Ratio,
                DiscountValue = 10
            });
            db.Campaigns.Add(new Campaign
            {
                Id = Guid.NewGuid(),
                Code = "CMP02",
                Name = "5$ Discount",
                DiscounType = EDiscountType.Amount,
                DiscountValue = 5
            });

            db.SaveChanges();
        }
    }
}

