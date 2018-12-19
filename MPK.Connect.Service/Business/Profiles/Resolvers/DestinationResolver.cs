using System.Linq;
using AutoMapper;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;

namespace MPK.Connect.Service.Business.Profiles.Resolvers
{
    public class DestinationResolver : IValueResolver<Path<StopTimeInfo>, TravelPlan, StopDto>
    {
        public StopDto Resolve(Path<StopTimeInfo> source, TravelPlan destination, StopDto member, ResolutionContext context)
        {
            return source.Any() ? source.Last().Stop : new StopDto();
        }
    }
}