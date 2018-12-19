using System;
using System.Linq;
using AutoMapper;
using MPK.Connect.Model;
using MPK.Connect.Model.Business;
using MPK.Connect.Model.Business.TravelPlan;
using MPK.Connect.Model.Graph;
using MPK.Connect.Service.Helpers;

namespace MPK.Connect.Service.Business.Profiles
{
    public class TravelPlanProfile : Profile
    {
        public TravelPlanProfile()
        {
            CreateMap<StopDto, Location>()
                .ForMember(dst => dst.Name, st => st.MapFrom(src => src.Name))
                .ForMember(dst => dst.Longitude, st => st.MapFrom(src => src.Longitude))
                .ForMember(dst => dst.Latitude, st => st.MapFrom(src => src.Latitude))
                .ForAllOtherMembers(dst => dst.Ignore());

            CreateMap<Stop, Location>()
                .ForMember(dst => dst.Name, st => st.MapFrom(src => src.Name))
                .ForMember(dst => dst.Longitude, st => st.MapFrom(src => src.Longitude))
                .ForMember(dst => dst.Latitude, st => st.MapFrom(src => src.Latitude))
                .ForAllOtherMembers(dst => dst.Ignore());

            CreateMap<StopTimeInfo, Location>()
                .ForMember(dst => dst.Name, st => st.MapFrom(src => src.Stop.Name))
                .ForMember(dst => dst.Longitude, st => st.MapFrom(src => src.Stop.Longitude))
                .ForMember(dst => dst.Latitude, st => st.MapFrom(src => src.Stop.Latitude))
                .ForAllOtherMembers(dst => dst.Ignore());

            CreateMap<Path<StopTimeInfo>, TravelPlan>()
                .ForMember(dst => dst.Destination, tp => tp.MapFrom(src => src.Any() ? src.Last().Stop : null))
                .ForMember(dst => dst.Source, tp => tp.MapFrom(src => src.Any() ? src.First().Stop : null))
                .ForMember(dst => dst.StartTime, tp => tp.MapFrom(src => src.Any() ? src.First().DepartureTime.ToDateTime() : DateTime.MinValue))
                .ForMember(dst => dst.EndTime, tp => tp.MapFrom(src => src.Any() ? src.Last().DepartureTime.ToDateTime() : DateTime.MinValue))
                .ForMember(dst => dst.Duration, tp => tp.MapFrom(src => src.Cost))
                .ForMember(dst => dst.RouteIds, tp => tp.MapFrom(src => src.Select(sti => sti.Route).Distinct()))
                .ForMember(dst => dst.Stops, tp => tp.MapFrom(src => src))
                .ForAllOtherMembers(dst => dst.Ignore());
        }
    }
}