#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CountryAPI.Data;
using CountryAPI.Models;

namespace CountryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(ProductListDto productDto)
        {
            try
            {
                return await _context.Products.Where(e => e.ref_assignto == productDto.ref_assignto).ToListAsync();
            }
            catch
            {
                return NotFound("Reference not valid");
            }
        }

        // GET: api/Products/5
        [HttpPost("read")]
        public async Task<ActionResult<Product>> GetProduct(ProductDto productDto)
        {

            try
            {
                var product = await _context.Products.Where(e => e.ref_assignto == productDto.ref_assignto && e.ref_product == productDto.ref_product).FirstAsync();
                return product;
            }
            catch
            {
                 return NotFound();
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<Product>> EditProduct(ProductDto productDto)
        {
            try
            {
                var product = await _context.Products.Where(e => e.ref_assignto == productDto.ref_assignto && e.ref_product == productDto.ref_product).FirstAsync();
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                _context.Entry(product).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return product;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productDto.ref_product))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch(DbUpdateConcurrencyException err)
            {   
                Console.WriteLine(err.Message);
                return BadRequest("Reference not valid");
            }
/*
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();*/
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Product>> PostProduct(Product request)
        {
            var newProduct = new Product
            {
                ref_assignto = request.ref_assignto,
                ref_product = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = newProduct.Id }, newProduct);
        }

        // DELETE: api/Products/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProduct(ProductDto productDto)
        {
            try
            {
                var product = await _context.Products.Where(e => e.ref_assignto == productDto.ref_assignto && e.ref_product == productDto.ref_product).FirstAsync();

                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok("Succesfully deleted.");
            }
            catch
            {
                return NotFound();
            }
        }

        private bool ProductExists(Guid ref_product)
        {
            return _context.Products.Any(e => e.ref_product == ref_product);
        }
    }
}
