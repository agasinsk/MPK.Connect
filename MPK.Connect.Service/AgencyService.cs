using Microsoft.Extensions.Logging;
using MPK.Connect.DataAccess.Agencies;
using MPK.Connect.Model;
using System;

namespace MPK.Connect.Service
{
    public class AgencyService : ImporterService<Agency>
    {
        private readonly ILogger<AgencyService> _logger;
        private readonly IAgencyRepository _repository;

        public AgencyService(IAgencyRepository repository, ILogger<AgencyService> logger) : base(repository, logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Agency Map(string entityString)
        {
            var routeTypeInfos = entityString.Split(',');
            var id = routeTypeInfos[0];
            var name = routeTypeInfos[1];
            var url = routeTypeInfos[2];
            var timeZone = routeTypeInfos[3];
            var phone = routeTypeInfos[4];
            var language = routeTypeInfos[5];
            var agency = new Agency
            {
                Id = id,
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