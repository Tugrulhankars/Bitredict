using AutoMapper;
using Bitredict.Dtos.Request;
using Bitredict.Dtos.Response;
using Bitredict.Models;

namespace Bitredict.Services.Profiles;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<AddMatchRequest, Match>().ReverseMap();
        CreateMap<AddMatchResponse, Match>().ReverseMap();
        CreateMap<AddMatchResponse, AddMatchRequest>().ReverseMap();

        CreateMap<UpdateMatchRequest, Match>().ReverseMap();
        CreateMap<UpdateMatchResponse, Match>().ReverseMap();
        CreateMap<UpdateMatchResponse, UpdateMatchRequest>().ReverseMap();

        CreateMap<GetAllMatchResponse, Match>().ReverseMap();
        CreateMap<GetAllMatchResponse, AddMatchResponse>().ReverseMap();

        CreateMap<DeleteMatchRequest, Match>().ReverseMap();

        CreateMap<CreateUserRequest, CreateUserResponse>().ReverseMap();

        CreateMap<GetMatchCenterStatistics,List< MatchCenterStatistics>>().ReverseMap();
        
        CreateMap<GetHomePageStatistics, List<HomePageStatistics>>().ReverseMap();

        CreateMap<CreateUserRequest, User>().ReverseMap();

        CreateMap<GetBetOfTheDatMatchReponse, Match>().ReverseMap();


        CreateMap<GetBetOfTheDayBakers, Match>().ReverseMap();
        CreateMap<GetBetOfTheDaySlipOfTheDay, Match>().ReverseMap();
        // CreateMap<AddBetOfTheDay, Match>().ReverseMap();
        CreateMap<AddBetOfTheDay, Match>()
     .ForMember(dest => dest.League, opt => opt.Ignore())
     .ForMember(dest => dest.MatchStatus, opt => opt.Ignore())
     .ForMember(dest => dest.Teams, opt => opt.Ignore())
     .ForMember(dest => dest.MatchDate, opt => opt.Ignore())
     .ForMember(dest => dest.Odds1, opt => opt.Ignore())
     .ForMember(dest => dest.OddsX, opt => opt.Ignore())
     .ForMember(dest => dest.Odds2, opt => opt.Ignore())
     .ForMember(dest => dest.Tip, opt => opt.Ignore())
     .ForMember(dest => dest.Goals, opt => opt.Ignore())
     .ForMember(dest => dest.Gg, opt => opt.Ignore())
     .ForMember(dest => dest.BestTip, opt => opt.Ignore())
     .ForMember(dest => dest.Trust, opt => opt.Ignore())
     .ReverseMap();

        CreateMap<Match, GetBetOfTheDayBankersAndSlipOfTheDay>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.Teams, opt => opt.MapFrom(src => src.Teams))
             .ForMember(dest => dest.MatchDate, opt => opt.MapFrom(src => src.MatchDate))
             .ForMember(dest => dest.Odds1, opt => opt.MapFrom(src => src.Odds1))
             .ForMember(dest => dest.OddsX, opt => opt.MapFrom(src => src.OddsX))
             .ForMember(dest => dest.Odds2, opt => opt.MapFrom(src => src.Odds2))
             .ForMember(dest => dest.Tip, opt => opt.MapFrom(src => src.Tip))
             .ForMember(dest => dest.Goals, opt => opt.MapFrom(src => src.Goals))
             .ForMember(dest => dest.Gg, opt => opt.MapFrom(src => src.Gg))
             .ForMember(dest => dest.BestTip, opt => opt.MapFrom(src => src.BestTip))
             .ForMember(dest => dest.Trust, opt => opt.MapFrom(src => src.Trust));



    }
}
