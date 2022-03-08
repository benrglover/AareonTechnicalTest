using AareonTechnicalTest.Models;
using AutoMapper;

namespace AareonTechnicalTest.Data
{
    public class TicketProfile: Profile
    {

        public TicketProfile()
        {
            this.CreateMap<Ticket, Ticket>()
                .ReverseMap();

        }

    }
}
