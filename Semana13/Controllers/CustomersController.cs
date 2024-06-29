using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Semana13.Models;

namespace Semana13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly DemoContext _context;

        public CustomersController(DemoContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest("The CustomerID in the URL does not match the CustomerID in the request body.");
            }

            var existingCustomer = await _context.Customers.FindAsync(id);

            if (existingCustomer == null)
            {
                return NotFound();
            }

            // Copiar propiedades del cliente actualizado al cliente existente
            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.DocumentNumber = customer.DocumentNumber;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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


        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'DemoContext.Customers' is null.");
            }

            // Agregar el nuevo cliente al contexto
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Devolver una respuesta con el estado 201 Created y la entidad del cliente creada
            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customer);
        }



        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Customers/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {
                return BadRequest("At least one of FirstName or LastName parameters is required.");
            }

            IQueryable<Customer> query = _context.Customers;

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                firstName = firstName.Trim().ToLower();
                query = query.Where(c => c.FirstName.ToLower().Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                lastName = lastName.Trim().ToLower();
                query = query.Where(c => c.LastName.ToLower().Contains(lastName));
            }

            var customers = await query.OrderByDescending(c => c.LastName).ToListAsync();

            if (customers == null || customers.Count == 0)
            {
                return NotFound();
            }

            return customers;
        }




        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerID == id)).GetValueOrDefault();
        }
    }
}
