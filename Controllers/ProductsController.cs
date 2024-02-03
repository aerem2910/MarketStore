using Microsoft.AspNetCore.Mvc;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreMarket.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext storeContext;

        public ProductsController(StoreContext context)
        {
            storeContext = context;
        }

        [HttpGet]
        [Route("products/{id}")]
        public ActionResult<ProductResponse> GetProduct(int id)
        {
            var result = storeContext.Products.FirstOrDefault(p => p.Id == id);

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new ProductResponse(result));
            }
        }

        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var result = storeContext.Products;

            return Ok(result.Select(product => new ProductResponse(product)));
        }

        [HttpPost]
        [Route("products")]
        public ActionResult<ProductResponse> AddProduct(ProductCreateRequest request)
        {
            Product product = request.ProductGetEntity();

            try
            {
                var result = storeContext.Products.Add(product).Entity;

                storeContext.SaveChanges();
                return Ok(new ProductResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("products/{id}")]
        public ActionResult<ProductResponse> UpdateProduct(int id, ProductUpdateRequest request)
        {
            var product = storeContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            storeContext.SaveChanges();

            return Ok(new ProductResponse(product));
        }

        [HttpDelete]
        [Route("products/{id}")]
        public ActionResult<ProductResponse> DeleteProduct(int id)
        {
            var product = storeContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            storeContext.Products.Remove(product);
            storeContext.SaveChanges();

            return Ok(new ProductResponse(product));
        }

        [HttpPost]
        [Route("groups")]
        public ActionResult<GroupResponse> AddGroup(GroupCreateRequest request)
        {
            Group group = new Group { Count = request.Count };

            try
            {
                var result = storeContext.Groups.Add(group).Entity;
                storeContext.SaveChanges();
                return Ok(new GroupResponse(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("groups/{id}")]
        public ActionResult<GroupResponse> DeleteGroup(int id)
        {
            var group = storeContext.Groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            storeContext.Groups.Remove(group);
            storeContext.SaveChanges();

            return Ok(new GroupResponse(group));
        }

        [HttpPut]
        [Route("products/{id}/price")]
        public ActionResult<ProductResponse> SetProductPrice(int id, decimal price)
        {
            var product = storeContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.Price = price;
            storeContext.SaveChanges();

            return Ok(new ProductResponse(product));
        }
    }
}
