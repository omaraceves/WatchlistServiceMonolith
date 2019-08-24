using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WatchlistMonolith.MappingProfiles
{
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<Entities.Media, Model.MediaModel>().ForMember(x => x.MediaGroup, opt => opt.MapFrom(
                src => $"{src.MediaGroup.Name}:{src.MediaGroupId}"))
                .ForMember(x => x.Title, opt => opt.MapFrom(src => $"{src.MediaGroup.Name} - {src.Title}"));

            CreateMap<Model.MediaForCreation, Entities.Media>();

            

            CreateMap<Entities.Watchlist, Model.UserModel>().ForMember(x => x.WatchLaterMedias, opt => opt.MapFrom(
                src => src.Medias.Select(m => m.Media)));

            CreateMap<Entities.User, Model.UserModel>();

            CreateMap<Model.UserForCreation, Entities.User>();

            CreateMap<Model.WatchlistEntry, Entities.WatchlistMedia>();

            CreateMap<Entities.Watchlist, Model.WatchlistModel>()
                .ForMember(dest => dest.WatchlistId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.WatchlistName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.MediaModel, opt => opt.MapFrom(src => src.Medias));

            CreateMap<Entities.WatchlistMedia, Model.MediaModel>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Media.Description))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Media.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => $"{src.Media.MediaGroup.Name} - {src.Media.Title}"))
                .ForMember(dest => dest.MediaGroup, opt => opt.MapFrom(src => $"{src.Media.MediaGroup.Name}:{src.Media.MediaGroupId}"));



        }
    }
}
