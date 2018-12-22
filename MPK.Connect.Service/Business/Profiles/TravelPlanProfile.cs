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
            CreateMap<Stop, StopDto>()
                .ForMember(dst => dst.Id, st => st.MapFrom(src => src.Id))
                .ForMember(dst => dst.Code, st => st.MapFrom(src => src.Code))
                .ForMember(dst => dst.Name, st => st.MapFrom(src => src.Name))
                .ForMember(dst => dst.Longitude, st => st.MapFrom(src => src.Longitude))
                .ForMember(dst => dst.Latitude, st => st.MapFrom(src => src.Latitude))
                .ForAllOtherMembers(dst => dst.Ignore());

            CreateMap<StopTime, StopTimeInfo>()
                .ForMember(dst => dst.Id, st => st.MapFrom(src => src.Id))
                .ForMember(dst => dst.StopId, st => st.MapFrom(src => src.StopId))
                .ForMember(dst => dst.ArrivalTime, st => st.MapFrom(src => src.ArrivalTime))
                .ForMember(dst => dst.DepartureTime, st => st.MapFrom(src => src.DepartureTime))
                .ForMember(dst => dst.StopSequence, st => st.MapFrom(src => src.StopSequence))
                .ForMember(dst => dst.StopDto, st => st.MapFrom(src => src.Stop))
                .ForMember(dst => dst.RouteType, st => st.MapFrom(src => src.Trip.Route.Type))
                .ForMember(dst => dst.Direction, st => st.MapFrom(src => src.Trip.HeadSign))
                .ForMember(dst => dst.DirectionId, st => st.MapFrom(src => src.Trip.DirectionId))
                .ForAllOtherMembers(dst => dst.Ignore());

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
                .ForMember(dst => dst.Name, st => st.MapFrom(src => src.StopDto.Name))
                .ForMember(dst => dst.Longitude, st => st.MapFrom(src => src.StopDto.Longitude))
                .ForMember(dst => dst.Latitude, st => st.MapFrom(src => src.StopDto.Latitude))
                .ForAllOtherMembers(dst => dst.Ignore());

            CreateMap<Path<StopTimeInfo>, TravelPlan>()
                .ForMember(dst => dst.Destination, tp => tp.MapFrom(src => src.Any() ? src.Last().StopDto : null))
                .ForMember(dst => dst.Source, tp => tp.MapFrom(src => src.Any() ? src.First().StopDto : null))
                .ForMember(dst => dst.StartTime, tp => tp.MapFrom(src => src.Any() ? src.First().DepartureTime.ToDateTime() : DateTime.MinValue))
                .ForMember(dst => dst.EndTime, tp => tp.MapFrom(src => src.Any() ? src.Last().DepartureTime.ToDateTime() : DateTime.MinValue))
                .ForMember(dst => dst.Duration, tp => tp.MapFrom(src => src.Cost))
                .ForMember(dst => dst.RouteIds, tp => tp.MapFrom(src => src.Select(sti => sti.Route).Distinct()))
                .ForMember(dst => dst.Transfers, tp => tp.MapFrom(src => src.Select(sti => sti.Route).Distinct().Count()))
                .ForMember(dst => dst.Stops, tp => tp.MapFrom(src => src))
                .ForAllOtherMembers(dst => dst.Ignore());
        }
    }
}