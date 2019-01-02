using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MPK.Connect.DataAccess;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPK.Connect.Service.Business
{
    public class StopService : IStopService
    {
        private readonly IGenericRepository<Stop> _stopRepository;
        private readonly IMapper _mapper;

        public StopService(IGenericRepository<Stop> stopRepository, IMapper mapper)
        {
            _stopRepository = stopRepository ?? throw new ArgumentNullException(nameof(stopRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
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

        public StopDto GetStopById(int stopId)
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

        public List<StopDto> GetDistinctStopsByName()
        {
            return _stopRepository.GetAll()
                .Select(s => _mapper.Map<StopDto>(s))
                .AsNoTracking()
                .ToList()
                .GroupBy(s => s.Name)
                .Select(g => g.First())
                .ToList();
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