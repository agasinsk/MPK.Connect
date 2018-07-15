using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Agencies;
using MPK.Connect.DataAccess.Routes.Types;
using MPK.Connect.Model;

namespace MPK.Connect.Service.Service
{
    public class AgencyService : IGenericService<Agency>
    {
        private readonly ILogger<AgencyService> _logger;
        private readonly IAgencyRepository _repository;

        public AgencyService(IAgencyRepository repository, ILogger<AgencyService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int ReadFromFile(string filePath)
        {
            var agencies = new List<Agency>();
            var totalCount = 0;

            using (var streamReader = new StreamReader(filePath))
            {
                var routeTypeString = streamReader.ReadLine();

                while ((routeTypeString = streamReader.ReadLine()) != null)
                {
                    var mappedAgency = Map(routeTypeString);
                    agencies.Add(mappedAgency);
                }
                _logger.LogInformation($"Read {agencies.Count} Agencies.");
                _logger.LogInformation("Sorting Agencies by Id...");
                agencies.Sort((route1, route2) => route1.Id.CompareTo(route2.Id));
                _logger.LogInformation("Agencies types have been sorted!");

                _repository.AddRange(agencies);
                _repository.Save();
                totalCount = agencies.Count;
                agencies.Clear();
                _logger.LogInformation("Agencies have been successfully saved!");
            }

            return totalCount;
        }

        private Agency Map(string routeString)
        {
            var routeTypeInfos = routeString.Split(',');
            var id = int.Parse(routeTypeInfos[0]);
            var name = routeTypeInfos[1];
            var url = routeTypeInfos[2];
            var timeZone = routeTypeInfos[3];
            var phone = routeTypeInfos[4];
            var language = routeTypeInfos[5];
            var agency = new Agency
            {
                AgencyId = id,
                Name = name,
                Url = url,
                Phone = phone,
                Timezone = timeZone,
                Language = language
            };
            return agency;
        }
    }
}