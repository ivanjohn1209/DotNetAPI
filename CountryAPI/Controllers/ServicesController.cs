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
    public class ServicesController : ControllerBase
    {
        private readonly DataContext _context;

        public ServicesController(DataContext context)
        {
            _context = context;
        }

        // Post: api/Services/list
        [HttpPost("list")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices(ServiceListDto serviceDto)
        {
            try
            {
                return await _context.Services.Where(e => e.ref_assignto == serviceDto.ref_assignto).ToListAsync();
            }
            catch
            {
                return NotFound("Reference not valid");
            }
        }


        // GET: api/Services/5
        [HttpPost("read")]
        public async Task<ActionResult<Service>> GetService(ServiceDto serviceDto)
        {

            try
            {
                var sercvice = await _context.Services.Where(e => e.ref_assignto == serviceDto.ref_assignto && e.ref_service == serviceDto.ref_service).FirstAsync();
                return sercvice;
            }
            catch
            {
                return NotFound("Reference not valid");
            }
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("update")]
        public async Task<ActionResult<Service>> PutService(ServiceDto serviceDto)
        {
            try
            {
                var service = await _context.Services.Where(e => e.ref_assignto == serviceDto.ref_assignto && e.ref_service == serviceDto.ref_service).FirstAsync();
                service.Name = serviceDto.Name;
                service.Description = serviceDto.Description;
                _context.Entry(service).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    return service;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ref_assignto))
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
            catch (DbUpdateConcurrencyException err)
            {
                return BadRequest("Reference not valid");
            }
            /* if (id != service.Id)
             {
                 return BadRequest();
             }

             _context.Entry(service).State = EntityState.Modified;

             try
             {
                 await _context.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!ServiceExists(id))
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

        // POST: api/Services/add
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Service>> PostService(Service request)
        {
            var newService = new Service
            {
                ref_assignto = request.ref_assignto,
                ref_service = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };
            _context.Services.Add(newService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetService", new { id = newService.Id }, newService);
        }

        // DELETE: api/Services/5
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteService(ServiceDto serviceDto)
        {
            try
            {
                var service = await _context.Services.Where(e => e.ref_assignto == serviceDto.ref_assignto && e.ref_service == serviceDto.ref_service).FirstAsync();

                if (service == null)
                {
                    return NotFound();
                }

                _context.Services.Remove(service);
                await _context.SaveChangesAsync();

                return Ok("Succesfully deleted.");
            }
            catch
            {
                return NotFound();
            }
        }

        private bool ServiceExists(Guid ref_service)
        {
            return _context.Services.Any(e => e.ref_service == ref_service);
        }
    }
}
