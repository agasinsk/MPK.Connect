using System;
using System.Collections.Generic;
using System.Linq;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;

namespace MPK.Connect.Service.Business
{
    public class StopService : IStopService
    {
        private readonly IGenericRepository<Stop> _stopRepository;

        public StopService(IGenericRepository<Stop> stopRepository)
        {
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
        }

        public List<StopDto> GetAllStops()
        {
            return _stopRepository.GetAll()
                .Select(s => new StopDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude
                })
                .ToList();
        }

        public StopDto GetStopById(string stopId)
        {
            return _stopRepository.FindBy(s => s.Id == stopId)
                .Select(s => new StopDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude
                })
                .FirstOrDefault();
        }

        public List<StopDto> GetStopByName(string stopName)
        {
            return _stopRepository.FindBy(s => s.Name == stopName)
                .Select(s => new StopDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude
                })
                .ToList();
        }
    }
}