using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AareonTechnicalTest.Models;


namespace AareonTechnicalTest.Data
{
    public class TicketRepository : ITicketRepository
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<TicketRepository> _logger;

        public TicketRepository(ApplicationContext context, ILogger<TicketRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save the changes in the context");

            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Ticket[]> GetAllTicketsAsync()
        {
            _logger.LogInformation($"Getting all Tickets");

            IQueryable<Ticket> query = _context.Tickets;
                
            query = query.OrderByDescending(c => c.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Ticket> GetTicketAsync(int id)
        {
            _logger.LogInformation($"Getting a Ticket for {id}");

            IQueryable<Ticket> query = _context.Tickets;
                
            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

    }
}
