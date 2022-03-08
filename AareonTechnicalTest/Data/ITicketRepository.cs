using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AareonTechnicalTest.Models;

namespace AareonTechnicalTest.Data
{        
    public interface ITicketRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        Task<Ticket[]> GetAllTicketsAsync();
        Task<Ticket> GetTicketAsync(int id);

    }

}
