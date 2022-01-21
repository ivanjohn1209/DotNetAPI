using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CountryAPI.Models;
using Microsoft.EntityFrameworkCore;
using CountryAPI.Data;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountryAPI.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class CountryItemsController : ControllerBase
    {
        private readonly DataContext _context;
        public CountryItemsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/<CountryItemsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryItem>>> GetCountryItems()
        {
            return await _context.CountryItems.ToListAsync();
        }

        // GET api/<CountryItemsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CountryItem>>> GetCountryItem(long id)
        {
            var countryItem = await _context.CountryItems.FindAsync(id);
            if(countryItem == null)
            {
               return NotFound();
            }
            return Ok(countryItem);
        }

        // POST api/<CountryItemsController>
        /* [HttpPost]
         public void Post([FromBody] string value)
         {
         }*/
        //POST api/<CountryItemsController>
        [HttpPost]
        public async Task<ActionResult<CountryItem>> CreateTodoItem(CountryItem countryItemDTO)
        {
            var countryItem = new CountryItem
            {
                IsDone = countryItemDTO.IsDone,
                Name = countryItemDTO.Name
            };

            _context.CountryItems.Add(countryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCountryItem), new { id = countryItem.Id }, CountryItemToDTO(countryItem));
        }

        /* // PUT api/<CountryItemsController>/5
         [HttpPut("{id}")]
         public void Put(int id, [FromBody] string value)
         {
         }*/
        // PUT api/<CountryItemsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, CountryItem countryItem)
        {
            if (id != countryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(countryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCountryItem), new { id = countryItem.Id }, CountryItemToDTO(countryItem));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryItemExists(id))
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
        // DELETE api/<CountryItemsController>/5
        /*[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
            
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.CountryItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.CountryItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool CountryItemExists(long id)
        {
            return _context.CountryItems.Any(e => e.Id == id);
        }

        private static CountryItem CountryItemToDTO(CountryItem todoItem) =>
            new CountryItem
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsDone = todoItem.IsDone
            };

    }
}
