using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Business
{
    public class StopService : IStopService
    {
        private readonly IGenericRepository<Stop> _stopRepository;

        public StopService(IGenericRepository<Stop> stopRepository)
        {
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
        }

        public List<Stop> GetAllStops()
        {
            return _stopRepository.GetAll().ToList();
        }

        public Stop GetStopById(string stopId)
        {
            return _stopRepository.FindBy(s => s.Id == stopId).FirstOrDefault();
        }

        public List<Stop> GetStopByName(string stopName)
        {
            return _stopRepository.FindBy(s => s.Name == stopName).ToList();
        }
    }
}