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
    public class PriceController : ControllerBase
    {
        private readonly DataContext _context;

        public PriceController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Prices
        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<Price>>> GetPrices(PriceListDto priceListDto)
        {
            var priceList = await (from p in _context.Prices
                                      where p.ref_assignto == priceListDto.ref_assignto
                                      select new Price
                                      {
                                          Id = p.Id,
                                          ref_assignto = p.ref_assignto,
                                          ref_price = p.ref_price,
                                          price = p.price,
                                          currency = p.currency,
                                      }
                                ).ToListAsync();
            return priceList;
        }

        // GET: api/Prices/5
        [HttpPost("read")]
        public async Task<ActionResult<Price>> GetPrice(Price price)
        {
            try
            {
                var priceItem = await (from p in _context.Prices
                                       where p.ref_assignto == price.ref_assignto && p.ref_price == price.ref_price

                                       select new Price
                                       {
                                           Id = p.Id,
                                           ref_assignto = p.ref_assignto,
                                           ref_price = p.ref_price,
                                           price = p.price,
                                           currency = p.currency,
                                       }
                             ).FirstAsync();
                return priceItem;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Prices/5
        // To protectid from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<Price>> PutPrice(PriceDto price)
        {
            try
            {
                var p = await _context.Prices.Where(e => e.ref_assignto == price.ref_assignto && e.ref_price == price.ref_price).FirstAsync();
                p.price = price.price;
                p.currency = price.currency;
                _context.Entry(p).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return p;
                }
                catch (DbUpdateConcurrencyException err)
                {
                    return BadRequest(err.Message);
                }
            }
            catch (DbUpdateConcurrencyException err)
            {
                return BadRequest(err.Message);
            }
        }

        // POST: api/Prices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Price>> PostPrice(Price request)
        {
            var newPrice = new Price
            {
                ref_assignto = request.ref_assignto,
                ref_price = Guid.NewGuid(),
                price = request.price,
                currency = request.currency
            };
            _context.Prices.Add(newPrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrice", new { id = request.Id }, request);
        }

        // DELETE: api/Prices/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeletePrice(PriceDto priceDto)
        {
            try
            {
                var p = await _context.Prices.Where(e => e.ref_assignto == priceDto.ref_assignto && e.ref_price == priceDto.ref_price).FirstAsync();
                _context.Prices.Remove(p);
                await _context.SaveChangesAsync();
                return Ok("Succesfully deleted.");

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private bool PriceExists(int id)
        {
            return _context.Prices.Any(e => e.Id == id);
        }
    }
}
