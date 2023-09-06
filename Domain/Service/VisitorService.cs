using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using dto = Domain.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.MAPPER;
using Domain.Interface;

namespace Domain.Service
{
    public class VisitorService : IVisitorService
    {
        private readonly Ecommerce_AppContext _dbContext;

        public VisitorService(Ecommerce_AppContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasVisitedToday(string ipAddress, DateTime visitDate)
        {
            return await _dbContext.Visitors
                .AnyAsync(vc => vc.Ipaddress == ipAddress && vc.VisitDate == visitDate);
        }

        public async Task AddOrUpdateVisitor(string ipAddress, DateTime visitDate)
        {
            var visitor = await _dbContext.Visitors
                .FirstOrDefaultAsync(vc => vc.Ipaddress == ipAddress && vc.VisitDate == visitDate);

            if (visitor == null)
            {
                var newVisitor = new dto.Visitors
                {
                    Ipaddress = ipAddress,
                    VisitDate = visitDate,
                };

                 _dbContext.Visitors.Add(MapVisitors.MAP(newVisitor));
                 await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<int> GetVisitorsCount()
        {
            var visitors = await _dbContext.Visitors.CountAsync();
            return visitors;
        }
    }
}
