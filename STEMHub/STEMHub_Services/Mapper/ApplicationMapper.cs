using AutoMapper;
using STEMHub.STEMHub_Data.Entities;
using STEMHub.STEMHub_Data.DTO;

namespace STEMHub.STEMHub_Services.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        {
            CreateMap<Banner, BannerDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();
            CreateMap<NewspaperArticle, NewspaperArticleDto>().ReverseMap();
            CreateMap<STEM, STEMDto>().ReverseMap();
            CreateMap<Topic, TopicDto>().ReverseMap();
            CreateMap<Video, VideoDto>().ReverseMap();
            CreateMap<Ingredients, IngredientsDto>().ReverseMap();
        }
    }
}
