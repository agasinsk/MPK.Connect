using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MPK.Connect.Model;

namespace MPK.Connect.DataAccess
{
    public class StopsRepository : IStopsRepository
    {
        private readonly MpkContext _dbContext;
        private readonly ILogger<StopsRepository> _logger;

        public StopsRepository(MpkContext dbContext, ILogger<StopsRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Stop CreateStop(Stop stop)
        {
            _logger.LogInformation($"Creating new stop with id {stop.Id}");
            _dbContext.Stops.Add(stop);
            return stop;
        }

        public List<Stop> CreateStops(List<Stop> stops)
        {
            _logger.LogInformation($"Creating {stops.Count} new stops");
            _dbContext.Stops.AddRange(stops);

            _dbContext.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT dbo.Stops ON");
            _dbContext.SaveChanges();
            _dbContext.Database.ExecuteSqlCommand($"SET IDENTITY_INSERT dbo.Stops OFF");

            return stops;
        }

        public Stop DeleteStop(Stop stop)
        {
            _logger.LogInformation($"Deleting stop with id {stop.Id}");
            _dbContext.Remove(stop);
            _dbContext.SaveChanges();
            return stop;
        }

        public Stop GetStop(int stopId)
        {
            return _dbContext.Stops.FirstOrDefault(s => s.Id == stopId);
        }

        public Stop UpdateStop(Stop stop)
        {
            _logger.LogInformation($"Updating stop with id {stop.Id}");
            _dbContext.SaveChanges();
            return stop;
        }
    }
}